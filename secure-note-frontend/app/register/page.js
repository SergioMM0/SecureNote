// pages/register.js
import React from "react";

export default function Register() {
    return (
        <div className="min-h-screen bg-base-200 flex justify-center items-center">
            <div className="w-full max-w-md p-8 space-y-6 bg-white rounded-lg shadow-lg">
                <h2 className="text-2xl font-semibold text-center text-primary">Sign Up</h2>

                <form className="space-y-4">
                    <div>
                        <label htmlFor="email" className="block text-sm font-medium text-base-content">
                            Email Address
                        </label>
                        <input
                            type="email"
                            id="email"
                            placeholder="Enter your email"
                            className="w-full p-3 mt-1 border rounded-md focus:ring-2 focus:ring-primary"
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
                        <span>Already have an account?</span> <a href="/login" className="text-link hover:text-link-focus">Log in</a>
                    </p>
                </div>
            </div>
        </div>
    );
}
