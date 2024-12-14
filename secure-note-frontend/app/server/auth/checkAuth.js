'use server';

import {cookies} from "next/headers";

export async function checkAuth() {
    const cookieStore = cookies(); // Access cookies using Next.js cookies API
    const authToken = cookieStore.get('auth_token'); // Get the 'auth_token' cookie

    // TODO: Check for expired token
    // Additonally, this can be deemed unsafe, as we are just checking for the presence of the token
    // We should also verify the token's signature and expiry time

    if (!authToken) {
        return {
            authenticated: false,
            message: 'No authentication token found',
        };
    }

    return {
        authenticated: true,
        message: 'User is authenticated',
    };
}