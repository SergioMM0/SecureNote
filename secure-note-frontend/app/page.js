'use client';

import React, { useState } from "react";
import ReactMarkdown from "react-markdown";
import Image from "next/image";

export default function Home() {
    // Mock notes data
    const mockNotes = [
        { title: "Note 1", content: "# Welcome to Note 1\n\nThis is **Note 1**." },
        { title: "Note 2", content: "# Welcome to Note 2\n\nThis is **Note 2**." },
        { title: "Note 3", content: "# Welcome to Note 3\n\nThis is **Note 3**." },
        { title: "Note 4", content: "# Welcome to Note 4\n\nThis is **Note 4**." },
    ];

    const [selectedNote, setSelectedNote] = useState(mockNotes[0].title);
    const [markdownContent, setMarkdownContent] = useState(mockNotes[0].content);

    return (
        <div className="flex min-h-screen p-[1rem] bg-base-100">
            {/* Sidebar */}
            <div className="w-64 bg-base-200 rounded-lg shadow-lg p-4">
                <h2 className="text-lg font-bold mb-4">Notes</h2>
                <ul className="space-y-2">
                    {mockNotes.map((note) => (
                        <li
                            key={note.title}
                            onClick={() => {
                                setSelectedNote(note.title);
                                setMarkdownContent(note.content);
                            }}
                            className={`p-2 bg-base-300 rounded-lg cursor-pointer hover:bg-primary hover:text-primary-content transition ${
                                selectedNote === note.title ? "bg-primary text-primary-content" : ""
                            }`}
                        >
                            {note.title}
                        </li>
                    ))}
                </ul>
            </div>

            {/* Main Content */}
            <div className="flex w-full min-h-full bg-base-100 p-6 rounded-lg ml-4 shadow-lg flex space-x-4">
                {/* Markdown Editor */}
                <div className="w-1/2 flex flex-col">
                    <h2 className="text-lg font-bold mb-4">Markdown Editor</h2>
                    <textarea
                        value={markdownContent}
                        onChange={(e) => setMarkdownContent(e.target.value)}
                        className="flex-1 border rounded-lg p-2 bg-base-200 text-base-content resize-none overflow-auto"
                        placeholder="Write your markdown here..."
                    />
                </div>

                {/* Markdown Renderer */}
                <div className="w-1/2 flex flex-col">
                    <h2 className="text-lg font-bold mb-4">Rendered Markdown</h2>
                    <div className="flex-1 prose bg-base-200 p-4 rounded-lg shadow overflow-auto">
                        <ReactMarkdown>{markdownContent}</ReactMarkdown>
                    </div>
                </div>
            </div>
        </div>
    );
}
