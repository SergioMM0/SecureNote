'use client';

import React, { useEffect, useState } from "react";
import ReactMarkdown from "react-markdown";
import { AiOutlineEyeInvisible, AiOutlineLogout, AiOutlineWarning, AiOutlineFile } from "react-icons/ai";
import { useAtom } from "jotai";
import { notesAtom } from "../atoms/notesAtom";
import { useRouter } from "next/navigation";
import { checkAuth } from "@/app/server/auth/checkAuth";
import { createNote } from "@/app/server/note/createNote";
import { getAllNotes } from "@/app/server/note/getAllNotes";

export default function Notes() {
    const router = useRouter();
    const [notes, setNotes] = useAtom(notesAtom);
    const [selectedNote, setSelectedNote] = useState(null);
    const [isConfirmed, setIsConfirmed] = useState(true);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const checkAuthentication = async () => {
            const response = await checkAuth();
            if (!response.authenticated) {
                router.push("/login");
                return;
            }
        }

        const fetchNotes = async () => {
            const notesResponse = await getAllNotes();
            if (notesResponse.success) {
                setNotes(notesResponse.data);
                console.log(notesResponse.data);
                if (notesResponse.data.length > 0) {
                    setSelectedNote(notesResponse.data[0]);
                }
            } else {
                console.error(notesResponse.message);
            }

            setIsLoading(false);
        };

        checkAuthentication();
        fetchNotes();
    }, [router, setNotes]);

    const handleNSFWToggle = () => {
        const updatedNotes = notes.map(note =>
            note.id === selectedNote.id ? { ...note, nsfw: !selectedNote.nsfw } : note
        );
        setNotes(updatedNotes);
        setSelectedNote(prevNote => ({ ...prevNote, nsfw: !prevNote.nsfw }));
        setIsConfirmed(!selectedNote.nsfw);
    };

    const logout = () => {
        router.push("/login");
    };

    const handleNewNote = async () => {
        const response = await createNote();
        if (!response.success) {
            console.error(response.message);
            return;
        }

        const newNote = response.data;
        setNotes([...notes, newNote]);
        setSelectedNote(newNote);
        setIsConfirmed(true);
    };

    if (isLoading) {
        return (
            <div className="flex items-center justify-center h-screen">
                <div className="loading loading-spinner text-primary"></div>
            </div>
        );
    }

    return (
        <div className="flex min-h-screen p-[1rem] bg-base-100">
            {/* Sidebar */}
            <div className="flex flex-col w-fit min-w-48 max-w-96 bg-base-200 rounded-lg shadow-lg p-4">
                <div className="flex justify-between items-center space-x-2 mb-4">
                    <h2 className="text-lg font-bold">Notes</h2>
                    <button
                        onClick={handleNewNote}
                        className="btn btn-sm btn-primary"
                    >
                        <span>New</span>
                        <AiOutlineFile size={20} />
                    </button>
                </div>
                <ul className="space-y-2 overflow-y-auto">
                    {notes.map((note) => (
                        <li
                            key={note.id}
                            onClick={() => {
                                setSelectedNote(note);
                                setIsConfirmed(!note.nsfw);
                            }}
                            className={`p-2 bg-base-300 rounded-lg cursor-pointer hover:bg-secondary hover:text-secondary-content transition ${
                                selectedNote?.id === note.id ? "bg-secondary text-secondary-content" : ""
                            } flex items-center justify-between`}
                        >
                            {note.title || <span className={"text-gray-500 italic"}>Untitled</span>}
                            {note.nsfw && (
                                <AiOutlineEyeInvisible size={20} className="text-gray-500 ml-2" />
                            )}
                        </li>
                    ))}
                </ul>
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
                {selectedNote?.nsfw && !isConfirmed ? (
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
                                value={selectedNote?.title || ''}
                                onChange={(e) => setSelectedNote({ ...selectedNote, title: e.target.value })}
                                className="input input-bordered mb-4"
                                placeholder="Enter note title"
                            />
                            <textarea
                                value={selectedNote?.content || ''}
                                onChange={(e) => setSelectedNote({ ...selectedNote, content: e.target.value })}
                                className="textarea textarea-bordered h-full"
                                placeholder="Write your markdown here..."
                            />
                            <div className="flex items-center mt-4">
                                <div className="flex space-x-2">
                                    {selectedNote?.tags?.map((tag, index) => (
                                        <span key={index} className="badge badge-accent">
                                            {tag}
                                        </span>
                                    ))}
                                </div>
                                <div className="ml-auto flex items-center">
                                    <label className="text-sm text-base-content mr-2">NSFW</label>
                                    <input
                                        type="checkbox"
                                        checked={selectedNote?.nsfw}
                                        onChange={handleNSFWToggle}
                                        className="toggle toggle-error"
                                    />
                                </div>
                            </div>
                        </div>
                        <div className="w-1/2 flex flex-col">
                            <h2 className="text-lg font-bold mb-4">Rendered Markdown</h2>
                            <div className="flex-1 prose bg-base-200 p-4 rounded-lg shadow overflow-auto">
                                <ReactMarkdown className={"markdown"}>
                                    {`# ${selectedNote?.title || ''}\n\n${selectedNote?.content || ''}`}
                                </ReactMarkdown>
                            </div>
                        </div>
                    </>
                )}
            </div>
        </div>
    );
}
