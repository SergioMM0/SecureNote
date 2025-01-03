'use client';

import React, { useState } from "react";
import Link from "next/link";
import Image from "next/image";
import { useRouter } from "next/navigation";
import { useSetAtom } from "jotai";
import { authAtom } from "@/app/atoms/authAtom";
import { login } from "@/app/server/auth/login";

export default function Login() {
    const router = useRouter();
    const setAuthState = useSetAtom(authAtom);
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [emailError, setEmailError] = useState("");
    const [passwordError, setPasswordError] = useState("");

    // Handle form submission
    const handleSubmit = async (e) => {
        e.preventDefault();

        // Reset errors
        setEmailError("");
        setPasswordError("");

        // Validate fields
        let isValid = true;

        if (!email) {
            setEmailError("Email is required.");
            isValid = false;
        } else if (!/\S+@\S+\.\S+/.test(email)) {
            setEmailError("Please enter a valid email address.");
            isValid = false;
        }

        if (!password) {
            setPasswordError("Password is required.");
            isValid = false;
        }

        if (!isValid) return;

        const dto = { email, password };

        const result = await login(dto);

        if (!result.success) {
            setPasswordError(result.message || "Login failed. Please check your credentials.");
            return;
        }

        const data = result.data;

        // Update global auth state with user and token
        setAuthState({
            isAuthenticated: true,
            user: {
                id: data.id,
                email: data.email,
                username: data.username,
                roles: data.roles,
            },
            token: data.token,
        });

        // Redirect to the protected notes page
        router.push("/notes");
    };

    return (
        <div className="min-h-screen bg-base-200 flex flex-col justify-center items-center">
            <Link href="/" className="btn btn-ghost normal-case text-xl mb-[1rem]">
                <Image src="/logo.png" alt="SecureNote" width={32} height={32} />
                <span>SecureNote</span>
            </Link>

            <div className="w-full max-w-md p-8 space-y-6 bg-white rounded-lg shadow-lg">
                <h2 className="text-2xl font-semibold text-center text-primary">Login</h2>

                <form className="space-y-4" onSubmit={handleSubmit}>
                    <div>
                        <label htmlFor="email" className="block text-sm font-medium text-base-content">
                            Email Address
                        </label>
                        <input
                            type="email"
                            id="email"
                            placeholder="Enter your email"
                            className={`w-full p-3 mt-1 border rounded-md focus:ring-2 ${emailError ? 'border-red-600' : 'focus:ring-primary'}`}
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        />
                        {emailError && <div className="text-red-600 text-sm">{emailError}</div>}
                    </div>

                    <div>
                        <label htmlFor="password" className="block text-sm font-medium text-base-content">
                            Password
                        </label>
                        <input
                            type="password"
                            id="password"
                            placeholder="Enter your password"
                            className={`w-full p-3 mt-1 border rounded-md focus:ring-2 ${passwordError ? 'border-red-600' : 'focus:ring-primary'}`}
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                        />
                        {passwordError && <div className="text-red-600 text-sm">{passwordError}</div>}
                    </div>

                    <div className="flex items-center justify-between">
                        <button
                            type="submit"
                            className="w-full py-3 mt-4 text-white bg-primary rounded-md hover:bg-primary-focus focus:outline-none focus:ring-2 focus:ring-primary"
                        >
                            Log In
                        </button>
                    </div>
                </form>

                <div className="text-center mt-4">
                    <p className="text-sm">
                        <span>Don't have an account?</span> <a href="/register" className="text-link link-primary hover:text-link-focus">Register</a>
                    </p>
                </div>
            </div>
        </div>
    );
}
