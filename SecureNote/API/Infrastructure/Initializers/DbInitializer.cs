using API.Core.Identity.Entities;
using API.Core.Identity.Managers;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace API.Infrastructure.Initializers;

public class DbInitializer {
    private readonly AppDbContext _db;
    private readonly CustomUserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public DbInitializer(AppDbContext db, CustomUserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager) {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // seeds the database with initial data
    public async Task Init() {
        await _db.Database.EnsureDeletedAsync();
        await _db.Database.EnsureCreatedAsync();

        if (!_db.Roles.Any()) {
            await _roleManager.CreateAsync(new ApplicationRole() {
                Name = "Admin"
            });
            await _roleManager.CreateAsync(new ApplicationRole() {
                Name = "User"
            });
        }
        await _db.SaveChangesAsync();

        _db.Tags.Add(
            new Tag {
                Name = "Work",
                Keywords = [
                    "job", "office", "project", "career", "tasks", "meetings", "deadlines",
                    "clients", "reports", "documents", "emails", "assignments", "collaboration",
                    "productivity", "workplace", "agenda", "schedules", "goals", "progress",
                    "performance", "promotion", "projects", "responsibilities", "teamwork",
                    "leadership", "workflow", "networking", "strategy", "efficiency", "planning",
                    "hours", "management", "supervisor", "tasks list", "analysis", "objectives",
                    "training", "development", "mentorship", "workspace", "priorities", "reviews",
                    "efforts", "presentation", "statistics", "KPIs", "achievements", "resources",
                    "logistics", "engagement", "deliverables"
                ]
            });

        _db.Tags.Add(
            new Tag() {
                Name = "Personal",
                Keywords = [
                    "private", "self", "diary", "me", "reflection", "thoughts", "feelings",
                    "emotions", "memories", "family", "friends", "relationships", "well-being",
                    "hobbies", "dreams", "self-care", "goals", "lifestyle", "routine", "journal",
                    "experiences", "preferences", "desires", "aspirations", "identity", "growth",
                    "mindfulness", "meditation", "personal notes", "wellness", "habits", "life",
                    "contentment", "mental health", "self-improvement", "learning", "mindset",
                    "balance", "work-life", "motivation", "resilience", "empathy", "values",
                    "self-expression", "authenticity", "reflection journal", "peace", "solitude",
                    "adventures", "relaxation", "interests"
                ]
            });

        _db.Tags.Add(
            new Tag() {
                Name = "Shopping",
                Keywords = [
                    "groceries", "store", "buy", "list", "market", "mall", "retail",
                    "cart", "checkout", "items", "purchases", "sale", "discount",
                    "online shopping", "delivery", "food", "clothing", "essentials",
                    "budget", "expenses", "offers", "brands", "grocery list",
                    "supermarket", "needs", "wants", "electronics", "appliances",
                    "bills", "receipt", "wishlist", "deals", "coupons", "weekly needs",
                    "categories", "home essentials", "household items", "supplies",
                    "beauty products", "cosmetics", "clothes", "footwear", "seasonal items",
                    "occasions", "festival shopping", "luxuries", "necessities", "trends",
                    "shopping spree"
                ]
            });

        _db.Tags.Add(
            new Tag() {
                Name = "Ideas",
                Keywords = [
                    "thoughts", "brainstorm", "concepts", "notes", "inspiration", "creativity",
                    "imagination", "plans", "strategy", "designs", "sketches", "drafts",
                    "visions", "dreams", "mind map", "problem solving", "solutions",
                    "inventions", "innovation", "creation", "projects", "conceptualization",
                    "blueprints", "notions", "proposals", "schemes", "insights", "visions",
                    "strategy plans", "abstracts", "reflections", "improvements", "adjustments",
                    "ideas log", "potential", "brainwork", "development", "inspirations",
                    "breakthroughs", "eureka moments", "concept board", "creative notes",
                    "intuition", "insights", "perspectives", "visions", "experiments", "draft ideas"
                ]
            });

        _db.Tags.Add(
            new Tag() {
                Name = "Fitness",
                Keywords = [
                    "exercise", "gym", "health", "workout", "training", "cardio", "strength",
                    "running", "yoga", "wellness", "nutrition", "diet", "muscles", "stamina",
                    "weights", "HIIT", "flexibility", "mobility", "endurance", "calories",
                    "fitness goals", "personal trainer", "routine", "sports", "swimming",
                    "cycling", "fitness equipment", "outdoor fitness", "stretching", "self-care",
                    "physical activity", "fitness apps", "hydration", "sleep", "bodybuilding",
                    "tracking progress", "balance", "recovery", "mind-body connection",
                    "fitness plans", "marathon training", "steps tracking", "aerobics", "pilates",
                    "resistance training", "meal plans", "mental health", "posture", "core strength"
                ]
            });

        _db.Tags.Add(
            new Tag() {
                Name = "Travel",
                Keywords = [
                    "vacation", "trip", "journey", "destinations", "exploration", "adventure",
                    "road trip", "flight", "itinerary", "maps", "travel plans", "luggage",
                    "packing", "hotels", "restaurants", "sightseeing", "photography",
                    "camping", "trekking", "beaches", "mountains", "travel guide", "tickets",
                    "transportation", "culture", "local food", "tourism", "landmarks",
                    "travel blog", "memories", "passport", "visas", "accommodation",
                    "vacation spots", "eco-tourism", "wildlife", "airports", "adventure sports",
                    "backpacking", "cruise", "safari", "exploring", "city tour", "festivals",
                    "travel hacks", "travel tips", "holiday planning"
                ]
            });

        _db.Tags.Add(
            new Tag() {
                Name = "Finance",
                Keywords = [
                    "money", "budget", "expenses", "savings", "investment", "income",
                    "debt", "taxes", "loans", "banking", "accounts", "stocks", "portfolio",
                    "financial goals", "credit card", "mortgage", "interest", "retirement",
                    "wealth", "spending", "budget planner", "expenses tracker", "assets",
                    "liabilities", "insurance", "returns", "capital", "dividends",
                    "financial health", "wealth management", "economic planning", "audit",
                    "cash flow", "funds", "emergency fund", "crypto", "investments",
                    "real estate", "property", "gold", "finance tools", "tracking finance",
                    "tax deductions", "financial advice", "financial planning", "market trends"
                ]
            });

        _db.Tags.Add(
            new Tag() {
                Name = "Learning",
                Keywords = [
                    "study", "education", "school", "knowledge", "courses", "classes",
                    "tutorials", "self-learning", "skills", "growth", "development",
                    "practice", "studying", "lessons", "reading", "books", "notes",
                    "homework", "assignments", "research", "projects", "progress",
                    "tests", "exams", "revision", "online learning", "learning tools",
                    "new skills", "hobbies", "mentorship", "internship", "training",
                    "webinars", "podcasts", "learning curve", "education plans",
                    "schoolwork", "student life", "academic goals", "teaching", "subject mastery",
                    "knowledge base", "open learning", "continuous education",
                    "education apps", "problem-solving", "knowledge sharing", "curiosity"
                ]
            });
    }
}
