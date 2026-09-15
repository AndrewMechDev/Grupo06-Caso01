using Grupo06_Caso01.Models;
using Grupo06_Caso01.Repositories;

namespace Grupo06_Caso01.Services.Implementations;

public class PedidoService : IPedidoService
{
    private readonly IUnitOfWork _uow;

    public PedidoService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<Pedido>> ListarAsync()
    {
        return (await _uow.Pedidos.GetAllAsync()).OrderBy(x => x.Pedidoid);
    }

    public async Task<Pedido?> ObtenerPorIdAsync(int id)
    {
        return await _uow.Pedidos.GetByIdAsync(id);
    }

    public async Task<Pedido> CrearAsync(Pedido valor)
    {
        ArgumentNullException.ThrowIfNull(valor);
        if (string.IsNullOrWhiteSpace(valor.Estado)) valor.Estado = "Pendiente";
        if (valor.Fechapedido == default) valor.Fechapedido = DateTime.Now;
        await ValidarAsync(valor);
        var nuevo = new Pedido
        {
            Proveedorid = valor.Proveedorid,
            Fechapedido = valor.Fechapedido,
            Estado = valor.Estado
        };
        await _uow.Pedidos.AddAsync(nuevo);
        await _uow.SaveChangesAsync();
        return nuevo;
    }

    public async Task<bool> ActualizarAsync(int id, Pedido valor)
    {
        ArgumentNullException.ThrowIfNull(valor);
        var actual = await _uow.Pedidos.GetByIdAsync(id);
        if (actual is null) return false;
        await ValidarAsync(valor);
        actual.Proveedorid = valor.Proveedorid;
        actual.Fechapedido = valor.Fechapedido;
        actual.Estado = valor.Estado;
        await _uow.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var actual = await _uow.Pedidos.GetByIdAsync(id);
        if (actual is null) return false;

        await _uow.Pedidos.DeleteAsync(id);
        await _uow.SaveChangesAsync();
        return true;
    }

    private async Task ValidarAsync(Pedido valor)
    {
        if (string.IsNullOrWhiteSpace(valor.Estado) || valor.Estado.Length > 50)
            throw new ArgumentException("El estado es obligatorio y admite hasta 50 caracteres.");
        if ((await _uow.Proveedores.GetByIdAsync(valor.Proveedorid)) is null)
            throw new ArgumentException("El proveedor indicado no existe.");
        if (valor.Fechapedido.Kind == DateTimeKind.Utc)
            throw new ArgumentException("La fecha del pedido debe ser una fecha local, sin zona horaria UTC.");

    }
}
