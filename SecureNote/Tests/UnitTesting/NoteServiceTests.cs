using API.Application.Interfaces;
using API.Application.Interfaces.Repositories;
using API.Application.Services;
using API.Core.Domain.Context;
using API.Core.Domain.Entities;
using API.Core.Domain.Exception;
using FluentAssertions;
using Moq;

/// <summary>
/// Unit tests for the NoteService class.
/// These tests verify the correct functionality of tagging and other business logic.
/// </summary>
public class NoteServiceTests {
    private readonly Mock<INoteRepository> _mockNoteRepo;
    private readonly Mock<ITagRepository> _mockTagRepo;
    private readonly Mock<IAppDbContext> _mockDbContext;
    private readonly Mock<CurrentContext> _mockCurrentContext;
    private readonly NoteService _service;

    /// <summary>
    /// Initializes mocks and sets up the test environment.
    /// </summary>
    public NoteServiceTests() {
        _mockNoteRepo = new Mock<INoteRepository>();
        _mockTagRepo = new Mock<ITagRepository>();
        _mockDbContext = new Mock<IAppDbContext>();
        _mockCurrentContext = new Mock<CurrentContext>();

        var tags = new List<Tag> {
            new Tag { Name = "Work", Keywords = new[] { "job", "office", "project", "career" } },
            new Tag { Name = "Personal", Keywords = new[] { "diary", "self", "reflection" } },
            new Tag { Name = "Shopping", Keywords = new[] { "groceries", "store", "buy" } },
            new Tag { Name = "Ideas", Keywords = new[] { "brainstorm", "concepts", "inspiration" } },
            new Tag { Name = "Fitness", Keywords = new[] { "exercise", "gym", "health" } },
            new Tag { Name = "Travel", Keywords = new[] { "vacation", "trip", "adventure" } },
            new Tag { Name = "Finance", Keywords = new[] { "money", "budget", "expenses" } },
            new Tag { Name = "Learning", Keywords = new[] { "study", "education", "skills" } }
        };

        _mockTagRepo.Setup(r => r.GetAll()).ReturnsAsync(tags);

        _service = new NoteService(
            _mockNoteRepo.Object,
            _mockTagRepo.Object,
            _mockDbContext.Object,
            _mockCurrentContext.Object
        );
    }

    [Fact]
    public async Task Tag_WhenNoTitleAndNoContent_ReturnsEmptyArray() {
        // Arrange
        var note = new Note {
            Title = "",
            Content = ""
        };

        // Act
        var result = await _service.Tag(note);

        // Assert
        result.Should().BeEmpty("because no tags should be assigned if both title and content are empty.");
    }

    [Fact]
    public async Task Tag_WhenContentContainsWorkKeyword_ShouldReturnWorkTag() {
        // Arrange
        var note = new Note {
            Title = "Office project",
            Content = "I have a new project at work."
        };

        // Act
        var result = await _service.Tag(note);

        // Assert
        result.Should().ContainSingle().Which.Should().Be("Work");
    }

    [Fact]
    public async Task Tag_WhenContentMatchesMultipleTags_ShouldReturnAllMatchingTags() {
        // Arrange
        var note = new Note {
            Title = "Health and Fitness",
            Content = "I started a gym routine and a new exercise program."
        };

        // Act
        var result = await _service.Tag(note);

        // Assert
        result.Should().Contain(new[] { "Fitness" });
    }

    [Fact]
    public async Task Tag_WhenTitleMatchesShoppingKeyword_ShouldReturnShoppingTag() {
        // Arrange
        var note = new Note {
            Title = "Groceries list",
            Content = "Buy milk, eggs, and bread."
        };

        // Act
        var result = await _service.Tag(note);

        // Assert
        result.Should().Contain("Shopping");
    }

    [Fact]
    public async Task Tag_WhenNoteIsNSFW_BlursContentAndTags() {
        // Arrange
        var note = new Note {
            Id = Guid.NewGuid(),
            Title = "Sensitive Content",
            Content = "This note contains NSFW information.",
            Nsfw = true
        };

        _mockNoteRepo.Setup(repo => repo.Get(note.Id)).ReturnsAsync(note);

        // Act
        var blurredNote = await _service.Get(note.Id);

        // Assert
        blurredNote.Should().NotBeNull();
        blurredNote!.Content.Should().BeNull("because NSFW notes should have their content blurred.");
        blurredNote.Tags.Should().BeNull("because NSFW notes should have their tags blurred.");
    }

