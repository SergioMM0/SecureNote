'use client';

import React, { useState } from "react";
import ReactMarkdown from "react-markdown";
import { AiOutlineEyeInvisible, AiOutlineWarning } from "react-icons/ai"; // React icon for the hidden eye
import Image from "next/image";

export default function Home() {
    // Mock notes data
    const [mockNotes, setMockNotes] = useState([
        {
            title: "Meeting Notes - Dec 10, 2024",
            content: "# Project Kickoff\n\nWe discussed the roadmap for the next quarter, including key milestones and deadlines. Action items: **Follow up on research** by end of this week.",
            isNSFW: false
        },
        {
            title: "Design Review - UI Update",
            content: "# New UI Design\n\nThe updated design focuses on user accessibility and responsiveness. **Important:** Make sure to incorporate feedback from the usability tests.",
            isNSFW: false
        },
        {
            title: "Confidential: Internal Policies",
            content: "# Company Policies\n\nThis document contains the internal policies related to security and operations. **Confidential**: Do not share outside the company.",
            isNSFW: true
        },
        {
            title: "Personal Journal - Dec 2024",
            content: "# Reflections on the Year\n\n2024 has been a year of growth. I faced many challenges, but also experienced personal and professional achievements. Here's to more learning!",
            isNSFW: false
        },
        {
            title: "Project Deadline Update",
            content: "# Project X Update\n\nThe team is a bit behind schedule on some deliverables. Need to revise deadlines. **Action Required:** Team leads to update timelines ASAP.",
            isNSFW: false
        },
        {
            title: "Social Media Strategy - Q1",
            content: "# Q1 Strategy\n\nWe need to increase engagement with the audience. Focus on video content and collaborations. Make sure all posts are aligned with the brand tone.",
            isNSFW: false
        },
        {
            title: "Team Outing - Event Details",
            content: "# Team Event\n\nWe're planning a team outing next month. **Location**: Mountain retreat. Mark your calendars!",
            isNSFW: false
        },
        {
            title: "Confidential: Financial Forecast",
            content: "# Financial Overview\n\nThis document contains sensitive financial projections for the next quarter. **Confidential**: Do not share with anyone outside of the finance team.",
            isNSFW: true
        },
        {
            title: "Health Tips for December",
            content: "# Staying Healthy During Winter\n\nMake sure to take vitamins, stay active, and get enough rest. **Bonus Tip**: Avoid heavy meals before bedtime.",
            isNSFW: false
        },
        {
            title: "Workout Plan - Week 2",
            content: "# Fitness Goals\n\nFocus on strength training and building endurance. **Note**: Track your progress and share your achievements with the team!",
            isNSFW: false
        }
    ]);


    const [selectedNote, setSelectedNote] = useState(mockNotes[0].title);
    const [noteTitle, setNoteTitle] = useState(mockNotes[0].title);
    const [markdownContent, setMarkdownContent] = useState(mockNotes[0].content);
    const [isConfirmed, setIsConfirmed] = useState(!mockNotes[0].isNSFW);

    const selectedNoteDetails = mockNotes.find((note) => note.title === selectedNote);

    const handleNSFWToggle = () => {
        const updatedNotes = mockNotes.map((note) =>
            note.title === selectedNote ? { ...note, isNSFW: !note.isNSFW } : note
        );
        setMockNotes(updatedNotes);
        setIsConfirmed(!selectedNoteDetails.isNSFW);
    };

    return (
        <div className="flex min-h-screen p-[1rem] bg-base-100">
            {/* Sidebar */}
            <div className="w-fit max-w-96 bg-base-200 rounded-lg shadow-lg p-4">
                <h2 className="text-lg font-bold mb-4">Notes</h2>
                <ul className="space-y-2">
                    {mockNotes.map((note) => (
                        <li
                            key={note.title}
                            onClick={() => {
                                setSelectedNote(note.title);
                                setNoteTitle(note.title);
                                setMarkdownContent(note.content);
                                setIsConfirmed(!note.isNSFW);
                            }}
                            className={`p-2 bg-base-300 rounded-lg cursor-pointer hover:bg-primary hover:text-primary-content transition ${
                                selectedNote === note.title ? "bg-primary text-primary-content" : ""
                            } flex items-center justify-between`}
                        >
                            <span>{note.title}</span>
                            {note.isNSFW && (
                                <AiOutlineEyeInvisible size={20} className="text-gray-500 ml-2" />
                            )}
                        </li>
                    ))}
                </ul>
            </div>

            {/* Main Content */}
            <div className="flex w-full min-h-full bg-base-100 p-6 rounded-lg ml-4 shadow-lg space-x-4">
                {/* NSFW Warning */}
                {selectedNoteDetails.isNSFW && !isConfirmed ? (
                    <div className="flex-1 flex flex-col items-center justify-center bg-red-100 text-red-700 rounded-lg p-6 shadow">
                        <AiOutlineWarning size={50} className="mb-4" />
                        <h2 className="text-xl font-bold mb-4">NSFW Content Warning</h2>
                        <p className="mb-6 text-center">
                            The note you're trying to view contains content that may not be suitable for all audiences.
                        </p>
                        <button
                            onClick={() => setIsConfirmed(true)}
                            className="px-4 py-2 bg-red-600 text-white rounded-lg shadow hover:bg-red-700 transition"
                        >
                            View Content
                        </button>
                    </div>
                ) : (
                    <>
                        <div className="w-1/2 flex flex-col">
                            <h2 className="text-lg font-bold mb-4">Markdown Editor</h2>
                            <input
                                type="text"
                                value={noteTitle}
                                onChange={(e) => setNoteTitle(e.target.value)}
                                className="input input-bordered mb-4"
                                placeholder="Enter note title"
                            />
                            <textarea
                                value={markdownContent}
                                onChange={(e) => setMarkdownContent(e.target.value)}
                                className="textarea textarea-bordered h-full"
                                placeholder="Write your markdown here..."
                            />
                            <div className="flex items-center mt-4">
                                <input
                                    type="checkbox"
                                    checked={selectedNoteDetails.isNSFW}
                                    onChange={handleNSFWToggle}
                                    className="mr-2"
                                />
                                <label className="text-sm text-base-content">Mark as NSFW</label>
                            </div>
                        </div>

                        {/* Markdown Renderer */}
                        <div className="w-1/2 flex flex-col">
                            <h2 className="text-lg font-bold mb-4">Rendered Markdown</h2>
                            <div className="flex-1 prose bg-base-200 p-4 rounded-lg shadow overflow-auto">
                                <ReactMarkdown className={"markdown"}>
                                    {/* Title and content */}
                                    {`# ${noteTitle}\n\n${markdownContent}`}
                                </ReactMarkdown>
                            </div>
                        </div>
                    </>
                )}
            </div>
        </div>
    );
}
