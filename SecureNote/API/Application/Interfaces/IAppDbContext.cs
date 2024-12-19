namespace API.Application.Interfaces;

public interface IAppDbContext {
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    void Attach<T>(T entity) where T : class;
}