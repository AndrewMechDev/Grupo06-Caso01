using Grupo06_Caso01.Models;
using Microsoft.EntityFrameworkCore;

namespace Grupo06_Caso01.Services.Implementations;

public class MovimientoInventarioService : IMovimientoInventarioService
{
    private readonly ApplicationDbContext _context;

    public MovimientoInventarioService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Movimientosinventario>> ListarAsync()
    {
        return await _context.Movimientosinventarios.AsNoTracking().ToListAsync();
    }

    public async Task<Movimientosinventario?> ObtenerPorIdAsync(int id)
    {
        return await _context.Movimientosinventarios.AsNoTracking()
            .FirstOrDefaultAsync(movimiento => movimiento.Movimientoid == id);
    }

    public async Task<Movimientosinventario> CrearAsync(Movimientosinventario movimiento)
    {
        _context.Movimientosinventarios.Add(movimiento);
        await _context.SaveChangesAsync();
        return movimiento;
    }

    public async Task<bool> ActualizarAsync(int id, Movimientosinventario movimiento)
    {
        var existente = await _context.Movimientosinventarios.FindAsync(id);
        if (existente is null)
        {
            return false;
        }

        existente.Productoid = movimiento.Productoid;
        existente.Tipomovimiento = movimiento.Tipomovimiento;
        existente.Cantidad = movimiento.Cantidad;
        existente.Fecha = movimiento.Fecha;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var movimiento = await _context.Movimientosinventarios.FindAsync(id);
        if (movimiento is null)
        {
            return false;
        }

        _context.Movimientosinventarios.Remove(movimiento);
        await _context.SaveChangesAsync();
        return true;
    }
}
