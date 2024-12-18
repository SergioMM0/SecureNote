'use server';

import axios from 'axios';
import { cookies } from 'next/headers'; // For managing cookies in Next.js

export async function updateNote(dto) {
    const API_URL = process.env.API_URL;

    // Retrieve the JWT token from the cookies
    const token = (await cookies()).get('auth_token'); // This reads the 'auth_token' cookie

    if (!token) {
        return {
            success: false,
            message: "Authentication token is missing or expired.",
        };
    }

    try {
        const response = await axios.put(
            `${API_URL}/Note/${dto.id}`, // Endpoint
            dto, // Updated note data
            {
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token.value}`, // Attach the token to the Authorization header
                },
            }
        );

        return {
            success: true,
            data: response.data,
        };
    } catch (err) {
        return {
            success: false,
            message: err.response?.data || err.message || "An unknown error occurred.",
        };
    }
}