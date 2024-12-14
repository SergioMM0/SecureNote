'use server';

import axios from 'axios';
import { cookies } from 'next/headers'; // For managing cookies in Next.js

export async function createNote() {
    const API_URL = process.env.API_URL;

    // Retrieve the JWT token from the cookies
    const cookieStore = await cookies();
    const token = await cookieStore.get('auth_token'); // This reads the 'auth_token' cookie

    if (!token) {
        return {
            success: false,
            message: "Authentication token is missing or expired.",
        };
    }

    try {
        const response = await axios.post(`${API_URL}/Note`, {
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`, // Attach the token to the Authorization header
            },
        });

        return {
            success: true,
            data: response.data,
        };
    } catch (err) {
        return {
            success: false,
            message: err.response?.data || err.data || "An unknown error occurred",
        };
    }
}
