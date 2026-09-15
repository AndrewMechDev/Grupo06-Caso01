using Grupo06_Caso01.Models;

namespace Grupo06_Caso01.Services;

public interface IMovimientoInventarioService
{
    Task<IEnumerable<Movimientosinventario>> ListarAsync();
    Task<Movimientosinventario?> ObtenerPorIdAsync(int id);
    Task<Movimientosinventario> CrearAsync(Movimientosinventario movimiento);
    Task<bool> ActualizarAsync(int id, Movimientosinventario movimiento);
    Task<bool> EliminarAsync(int id);
}
