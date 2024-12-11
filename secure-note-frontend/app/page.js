'use client'

import Link from 'next/link'
import Image from 'next/image'
import { AiOutlineLock, AiOutlineEdit, AiOutlineTag, AiOutlineEye, AiFillStar, AiOutlineThunderbolt } from 'react-icons/ai'

export default function LandingPage() {
    return (
        <div className="min-h-screen flex flex-col">
            <nav className="navbar bg-base-100">
                <div className="flex-1">
                    <Link href="/" className="btn btn-ghost normal-case text-xl">
                        <Image src="/logo.png" alt="SecureNote" width={32} height={32}/>
                        <span>SecureNote</span>
                    </Link>
                </div>
                <div className="flex-none">
                    <Link href="/register" className="btn btn-primary mr-2">Register</Link>
                    <Link href="/login" className="btn btn-ghost">Login</Link>
                </div>
            </nav>

            <main className="flex-grow">
                <section className="hero min-h-[90vh] bg-base-200">
                    <div className="hero-content flex flex-col md:flex-row-reverse justify-between items-center">
                        <div className="mt-6 md:mt-0 md:ml-6">
                            <Image src="/logo.png" alt="SecureNote" width={256} height={256}/>
                        </div>
                        <div className="max-w-md text-left text-center md:text-left">
                            <h1 className="text-5xl font-bold">SecureNote</h1>
                            <h2 className="py-6">Notes, for your eyes only. Secure, private and confidential note-taking
                                application.</h2>
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
                                    <h3 className="card-title"><AiOutlineLock className="text-2xl mr-2"/> Secure Storage
                                    </h3>
                                    <p>Your notes are securely stored and accessed, ensuring your privacy.</p>
                                </div>
                            </div>
                            <div className="card bg-base-100 shadow-xl">
                                <div className="card-body">
                                    <h3 className="card-title"><AiOutlineEdit className="text-2xl mr-2"/> Markdown
                                        Support</h3>
                                    <p>Create and edit notes using Markdown for rich formatting.</p>
                                </div>
                            </div>
                            <div className="card bg-base-100 shadow-xl">
                                <div className="card-body">
                                    <h3 className="card-title"><AiOutlineTag className="text-2xl mr-2"/> Automatic
                                        Tagging</h3>
                                    <p>Notes are automatically tagged upon saving for easy organization.</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>

                <section className="py-12 bg-base-200">
                    <div className="container mx-auto px-4">
                        <h2 className="text-3xl font-bold text-center mb-8">Why Choose SecureNote?</h2>
                        <ul className="list-none max-w-2xl mx-auto">
                            <li className="mb-4 flex items-center"><AiOutlineThunderbolt
                                className="text-2xl mr-2"/> Create notes quickly without specifying title and content
                                right away
                            </li>
                            <li className="mb-4 flex items-center"><AiOutlineEdit className="text-2xl mr-2"/> Edit,
                                delete, and read notes with full Markdown rendering
                            </li>
                            <li className="mb-4 flex items-center"><AiOutlineEye className="text-2xl mr-2"/> Mark notes
                                as NSFW for additional privacy
                            </li>
                            <li className="mb-4 flex items-center"><AiOutlineLock className="text-2xl mr-2"/> Consent
                                required before viewing NSFW content
                            </li>
                            <li className="flex items-center"><AiOutlineTag className="text-2xl mr-2"/> Automatic
                                tagging system for effortless organization
                            </li>
                        </ul>
                    </div>
                </section>

                <section className="py-12">
                    <div className="container mx-auto px-4">
                        <h2 className="text-3xl font-bold text-center mb-8">What Our Users Say</h2>
                        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
                            {[
                                {
                                    name: "Sergio Moreno Martinez",
                                    role: "Project Manager",
                                    quote: "I love the simplicity of SecureNote. It's easy to use and secure."
                                },
                                {
                                    name: "Klaus Hviid",
                                    role: "Consultant",
                                    quote: "The NSFW feature is a game-changer for me. I can keep my notes private."
                                },
                                {
                                    name: "Tawfik Azza",
                                    role: "Software Engineer",
                                    quote: "I can't believe SecureNote is free. It's a must-have for anyone."
                                },
                                {
                                    name: "Adam Lorincz",
                                    role: "Business Owner",
                                    quote: "SecureNote's privacy features give me peace of mind when jotting down business ideas."
                                }
                            ].map((testimonial, index) => (
                                <div key={index} className="card bg-base-100 shadow-xl">
                                    <div className="card-body">
                                        <p className="mb-4">"{testimonial.quote}"</p>
                                        <h3 className="font-bold">{testimonial.name}</h3>
                                        <p className="text-sm">{testimonial.role}</p>
                                        <div className="mt-2 flex">
                                            {[...Array(5)].map((_, i) => (
                                                <AiFillStar key={i} className="text-yellow-400"/>
                                            ))}
                                        </div>
                                    </div>
                                </div>
                            ))}
                        </div>
                    </div>
                </section>

                <section className="py-12 bg-base-200">
                    <div className="container mx-auto px-4">
                        <h2 className="text-3xl font-bold text-center mb-8">Pricing Plan</h2>
                        <div className="flex justify-center items-center">
                            <div className="card bg-base-100 shadow-xl">
                                <div className="card-body">
                                    <h3 className="card-title text-2xl font-bold mb-4">Free Forever</h3>
                                    <ul className="list-disc list-inside mb-4">
                                        <li>Unlimited notes</li>
                                        <li>Unlimited storage</li>
                                        <li>EASV Grade Encryption</li>
                                    </ul>
                                    <p className="text-sm text-gray-500">*We may or may not be stealing your data. But
                                        hey, it's free!</p>
                                    <div className="card-actions justify-end">
                                        <Link href="/register" className="btn btn-primary">Choose Plan</Link>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </main>

            <footer className="footer footer-center p-4 bg-base-300 text-base-content">
                <div>
                    <p>© 2024 First Row Knights - All rights reserved</p>
                </div>
            </footer>
        </div>
    )
}

