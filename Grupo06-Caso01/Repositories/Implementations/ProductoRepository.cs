using Grupo06_Caso01.Models;
using Microsoft.EntityFrameworkCore;

namespace Grupo06_Caso01.Repositories.Implementations;

public class ProductoRepository(ApplicationDbContext context)
    : GenericRepository<Producto>(context), IProductoRepository
{
    public async Task<IEnumerable<Producto>> GetByCategoriaAsync(int categoriaId) =>
        await Entities.AsNoTracking().Where(p => p.Categoriaid == categoriaId).ToListAsync();

    public async Task<IEnumerable<Producto>> GetWithLowStockAsync() =>
        await Entities.AsNoTracking().Where(p => p.Stockactual <= p.Stockminimo).ToListAsync();

    public async Task<bool> ExisteParaCategoriaAsync(int categoriaId) =>
        await Entities.AnyAsync(p => p.Categoriaid == categoriaId);
}
