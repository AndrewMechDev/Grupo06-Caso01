using Grupo06_Caso01.Models;

namespace Grupo06_Caso01.Repositories;

public interface IPedidoRepository : IGenericRepository<Pedido>
{
    Task<IEnumerable<Pedido>> GetByProveedorAsync(int proveedorId);
}
