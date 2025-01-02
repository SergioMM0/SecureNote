using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Interfaces.Repositories;
using API.Application.Services;
using API.Core.Domain.Entities;
using API.Core.Interfaces;
using Moq;
using Reqnroll;
using Xunit;

namespace CucumberTesting.StepDefinitions
{
    [Binding]
    public class NoteServiceTagMethodStepDefinitions
    {
        private Note? _note;
        private List<Tag> _tags;
        private readonly Mock<INoteService> _noteServiceMock;

        public NoteServiceTagMethodStepDefinitions()
        {
            _tags = new List<Tag>();
            _noteServiceMock = new Mock<INoteService>();
        }

        [Given("a note with the title {string} and the content {string}")]
        public void GivenANoteWithTheTitleAndTheContent(string title, string content)
        {
            // Handle null or empty strings
            title = title == "null" ? null : title;
            content = content == "null" ? null : content;

            // Create a new note with the given title and content
            _note = new Note
            {
                Title = title,
                Content = content,
                Tags = Array.Empty<string>()
            };
        }

        [Given("a note with the title null and the content null")]
        public void GivenANoteWithTheTitleNullAndTheContentNull()
        {
            // Create a new note with null title and content
            _note = new Note
            {
                Title = null,
                Content = null,
                Tags = Array.Empty<string>()
            };
        }

        [Given("a note with the title {string} and the content null")]
        public void GivenANoteWithTheTitleAndTheContentNull(string title)
        {
            // Create a new note with the given title and null content
            _note = new Note
            {
                Title = title,
                Content = null,
                Tags = Array.Empty<string>()
            };
        }

        [Given("a note with the title null and the content {string}")]
        public void GivenANoteWithTheTitleNullAndTheContent(string content)
        {
            // Create a new note with null title and the given content
            _note = new Note
            {
                Title = null,
                Content = content,
                Tags = Array.Empty<string>()
            };
        }

        [Given("the following tags exist in the repository:")]
        public void GivenTheFollowingTagsExistInTheRepository(Table table)
        {
            _tags = table.CreateSet<Tag>().ToList();
        }

        [When("the note is tagged")]
        public async Task WhenTheNoteIsTagged()
        {
            // Mock the tag repository to return the given tags
            var tagRepositoryMock = new Mock<ITagRepository>();
            tagRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(_tags);

            // Create an instance of NoteService with the mocked repository
            var noteService = new NoteService(null!, tagRepositoryMock.Object, null!, null!);

            // Tag the note
            _note!.Tags = await noteService.Tag(_note);
        }

        [Then("the note should have the following tags:")]
        public void ThenTheNoteShouldHaveTheFollowingTags(Table table)
        {
            var expectedTags = table.Rows.Select(row => row[0]).ToList();
            var actualTags = _note!.Tags!.ToList();

            Assert.Equal(expectedTags, actualTags);
        }
    }
}


