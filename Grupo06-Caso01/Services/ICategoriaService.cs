using Grupo06_Caso01.Models;

namespace Grupo06_Caso01.Services;

public interface ICategoriaService
{
    Task<IEnumerable<Categoria>> ListarAsync();
    Task<Categoria?> ObtenerPorIdAsync(int id);
    Task<Categoria> CrearAsync(Categoria categoria);
    Task<bool> ActualizarAsync(int id, Categoria categoria);
    Task<bool> EliminarAsync(int id);
}
