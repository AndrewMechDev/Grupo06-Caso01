using Grupo06_Caso01.Models;

namespace Grupo06_Caso01.Repositories;

public interface IMovimientoInventarioRepository : IGenericRepository<Movimientosinventario>
{
    Task<IEnumerable<Movimientosinventario>> GetByProductoAsync(int productoId);
    Task<bool> ExisteParaProductoAsync(int productoId);
}
