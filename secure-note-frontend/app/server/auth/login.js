'use server';

import axios from 'axios';
import { cookies } from 'next/headers'; // For managing cookies in Next.js

export async function login(dto) {
    const API_URL = process.env.API_URL;

    try {
        const response = await axios.post(`${API_URL}/Auth/Login`, dto, {
            headers: {
                "Content-Type": "application/json",
            },
        });

        const token = response.data.token; // Assuming the token is in `response.data.token`

        // Set the token as an HTTP-only cookie
        cookies().set({
            name: 'auth_token',
            value: token,
            httpOnly: true, // Ensure it's only accessible by the server
            secure: process.env.NODE_ENV === 'production', // Use secure flag in production
            sameSite: 'strict', // Restrict cross-site cookie usage
            path: '/', // Make it available for the whole domain
            //maxAge: 60 * 60 * 24 * 7, // Optional, 7 days here
        });

        return {
            success: true,
            data: response.data,
        };
    } catch (err) {
        return {
            success: false,
            message: err.response?.data || "An unknown error occurred",
        };
    }
}