    [Fact]
    public async Task Tag_WhenNoMatchingKeywords_ReturnsEmpty() {
        // Arrange
        var note = new Note {
            Title = "Random Title",
            Content = "Unrelated content that matches no tags."
        };

        // Act
        var result = await _service.Tag(note);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Tag_WhenLearningKeywordsMatch_ShouldReturnLearningTag() {
        // Arrange
        var note = new Note {
            Title = "Education Plan",
            Content = "Studying new skills and taking online courses."
        };

        // Act
        var result = await _service.Tag(note);

        // Assert
        result.Should().Contain("Learning");
    }
    
    [Fact]
    public async Task Create_ShouldReturnNewNote() {
        // Arrange
        var userId = Guid.NewGuid();
        _mockCurrentContext.Setup(ctx => ctx.UserId).Returns(userId);

        var newNote = new Note { Id = Guid.NewGuid(), UserId = userId };
        _mockNoteRepo.Setup(repo => repo.Create(It.IsAny<Note>())).ReturnsAsync(newNote);

        // Act
        var result = await _service.Create();

        // Assert
        result.Should().NotBeNull();
        result.UserId.Should().Be(userId);
        _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Get_ShouldReturnNote_WhenIdExists() {
        // Arrange
        var noteId = Guid.NewGuid();
        var note = new Note { Id = noteId, Title = "Test Note", Content = "This is a test note." };
        _mockNoteRepo.Setup(repo => repo.Get(noteId)).ReturnsAsync(note);

        // Act
        var result = await _service.Get(noteId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(noteId);
    }

    [Fact]
    public async Task Get_ShouldReturnNull_WhenIdDoesNotExist() {
        // Arrange
        var noteId = Guid.NewGuid();
        _mockNoteRepo.Setup(repo => repo.Get(noteId)).ReturnsAsync((Note?)null);

        // Act
        var result = await _service.Get(noteId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Update_ShouldUpdateNote_WhenIdExists() {
        // Arrange
        var noteId = Guid.NewGuid();
        var existingNote = new Note { Id = noteId, Title = "Old Title", Content = "Old Content" };
        var updatedNote = new Note { Id = noteId, Title = "New Title", Content = "New Content" };

        _mockNoteRepo.Setup(repo => repo.Get(noteId)).ReturnsAsync(existingNote);
        _mockDbContext.Setup(db => db.Attach(existingNote));

        // Act
        var result = await _service.Update(updatedNote);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(noteId);
        result.Title.Should().Be(updatedNote.Title);
        result.Content.Should().Be(updatedNote.Content);
        _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Update_ShouldThrowNotFoundException_WhenIdDoesNotExist() {
        // Arrange
        var noteId = Guid.NewGuid();
        var updatedNote = new Note { Id = noteId, Title = "New Title", Content = "New Content" };

        _mockNoteRepo.Setup(repo => repo.Get(noteId)).ReturnsAsync((Note?)null);

        // Act
        var act = async () => await _service.Update(updatedNote);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Note with id: {noteId} not found.");
    }

    [Fact]
    public async Task Delete_ShouldRemoveNote_WhenIdExists() {
        // Arrange
        var noteId = Guid.NewGuid();
        var note = new Note { Id = noteId };
        _mockNoteRepo.Setup(repo => repo.Get(noteId)).ReturnsAsync(note);

        // Act
        await _service.Delete(noteId);

        // Assert
        _mockNoteRepo.Verify(repo => repo.Delete(note), Times.Once);
        _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldThrowNotFoundException_WhenIdDoesNotExist() {
        // Arrange
        var noteId = Guid.NewGuid();
        _mockNoteRepo.Setup(repo => repo.Get(noteId)).ReturnsAsync((Note?)null);

        // Act
        var act = async () => await _service.Delete(noteId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Note with id: {noteId} not found.");
    }
}
