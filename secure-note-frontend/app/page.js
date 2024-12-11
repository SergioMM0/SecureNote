import Link from 'next/link'

export default function LandingPage() {
    return (
        <div className="min-h-screen flex flex-col">
            <nav className="navbar bg-base-100">
                <div className="flex-1">
                    <Link href="/" className="btn btn-ghost normal-case text-xl">SecureNote</Link>
                </div>
                <div className="flex-none">
                    <Link href="/register" className="btn btn-primary mr-2">Register</Link>
                    <Link href="/login" className="btn btn-ghost">Login</Link>
                </div>
            </nav>

            <main className="flex-grow">
                <section className="hero bg-base-200">
                    <div className="hero-content text-center">
                        <div className="max-w-md">
                            <h1 className="text-5xl font-bold">SecureNote</h1>
                            <p className="py-6">Notes, for your eyes only. Secure, private, and feature-rich note-taking application.</p>
                            <Link href="/register" className="btn btn-primary">Get Started</Link>
                        </div>
                    </div>
                </section>

                <section className="py-12">
                    <div className="container mx-auto px-4">
                        <h2 className="text-3xl font-bold text-center mb-8">Key Features</h2>
                        <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
                            <div className="card bg-base-100 shadow-xl">
                                <div className="card-body">
                                    <h3 className="card-title">Secure Storage</h3>
                                    <p>Your notes are securely stored and accessed, ensuring your privacy.</p>
                                </div>
                            </div>
                            <div className="card bg-base-100 shadow-xl">
                                <div className="card-body">
                                    <h3 className="card-title">Markdown Support</h3>
                                    <p>Create and edit notes using Markdown for rich formatting.</p>
                                </div>
                            </div>
                            <div className="card bg-base-100 shadow-xl">
                                <div className="card-body">
                                    <h3 className="card-title">Automatic Tagging</h3>
                                    <p>Notes are automatically tagged upon saving for easy organization.</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>

                <section className="py-12 bg-base-200">
                    <div className="container mx-auto px-4">
                        <h2 className="text-3xl font-bold text-center mb-8">Why Choose SecureNote?</h2>
                        <ul className="list-disc list-inside max-w-2xl mx-auto">
                            <li className="mb-4">Create notes quickly without specifying title and content right away</li>
                            <li className="mb-4">Edit, delete, and read notes with full Markdown rendering</li>
                            <li className="mb-4">Mark notes as NSFW for additional privacy</li>
                            <li className="mb-4">Consent required before viewing NSFW content</li>
                            <li>Automatic tagging system for effortless organization</li>
                        </ul>
                    </div>
                </section>
            </main>

            <footer className="footer footer-center p-4 bg-base-300 text-base-content">
                <div>
                    <p>© 2023 SecureNote - All rights reserved</p>
                </div>
            </footer>
        </div>
    )
}

