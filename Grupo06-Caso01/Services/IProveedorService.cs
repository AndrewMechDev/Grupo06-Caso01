using Grupo06_Caso01.Models;

namespace Grupo06_Caso01.Services;

public interface IProveedorService
{
    Task<IEnumerable<Proveedore>> ListarAsync();
    Task<Proveedore?> ObtenerPorIdAsync(int id);
    Task<Proveedore> CrearAsync(Proveedore proveedor);
    Task<bool> ActualizarAsync(int id, Proveedore proveedor);
    Task<bool> EliminarAsync(int id);
}
