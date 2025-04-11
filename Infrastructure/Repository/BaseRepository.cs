using Domain.Interfaces;

using Infrastructure.DbContexts;

namespace Infrastructure.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly CommandDbContext commandDbContext;
    protected readonly QueryDbContext queryDbContext;

    public BaseRepository(CommandDbContext commandDbContext, QueryDbContext queryDbContext)
    {
        this.commandDbContext = commandDbContext;
        this.queryDbContext = queryDbContext;
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
    {
        await commandDbContext.Set<T>().AddAsync(entity, cancellationToken);

        return entity;
    }

    public async Task<T> UpdateAsync(T modifierEntity, CancellationToken cancellationToken)
    {
        commandDbContext.Set<T>().Update(modifierEntity);

        await commandDbContext.SaveChangesAsync(cancellationToken);

        return modifierEntity;
    }

    public async Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken) =>
        await queryDbContext.Set<T>().FindAsync([id, cancellationToken], cancellationToken: cancellationToken);

    public IQueryable<T> GetAll() => queryDbContext.Set<T>().AsQueryable();
}
