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
        return await _context.Pedidos.AsNoTracking().ToListAsync();
    }

    public async Task<Pedido?> ObtenerPorIdAsync(int id)
    {
        return await _context.Pedidos.AsNoTracking()
            .FirstOrDefaultAsync(pedido => pedido.Pedidoid == id);
    }

    public async Task<Pedido> CrearAsync(Pedido pedido)
    {
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();
        return pedido;
    }

    public async Task<bool> ActualizarAsync(int id, Pedido pedido)
    {
        var existente = await _context.Pedidos.FindAsync(id);
        if (existente is null)
        {
            return false;
        }

        existente.Proveedorid = pedido.Proveedorid;
        existente.Fechapedido = pedido.Fechapedido;
        existente.Estado = pedido.Estado;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido is null)
        {
            return false;
        }

        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync();
        return true;
    }
}
