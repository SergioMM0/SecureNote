Feature: Tagging Notes

  # Both title and content empty
  Scenario: Both title and content empty
    Given a note with the title "" and the content ""
    And the following tags exist in the repository:
      | Name    | Keywords                 |
      | Task    | todo, task, work         |
      | Urgent  | important, asap          |
      | Meeting | meeting, discuss, review |
    When the note is tagged
    Then the note should have the following tags:
      |                      |

  # Both title and content whitespace
  Scenario: Both title and content whitespace
    Given a note with the title " " and the content "  "
    And the following tags exist in the repository:
      | Name    | Keywords                 |
      | Task    | todo, task, work         |
      | Urgent  | important, asap          |
      | Meeting | meeting, discuss, review |
    When the note is tagged
    Then the note should have the following tags:
      |                      |

  # Both title and content null
  Scenario: Both title and content null
    Given a note with the title null and the content null
    And the following tags exist in the repository:
      | Name    | Keywords                 |
      | Task    | todo, task, work         |
      | Urgent  | important, asap          |
      | Meeting | meeting, discuss, review |
    When the note is tagged
    Then the note should have the following tags:
      |                      |

  # Title empty, content with keyword
  Scenario: Title empty, content with keyword
    Given a note with the title "" and the content "This is a task"
    And the following tags exist in the repository:
      | Name    | Keywords                 |
      | Task    | todo, task, work         |
      | Urgent  | important, asap          |
      | Meeting | meeting, discuss, review |
    When the note is tagged
    Then the note should have the following tags:
      | Task                 |

  # Title null, content with keywords
  Scenario: Title null, content with keywords
    Given a note with the title null and the content "Important meeting"
    And the following tags exist in the repository:
      | Name    | Keywords                 |
      | Task    | todo, task, work         |
      | Urgent  | important, asap          |
      | Meeting | meeting, discuss, review |
    When the note is tagged
    Then the note should have the following tags:
      | Urgent, Meeting      |

  # Content empty, title with keywords
  Scenario: Content empty, title with keywords
    Given a note with the title "Urgent task" and the content ""
    And the following tags exist in the repository:
      | Name    | Keywords                 |
      | Task    | todo, task, work         |
      | Urgent  | important, asap          |
      | Meeting | meeting, discuss, review |
    When the note is tagged
    Then the note should have the following tags:
      | Task, Urgent         |

  # Content null, title with keyword
  Scenario: Content null, title with keyword
    Given a note with the title "Meeting notes" and the content null
    And the following tags exist in the repository:
      | Name    | Keywords                 |
      | Task    | todo, task, work         |
      | Urgent  | important, asap          |
      | Meeting | meeting, discuss, review |
    When the note is tagged
    Then the note should have the following tags:
      | Meeting              |

  # Both with same keywords
  Scenario: Both with same keywords
    Given a note with the title "Urgent task" and the content "This is a task"
    And the following tags exist in the repository:
      | Name    | Keywords                 |
      | Task    | todo, task, work         |
      | Urgent  | important, asap          |
      | Meeting | meeting, discuss, review |
    When the note is tagged
    Then the note should have the following tags:
      | Task, Urgent         |

  # Both with different keywords for same tag
  Scenario: Both with different keywords for same tag
    Given a note with the title "Meeting notes" and the content "Discuss and review"
    And the following tags exist in the repository:
      | Name    | Keywords                 |
      | Task    | todo, task, work         |
      | Urgent  | important, asap          |
      | Meeting | meeting, discuss, review |
    When the note is tagged
    Then the note should have the following tags:
      | Meeting              |

  # Both with different keywords for different tags
  Scenario: Both with different keywords for different tags
    Given a note with the title "Urgent meeting" and the content "This is a task"
    And the following tags exist in the repository:
      | Name    | Keywords                 |
      | Task    | todo, task, work         |
      | Urgent  | important, asap          |
      | Meeting | meeting, discuss, review |
    When the note is tagged
    Then the note should have the following tags:
      | Task, Urgent, Meeting|

  # Content with keyword, title without
  Scenario: Content with keyword, title without
    Given a note with the title "Grocery Shopping" and the content "Buy milk, eggs, and bread"
    And the following tags exist in the repository:
      | Name    | Keywords                 |
      | Task    | todo, task, work         |
      | Urgent  | important, asap          |
      | Meeting | meeting, discuss, review |
    When the note is tagged
    Then the note should have the following tags:
      | Task               |

  # Both with keywords for different tags
  Scenario: Both with keywords for different tags
    Given a note with the title "Team Meeting" and the content "Discuss progress and review tasks"
    And the following tags exist in the repository:
      | Name    | Keywords                 |
      | Task    | todo, task, work         |
      | Urgent  | important, asap          |
      | Meeting | meeting, discuss, review |
    When the note is tagged
    Then the note should have the following tags:
      | Task, Meeting |

  # No keywords in title or content
  Scenario: No keywords in title or content
    Given a note with the title "Vacation Photos" and the content "Pictures from my trip to Hawaii"
    And the following tags exist in the repository:
      | Name    | Keywords                 |
      | Task    | todo, task, work         |
      | Urgent  | important, asap          |
      | Meeting | meeting, discuss, review |
    When the note is tagged
    Then the note should have the following tags:
      |                |

  # Case-insensitive keyword matching
  Scenario: Case-insensitive keyword matching
    Given a note with the title "This is a TASK" and the content "Very IMPORTANT indeed"
    And the following tags exist in the repository:
      | Name    | Keywords                 |
      | Task    | todo, task, work         |
      | Urgent  | important, asap          |
      | Meeting | meeting, discuss, review |
    When the note is tagged
    Then the note should have the following tags:
      | Task, Urgent         |

  # Mixed case keywords
  Scenario: Mixed case keywords
    Given a note with the title "urgent TASK" and the content "ASAP work"
    And the following tags exist in the repository:
      | Name    | Keywords                 |
      | Task    | todo, task, work         |
      | Urgent  | important, asap          |
      | Meeting | meeting, discuss, review |
    When the note is tagged
    Then the note should have the following tags:
      | Task, Urgent         |

  # Synonym keywords
  Scenario: Synonym keywords
    Given a note with the title "Review meeting" and the content "Discuss project"
    And the following tags exist in the repository:
      | Name    | Keywords                 |
      | Task    | todo, task, work         |
      | Urgent  | important, asap          |
      | Meeting | meeting, discuss, review |
    When the note is tagged
    Then the note should have the following tags:
      | Meeting              |
