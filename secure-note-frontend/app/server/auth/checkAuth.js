// Frontend: Making a secure request to the backend
'use server';

import {cookies} from "next/headers";

export async function checkAuth() {
    const cookieStore = await cookies();
    const authToken = cookieStore.get('auth_token').value;

    if (!authToken) {
        return {
            authenticated: false,
            message: 'No authentication token found',
        };
    }

    let response;
    try {
        response = await fetch(`${process.env.API_URL}/Auth/VerifyToken`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${authToken}`,
            },
        });
    } catch (err) {
        return {
            authenticated: false,
            message: 'An error occurred while verifying the authentication token',
        };
    }

    const data = await response.json();
    if (!response.ok) {
        return {
            authenticated: false,
            message: 'User is not authenticated',
        };
    }

    return {
        authenticated: true,
        message: 'User is authenticated',
    };
}
