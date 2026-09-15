using Grupo06_Caso01.Models;

namespace Grupo06_Caso01.Repositories;

public interface IProductoRepository : IGenericRepository<Producto>
{
    Task<IEnumerable<Producto>> GetByCategoriaAsync(int categoriaId);
    Task<IEnumerable<Producto>> GetWithLowStockAsync();
}
