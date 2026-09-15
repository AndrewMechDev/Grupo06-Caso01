using Grupo06_Caso01.Models;
using Microsoft.EntityFrameworkCore;

namespace Grupo06_Caso01.Repositories.Implementations;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext Context;
    protected readonly DbSet<T> Entities;

    public GenericRepository(ApplicationDbContext context)
    {
        Context = context;
        Entities = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await Entities.AsNoTracking().ToListAsync();

    public async Task<T?> GetByIdAsync(int id) => await Entities.FindAsync(id);

    public async Task AddAsync(T entity) => await Entities.AddAsync(entity);

    public void Update(T entity) => Entities.Update(entity);

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await Entities.FindAsync(id);
        if (entity is null) return false;

        Entities.Remove(entity);
        return true;
    }
}
