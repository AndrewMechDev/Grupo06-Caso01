using Grupo06_Caso01.Models;

namespace Grupo06_Caso01.Services;

public interface IPedidoService
{
    Task<IEnumerable<Pedido>> ListarAsync();
    Task<Pedido?> ObtenerPorIdAsync(int id);
    Task<Pedido> CrearAsync(Pedido pedido);
    Task<bool> ActualizarAsync(int id, Pedido pedido);
    Task<bool> EliminarAsync(int id);
}
