Feature: NoteService
  In order to manage notes
  As a user
  I want to create, update, delete, and tag notes

  Scenario: Create a new note
    When I create a new note
    Then the note should be created successfully

  Scenario: Update a note with valid content
    Given a note with title "Test Title" and content "Test Content"
    When I update the note
    Then the note should be updated successfully

  Scenario: Update a note with empty title and content
    Given a note with empty title and content
    When I update the note
    Then the note should not have any tags

  Scenario: Delete a note
    Given a note with id "12345"
    When I delete the note
    Then the note should be deleted successfully

  Scenario: Get a note by id
    Given a note with id "12345"
    When I get the note by id
    Then the note should be retrieved successfully