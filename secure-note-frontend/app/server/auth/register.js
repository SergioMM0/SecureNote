'use server';

import axios from 'axios';

export async function register(dto) {
    const API_URL = process.env.API_URL;

    try {
        const response = await axios.post(`${API_URL}/Auth/Register`, dto, {
            headers: {
                "Content-Type": "application/json",
            },
        });

        return {
            success: true,
            data: response.data, // Includes user and token information
        };
    } catch (err) {
        return {
            success: false,
            message: err.message,
        };
    }
}
