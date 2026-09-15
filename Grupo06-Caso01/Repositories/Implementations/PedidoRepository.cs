using Grupo06_Caso01.Models;
using Microsoft.EntityFrameworkCore;

namespace Grupo06_Caso01.Repositories.Implementations;

public class PedidoRepository(ApplicationDbContext context)
    : GenericRepository<Pedido>(context), IPedidoRepository
{
    public async Task<IEnumerable<Pedido>> GetByProveedorAsync(int proveedorId) =>
        await Entities.AsNoTracking().Where(p => p.Proveedorid == proveedorId).ToListAsync();
}
