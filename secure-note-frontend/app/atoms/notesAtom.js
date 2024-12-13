import { atom } from 'jotai';

export const notesAtom = atom([
    {
        title: "Meeting Notes - Dec 10, 2024",
        content: "# Project Kickoff\n\nWe discussed the roadmap for the next quarter, including key milestones and deadlines. Action items: **Follow up on research** by end of this week.",
        isNSFW: false,
        tags: ["meeting", "project", "action items"]
    },
    {
        title: "Design Review - UI Update",
        content: "# New UI Design\n\nThe updated design focuses on user accessibility and responsiveness. **Important:** Make sure to incorporate feedback from the usability tests.",
        isNSFW: false,
        tags: ["design", "UI", "accessibility"]
    },
    {
        title: "Confidential: Internal Policies",
        content: "# Company Policies\n\nThis document contains the internal policies related to security and operations. **Confidential**: Do not share outside the company.",
        isNSFW: true,
        tags: ["confidential", "policies", "security"]
    },
    {
        title: "Personal Journal - Dec 2024",
        content: "# Reflections on the Year\n\n2024 has been a year of growth. I faced many challenges, but also experienced personal and professional achievements. Here's to more learning!",
        isNSFW: false,
        tags: ["journal", "personal", "reflections"]
    },
    {
        title: "Project Deadline Update",
        content: "# Project X Update\n\nThe team is a bit behind schedule on some deliverables. Need to revise deadlines. **Action Required:** Team leads to update timelines ASAP.",
        isNSFW: false,
        tags: ["project", "deadlines", "team"]
    },
    {
        title: "Social Media Strategy - Q1",
        content: "# Q1 Strategy\n\nWe need to increase engagement with the audience. Focus on video content and collaborations. Make sure all posts are aligned with the brand tone.",
        isNSFW: false,
        tags: ["social media", "strategy", "engagement"]
    },
    {
        title: "Team Outing - Event Details",
        content: "# Team Event\n\nWe're planning a team outing next month. **Location**: Mountain retreat. Mark your calendars!",
        isNSFW: false,
        tags: ["event", "team", "outing"]
    },
    {
        title: "Confidential: Financial Forecast",
        content: "# Financial Overview\n\nThis document contains sensitive financial projections for the next quarter. **Confidential**: Do not share with anyone outside of the finance team.",
        isNSFW: true,
        tags: ["confidential", "finance", "forecast"]
    },
    {
        title: "Health Tips for December",
        content: "# Staying Healthy During Winter\n\nMake sure to take vitamins, stay active, and get enough rest. **Bonus Tip**: Avoid heavy meals before bedtime.",
        isNSFW: false,
        tags: ["health", "tips", "winter"]
    },
    {
        title: "Workout Plan - Week 2",
        content: "# Fitness Goals\n\nFocus on strength training and building endurance. **Note**: Track your progress and share your achievements with the team!",
        isNSFW: false,
        tags: ["fitness", "workout", "goals"]
    }
]);
