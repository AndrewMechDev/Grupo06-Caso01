using Grupo06_Caso01.Models;

namespace Grupo06_Caso01.Services;

public interface IProductoService
{
    Task<IEnumerable<Producto>> ListarAsync();
    Task<Producto?> ObtenerPorIdAsync(int id);
    Task<Producto> CrearAsync(Producto producto);
    Task<bool> ActualizarAsync(int id, Producto producto);
    Task<bool> EliminarAsync(int id);
}
