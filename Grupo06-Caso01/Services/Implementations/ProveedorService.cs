using Grupo06_Caso01.Models;
using Microsoft.EntityFrameworkCore;

namespace Grupo06_Caso01.Services.Implementations;

public class ProveedorService : IProveedorService
{
    private readonly ApplicationDbContext _context;

    public ProveedorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Proveedore>> ListarAsync()
    {
        return await _context.Proveedores.AsNoTracking().ToListAsync();
    }

    public async Task<Proveedore?> ObtenerPorIdAsync(int id)
    {
        return await _context.Proveedores.AsNoTracking()
            .FirstOrDefaultAsync(proveedor => proveedor.Proveedorid == id);
    }

    public async Task<Proveedore> CrearAsync(Proveedore proveedor)
    {
        _context.Proveedores.Add(proveedor);
        await _context.SaveChangesAsync();
        return proveedor;
    }

    public async Task<bool> ActualizarAsync(int id, Proveedore proveedor)
    {
        var existente = await _context.Proveedores.FindAsync(id);
        if (existente is null)
        {
            return false;
        }

        existente.Nombre = proveedor.Nombre;
        existente.Contacto = proveedor.Contacto;
        existente.Telefono = proveedor.Telefono;
        existente.Email = proveedor.Email;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var proveedor = await _context.Proveedores.FindAsync(id);
        if (proveedor is null)
        {
            return false;
        }

        _context.Proveedores.Remove(proveedor);
        await _context.SaveChangesAsync();
        return true;
    }
}
