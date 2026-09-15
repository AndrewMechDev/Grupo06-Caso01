using Grupo06_Caso01.Models;
using Microsoft.EntityFrameworkCore;

namespace Grupo06_Caso01.Repositories.Implementations;

public class MovimientoInventarioRepository(ApplicationDbContext context)
    : GenericRepository<Movimientosinventario>(context), IMovimientoInventarioRepository
{
    public async Task<IEnumerable<Movimientosinventario>> GetByProductoAsync(int productoId) =>
        await Entities.AsNoTracking().Where(m => m.Productoid == productoId).ToListAsync();
}
