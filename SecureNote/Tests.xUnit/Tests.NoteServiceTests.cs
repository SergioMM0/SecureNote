using API.Application.Interfaces.Repositories;
using API.Application.Services;
using Domain;
using Moq;
using Xunit;

namespace Tests.xUnit
{
    public class NoteServiceTests
    {
        private readonly Mock<INoteRepository> _noteRepositoryMock;
        private readonly Mock<ITagRepository> _tagRepositoryMock;
        private readonly NoteService _noteService;

        public NoteServiceTests()
        {
            _noteRepositoryMock = new Mock<INoteRepository>();
            _tagRepositoryMock = new Mock<ITagRepository>();
            _noteService = new NoteService(_noteRepositoryMock.Object, _tagRepositoryMock.Object);
        }

        [Fact]
        public void Create_CreatesAndReturnsNewNote()
        {
            // Arrange
            var newNote = new Note();
            _noteRepositoryMock.Setup(repo => repo.Create(It.IsAny<Note>())).Returns(newNote);

            // Act
            var result = _noteService.Create();

            // Assert
            Assert.NotNull(result);
            _noteRepositoryMock.Verify(repo => repo.Create(It.IsAny<Note>()), Times.Once);
        }

        [Fact]
        public void Update_UpdatesNoteAndReturnsUpdatedNote()
        {
            // Arrange
            var note = new Note { Title = "Test", Content = "Content" };
            var updatedNote = new Note { Title = "Test", Content = "Content", Tags = new[] { "TestTag" } };
            _noteRepositoryMock.Setup(repo => repo.Update(It.IsAny<Note>())).Returns(updatedNote);
            _tagRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Tag>
            {
                new Tag { Name = "TestTag", Keywords = new[] { "test" } }
            });

            // Act
            var result = _noteService.Update(note);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedNote.Tags, result.Tags);
            _noteRepositoryMock.Verify(repo => repo.Update(It.IsAny<Note>()), Times.Once);
        }

        [Fact]
        public void Delete_DeletesNote()
        {
            // Arrange
            var noteId = Guid.NewGuid();

            // Act
            _noteService.Delete(noteId);

            // Assert
            _noteRepositoryMock.Verify(repo => repo.Delete(noteId), Times.Once);
        }

        [Fact]
        public void Get_ReturnsNoteById()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note { Id = noteId };
            _noteRepositoryMock.Setup(repo => repo.Get(noteId)).Returns(note);

            // Act
            var result = _noteService.Get(noteId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(noteId, result.Id);
            _noteRepositoryMock.Verify(repo => repo.Get(noteId), Times.Once);
        }

        [Fact]
        public void Tag_EmptyNote_ReturnsEmptyArray()
        {
            // Arrange
            var note = new Note { Title = "", Content = "" };

            // Act
            var result = _noteService.Tag(note);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Tag_SingleTagMatchInTitle_ReturnsTag()
        {
            // Arrange
            var note = new Note { Title = "Shopping List", Content = "" };
            _tagRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Tag>
            {
                new Tag { Name = "Shopping", Keywords = new[] { "shopping" } }
            });

            // Act
            var result = _noteService.Tag(note);

            // Assert
            Assert.Single(result);
            Assert.Contains("Shopping", result);
        }

        [Fact]
        public void Tag_SingleTagMatchInContent_ReturnsTag()
        {
            // Arrange
            var note = new Note { Title = "", Content = "Milk, eggs, bread" };
            _tagRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Tag>
            {
                new Tag { Name = "Shopping", Keywords = new[] { "milk" } }
            });

            // Act
            var result = _noteService.Tag(note);

            // Assert
            Assert.Single(result);
            Assert.Contains("Shopping", result);
        }

        [Fact]
        public void Tag_MatchInBothTitleAndContent_ReturnsTags()
        {
            // Arrange
            var note = new Note { Title = "Reminder", Content = "Cleaning" };
            _tagRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Tag>
            {
                new Tag { Name = "Home", Keywords = new[] { "reminder" } },
                new Tag { Name = "Chores", Keywords = new[] { "cleaning" } }
            });

            // Act
            var result = _noteService.Tag(note);

            // Assert
            Assert.Equal(2, result.Length);
            Assert.Contains("Home", result);
            Assert.Contains("Chores", result);
        }

        [Fact]
        public void Tag_NoMatch_ReturnsEmptyArray()
        {
            // Arrange
            var note = new Note { Title = "Test", Content = "123" };
            _tagRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Tag>
            {
                new Tag { Name = "Shopping", Keywords = new[] { "milk" } }
            });

            // Act
            var result = _noteService.Tag(note);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Tag_CaseInsensitiveMatching_ReturnsTag()
        {
            // Arrange
            var note = new Note { Title = "shopping list", Content = "Milk, Eggs, bread" };
            _tagRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Tag>
            {
                new Tag { Name = "Shopping", Keywords = new[] { "milk", "eggs" } }
            });

            // Act
            var result = _noteService.Tag(note);

            // Assert
            Assert.Single(result);
            Assert.Contains("Shopping", result);
        }

        [Fact]
        public void Tag_MultipleKeywordsInOneTag_ReturnsTag()
        {
            // Arrange
            var note = new Note { Title = "Football Match", Content = "ball, goal, stadium" };
            _tagRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Tag>
            {
                new Tag { Name = "Sports", Keywords = new[] { "ball", "goal", "stadium" } }
            });

            // Act
            var result = _noteService.Tag(note);

            // Assert
            Assert.Single(result);
            Assert.Contains("Sports", result);
        }

        [Fact]
        public void Tag_DuplicateKeywords_ReturnsSingleTag()
        {
            // Arrange
            var note = new Note { Title = "Milk Milk Milk", Content = "Milk" };
            _tagRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Tag>
            {
                new Tag { Name = "Dairy", Keywords = new[] { "milk" } }
            });

            // Act
            var result = _noteService.Tag(note);

            // Assert
            Assert.Single(result);
            Assert.Contains("Dairy", result);
        }
    }
}

