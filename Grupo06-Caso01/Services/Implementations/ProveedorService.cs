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
        return await _context.Proveedores.AsNoTracking().OrderBy(x => x.Proveedorid).ToListAsync();
    }

    public async Task<Proveedore?> ObtenerPorIdAsync(int id)
    {
        return await _context.Proveedores.AsNoTracking().FirstOrDefaultAsync(x => x.Proveedorid == id);
    }

    public async Task<Proveedore> CrearAsync(Proveedore valor)
    {
        ArgumentNullException.ThrowIfNull(valor);
        Validar(valor);
        var nuevo = new Proveedore
        {
            Nombre = valor.Nombre,
            Contacto = valor.Contacto,
            Telefono = valor.Telefono,
            Email = valor.Email
        };
        _context.Proveedores.Add(nuevo);
        await _context.SaveChangesAsync();
        return nuevo;
    }

    public async Task<bool> ActualizarAsync(int id, Proveedore valor)
    {
        ArgumentNullException.ThrowIfNull(valor);
        var actual = await _context.Proveedores.FindAsync(id);
        if (actual is null) return false;
        Validar(valor);
        actual.Nombre = valor.Nombre;
        actual.Contacto = valor.Contacto;
        actual.Telefono = valor.Telefono;
        actual.Email = valor.Email;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var actual = await _context.Proveedores.FindAsync(id);
        if (actual is null) return false;
        if (await _context.Pedidos.AnyAsync(p => p.Proveedorid == id))
            throw new InvalidOperationException("No se puede eliminar un proveedor con pedidos asociados.");
        _context.Proveedores.Remove(actual);
        await _context.SaveChangesAsync();
        return true;
    }

    private static void Validar(Proveedore valor)
    {
        if (string.IsNullOrWhiteSpace(valor.Nombre))
            throw new ArgumentException("El nombre del proveedor es obligatorio.");
        if (valor.Nombre?.Length > 100)
            throw new ArgumentException("Nombre admite hasta 100 caracteres.");
        if (valor.Contacto?.Length > 100)
            throw new ArgumentException("Contacto admite hasta 100 caracteres.");
        if (valor.Telefono?.Length > 20)
            throw new ArgumentException("Telefono admite hasta 20 caracteres.");
        if (valor.Email?.Length > 150)
            throw new ArgumentException("Email admite hasta 150 caracteres.");
    }
}
