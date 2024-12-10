'use client';

import React, { useState } from "react";
import ReactMarkdown from "react-markdown";
import { AiOutlineEyeInvisible, AiOutlineWarning } from "react-icons/ai"; // React icon for the hidden eye
import { useAtom } from "jotai"; // Jotai hook to access store
import { mockNotesAtom } from "./atoms"; // Import the atom

export default function Home() {
    // Fetch notes from the Jotai store
    const [mockNotes, setMockNotes] = useAtom(mockNotesAtom);
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
