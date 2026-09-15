using Grupo06_Caso01.Models;
using Microsoft.EntityFrameworkCore;

namespace Grupo06_Caso01.Services.Implementations;

public class PedidoService : IPedidoService
{
    private readonly ApplicationDbContext _context;

    public PedidoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pedido>> ListarAsync()
    {
        return await _context.Pedidos.AsNoTracking().OrderBy(x => x.Pedidoid).ToListAsync();
    }

    public async Task<Pedido?> ObtenerPorIdAsync(int id)
    {
        return await _context.Pedidos.AsNoTracking().FirstOrDefaultAsync(x => x.Pedidoid == id);
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
        _context.Pedidos.Add(nuevo);
        await _context.SaveChangesAsync();
        return nuevo;
    }

    public async Task<bool> ActualizarAsync(int id, Pedido valor)
    {
        ArgumentNullException.ThrowIfNull(valor);
        var actual = await _context.Pedidos.FindAsync(id);
        if (actual is null) return false;
        await ValidarAsync(valor);
        actual.Proveedorid = valor.Proveedorid;
        actual.Fechapedido = valor.Fechapedido;
        actual.Estado = valor.Estado;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var actual = await _context.Pedidos.FindAsync(id);
        if (actual is null) return false;

        _context.Pedidos.Remove(actual);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task ValidarAsync(Pedido valor)
    {
        if (string.IsNullOrWhiteSpace(valor.Estado) || valor.Estado.Length > 50)
            throw new ArgumentException("El estado es obligatorio y admite hasta 50 caracteres.");
        if (!await _context.Proveedores.AnyAsync(p => p.Proveedorid == valor.Proveedorid))
            throw new ArgumentException("El proveedor indicado no existe.");
        if (valor.Fechapedido.Kind == DateTimeKind.Utc)
            throw new ArgumentException("La fecha del pedido debe ser una fecha local, sin zona horaria UTC.");

    }
}
