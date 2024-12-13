using System;
using API.Application.Services;
using API.Infrastructure.Repositories;
using Domain;
using TechTalk.SpecFlow;
using Xunit;

namespace Tests.Specs.StepDefinitions
{
    [Binding]
    public class NoteServiceSteps
    {
        private Note _note;
        private NoteService _noteService;
        private bool _operationResult;

        public NoteServiceSteps()
        {
            _noteService = new NoteService(new NoteRepository(), new TagRepository());
        }

        [When(@"I create a new note")]
        public void WhenICreateANewNote()
        {
            _note = _noteService.Create();
            _note.Title = "New Note";
            _note.Content = "New Content";
            _note = _noteService.Update(_note);
            _operationResult = _note != null;
        }

        [Then(@"the note should be created successfully")]
        public void ThenTheNoteShouldBeCreatedSuccessfully()
        {
            Assert.True(_operationResult);
        }

        [Given(@"a note with title ""(.*)"" and content ""(.*)""")]
        public void GivenANoteWithTitleAndContent(string title, string content)
        {
            _note = _noteService.Create();
            _note.Title = title;
            _note.Content = content;
            _note = _noteService.Update(_note);
        }

        [When(@"I update the note")]
        public void WhenIUpdateTheNote()
        {
            _note.Title = "Updated Title";
            _note.Content = "Updated Content";
            _note = _noteService.Update(_note);
            _operationResult = _note != null;
        }

        [Then(@"the note should be updated successfully")]
        public void ThenTheNoteShouldBeUpdatedSuccessfully()
        {
            Assert.True(_operationResult);
        }

        [Given(@"a note with empty title and content")]
        public void GivenANoteWithEmptyTitleAndContent()
        {
            _note = _noteService.Create();
            _note.Title = string.Empty;
            _note.Content = string.Empty;
            _note = _noteService.Update(_note);
        }

        [Then(@"the note should not have any tags")]
        public void ThenTheNoteShouldNotHaveAnyTags()
        {
            Assert.Empty(_note.Tags);
        }

        [Given(@"a note with id ""(.*)""")]
        public void GivenANoteWithId(string id)
        {
            _note = new Note { Id = Guid.Parse(id), Title = "Test Title", Content = "Test Content" };
            _note = _noteService.Update(_note);
        }

        [When(@"I delete the note")]
        public void WhenIDeleteTheNote()
        {
            _noteService.Delete(_note.Id);
            _operationResult = _noteService.Get(_note.Id) == null;
        }

        [Then(@"the note should be deleted successfully")]
        public void ThenTheNoteShouldBeDeletedSuccessfully()
        {
            Assert.True(_operationResult);
        }

        [When(@"I get the note by id")]
        public void WhenIGetTheNoteById()
        {
            _note = _noteService.Get(_note.Id);
        }

        [Then(@"the note should be retrieved successfully")]
        public void ThenTheNoteShouldBeRetrievedSuccessfully()
        {
            Assert.NotNull(_note);
        }
    }
}
