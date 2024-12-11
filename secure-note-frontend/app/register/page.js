'use client';

import React, { useState } from "react";
import Link from "next/link";
import Image from "next/image";
import { useRouter } from "next/navigation"; // Import the useRouter hook

export default function Register() {
    const router = useRouter(); // Initialize useRouter
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");

    // Handle form submission
    const handleSubmit = (e) => {
        e.preventDefault(); // Prevent the default form submission behavior

        // Optional: Add validation here (e.g., check if passwords match)
        if (password !== confirmPassword) {
            alert("Passwords do not match!");
            return;
        }

        // If validation passes, navigate to the /notes page
        router.push("/notes");
    };

    return (
        <div className="min-h-screen bg-base-200 flex flex-col justify-center items-center">
            <Link href="/" className="btn btn-ghost normal-case text-xl mb-[1rem]">
                <Image src="/logo.png" alt="SecureNote" width={32} height={32} />
                <span>SecureNote</span>
            </Link>
            <div className="w-full max-w-md p-8 space-y-6 bg-white rounded-lg shadow-lg">
                <h2 className="text-2xl font-semibold text-center text-primary">Register</h2>

                <form className="space-y-4" onSubmit={handleSubmit}>
                    <div>
                        <label htmlFor="email" className="block text-sm font-medium text-base-content">
                            Email Address
                        </label>
                        <input
                            type="email"
                            id="email"
                            placeholder="Enter your email"
                            className="w-full p-3 mt-1 border rounded-md focus:ring-2 focus:ring-primary"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        />
                    </div>

                    <div>
                        <label htmlFor="username" className="block text-sm font-medium text-base-content">
                            Username
                        </label>
                        <input
                            type="text"
                            id="username"
                            placeholder="Choose a username"
                            className="w-full p-3 mt-1 border rounded-md focus:ring-2 focus:ring-primary"
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                        />
                    </div>

                    <div>
                        <label htmlFor="password" className="block text-sm font-medium text-base-content">
                            Password
                        </label>
                        <input
                            type="password"
                            id="password"
                            placeholder="Enter your password"
                            className="w-full p-3 mt-1 border rounded-md focus:ring-2 focus:ring-primary"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                        />
                    </div>

                    <div>
                        <label htmlFor="confirmPassword" className="block text-sm font-medium text-base-content">
                            Confirm Password
                        </label>
                        <input
                            type="password"
                            id="confirmPassword"
                            placeholder="Confirm your password"
                            className="w-full p-3 mt-1 border rounded-md focus:ring-2 focus:ring-primary"
                            value={confirmPassword}
                            onChange={(e) => setConfirmPassword(e.target.value)}
                        />
                    </div>

                    <div className="flex items-center justify-between">
                        <button
                            type="submit"
                            className="w-full py-3 mt-4 text-white bg-primary rounded-md hover:bg-primary-focus focus:outline-none focus:ring-2 focus:ring-primary"
                        >
                            Sign Up
                        </button>
                    </div>
                </form>

                <div className="text-center mt-4">
                    <p className="text-sm">
                        <span>Already have an account?</span> <a href="/login" className="text-link link-primary hover:text-link-focus">Log in</a>
                    </p>
                </div>
            </div>
        </div>
    );
}
