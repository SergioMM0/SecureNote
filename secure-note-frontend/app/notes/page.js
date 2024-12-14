'use client';

import React, {useEffect, useState} from "react";
import ReactMarkdown from "react-markdown";
import {AiOutlineEyeInvisible, AiOutlineLogout, AiOutlineWarning, AiOutlineFile } from "react-icons/ai"; // React icon for the hidden eye
import { useAtom } from "jotai"; // Jotai hook to access store
import { notesAtom } from "../atoms/notesAtom"; // Import the atom
import { useRouter } from "next/navigation";
import {checkAuth} from "@/app/server/auth/checkAuth"; // Import the useRouter hook

export default function Notes() {
    const router = useRouter(); // Initialize useRouter
    // Fetch notes from the Jotai store
    const [mockNotes, setMockNotes] = useAtom(notesAtom);
    const [selectedNote, setSelectedNote] = useState(mockNotes[0].title);
    const [noteTitle, setNoteTitle] = useState(mockNotes[0].title);
    const [markdownContent, setMarkdownContent] = useState(mockNotes[0].content);
    const [isConfirmed, setIsConfirmed] = useState(!mockNotes[0].isNSFW);
    const [isAuthLoading, setIsAuthLoading] = useState(true);

    useEffect(() => {
        checkAuth().then((response) => {
            if (!response.authenticated) {
                router.push("/login");
            }
            setIsAuthLoading(false);
        });
    }, [router]);

    const selectedNoteDetails = mockNotes.find((note) => note.title === selectedNote);

    const handleNSFWToggle = () => {
        const updatedNotes = mockNotes.map((note) =>
            note.title === selectedNote ? { ...note, isNSFW: !note.isNSFW } : note
        );
        setMockNotes(updatedNotes);
        setIsConfirmed(!selectedNoteDetails.isNSFW);
    };

    const logout = () => {
        // Redirect to the login page
        router.push("/login");
    }

    if (isAuthLoading) {
        return <div className="flex items-center justify-center h-screen">
            <div className="loading loading-spinner text-primary"></div>
        </div>
    }

    return (
        <div className="flex min-h-screen p-[1rem] bg-base-100">
            {/* Sidebar */}
            <div className="flex flex-col w-fit max-w-96 bg-base-200 rounded-lg shadow-lg p-4">
                <div className="flex justify-between items-center space-x-2 mb-4">
                    <h2 className="text-lg font-bold">Notes</h2>
                    <button
                        onClick={() => {
                            setNoteTitle("");
                            setMarkdownContent("");
                            setIsConfirmed(true);
                        }}
                        className="btn btn-sm btn-primary"
                    >
                        <span>New</span>
                        <AiOutlineFile size={20} />
                    </button>
                </div>
                <ul className="space-y-2 overflow-y-auto">
                    {mockNotes.map((note) => (
                        <li
                            key={note.title}
                            onClick={() => {
                                setSelectedNote(note.title);
                                setNoteTitle(note.title);
                                setMarkdownContent(note.content);
                                setIsConfirmed(!note.isNSFW);
                            }}
                            className={`p-2 bg-base-300 rounded-lg cursor-pointer hover:bg-secondary hover:text-secondary-content transition ${
                                selectedNote === note.title ? "bg-secondary text-secondary-content" : ""
                            } flex items-center justify-between`}
                        >
                            <span>{note.title}</span>
                            {note.isNSFW && (
                                <AiOutlineEyeInvisible size={20} className="text-gray-500 ml-2" />
                            )}
                        </li>
                    ))}
                </ul>
                {/* Logout Button */}
                <button
                    onClick={logout}
                    className="btn btn-ghost btn-sm mt-auto"
                >
                    <span>Logout</span>
                    <AiOutlineLogout size={20} />
                </button>
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
                                <div className="flex space-x-2">
                                    {selectedNoteDetails.tags.map((tag, index) => (
                                        <span key={index} className="badge badge-accent">
                                            {tag}
                                        </span>
                                    ))}
                                </div>
                                <div className="ml-auto flex items-center">
                                    <label className="text-sm text-base-content mr-2">NSFW</label>
                                    <input
                                        type="checkbox"
                                        checked={selectedNoteDetails.isNSFW}
                                        onChange={handleNSFWToggle}
                                        className="toggle toggle-error"
                                    />
                                </div>
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
