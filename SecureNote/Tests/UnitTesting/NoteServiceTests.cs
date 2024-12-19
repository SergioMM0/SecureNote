using API.Application.Interfaces;
using API.Application.Interfaces.Repositories;
using API.Application.Services;
using API.Core.Domain.Context;
using API.Core.Domain.Entities;
using FluentAssertions;
using Moq;

public class NoteServiceTagMethodTests
{
    private readonly Mock<INoteRepository> _mockNoteRepo;
    private readonly Mock<ITagRepository> _mockTagRepo;
    private readonly Mock<IAppDbContext> _mockDbContext;
    private readonly Mock<CurrentContext> _mockCurrentContext;
    private readonly NoteService _service;

    public NoteServiceTagMethodTests()
    {
        _mockNoteRepo = new Mock<INoteRepository>();
        _mockTagRepo = new Mock<ITagRepository>();
        _mockDbContext = new Mock<IAppDbContext>();
        _mockCurrentContext = new Mock<CurrentContext>();

        var tags = new List<Tag>
        {
            new Tag { Name = "Animals", Keywords = new[] { "cat", "dog" } },
            new Tag { Name = "Travel", Keywords = new[] { "vacation", "trip" } },
            new Tag { Name = "Food", Keywords = new[] { "pizza", "burger" } }
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
    public async Task Tag_WhenNoTitleAndNoContent_ReturnsEmptyArray()
    {
        var note = new Note
        {
            Title = "",
            Content = ""
        };

        var result = await _service.Tag(note);
        result.Should().BeEmpty("because no tags should be assigned if both title and content are empty.");
    }

    [Fact]
    public async Task Tag_WhenContentContainsCat_ShouldReturnAnimalsTag()
    {
        var note = new Note
        {
            Title = "My lovely kitty",
            Content = "I adopted a new cat yesterday!"
        };

        var result = await _service.Tag(note);
        result.Should().ContainSingle("Animals");
    }

    [Fact]
    public async Task Tag_WhenMultipleTagsMatch_ShouldReturnAllMatchingTags()
    {
        var note = new Note
        {
            Title = "Vacation and pets",
            Content = "I went on a trip with my cat. It was a fun vacation!"
        };

        var result = await _service.Tag(note);
        result.Should().Contain(new[] { "Animals", "Travel" }).And.HaveCount(2);
    }

    [Fact]
    public async Task Tag_WhenNoMatchingKeywords_ReturnsEmpty()
    {
        var note = new Note
        {
            Title = "Random Title",
            Content = "Just some random text that matches no known keywords."
        };

        var result = await _service.Tag(note);
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Tag_WhenTitleMatchesFoodKeywords_ShouldReturnFood()
    {
        var note = new Note
        {
            Title = "Pizza Party",
            Content = "We had a blast last night."
        };

        var result = await _service.Tag(note);
        result.Should().Contain("Food");
    }
}
