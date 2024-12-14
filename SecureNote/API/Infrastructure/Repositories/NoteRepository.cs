using API.Application.Interfaces.Repositories;
using API.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Repositories;

public class NoteRepository : INoteRepository {
    private readonly AppDbContext _db;
    
    public NoteRepository(AppDbContext appDbContext) {
        _db = appDbContext;
    }

    public async Task<IEnumerable<Note>> GetAllByUserId(Guid userId, bool nfsw) {
        return await _db.Notes
            .Where(n => n.UserId == userId && n.Nfsw == nfsw)
            .ToListAsync();
    }
    
    public async Task<Note> Create(Note note) {
        var result = await _db.Notes.AddAsync(note);
        return result.Entity;
    }
    
    public async Task<Note?> Get(Guid id) {
        return await _db.Notes.FindAsync(id);
    }
    
    public void Delete(Note note) {
        _db.Notes.Remove(note);
    }
}