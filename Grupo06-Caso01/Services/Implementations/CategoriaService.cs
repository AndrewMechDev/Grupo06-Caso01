using Grupo06_Caso01.Models;
using Microsoft.EntityFrameworkCore;

namespace Grupo06_Caso01.Services.Implementations;

public class CategoriaService : ICategoriaService
{
    private readonly ApplicationDbContext _context;

    public CategoriaService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Categoria>> ListarAsync()
    {
        return await _context.Categorias.AsNoTracking().OrderBy(x => x.Categoriaid).ToListAsync();
    }

    public async Task<Categoria?> ObtenerPorIdAsync(int id)
    {
        return await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(x => x.Categoriaid == id);
    }

    public async Task<Categoria> CrearAsync(Categoria valor)
    {
        ArgumentNullException.ThrowIfNull(valor);
        Validar(valor);
        var nuevo = new Categoria
        {
            Nombre = valor.Nombre
        };
        _context.Categorias.Add(nuevo);
        await _context.SaveChangesAsync();
        return nuevo;
    }

    public async Task<bool> ActualizarAsync(int id, Categoria valor)
    {
        ArgumentNullException.ThrowIfNull(valor);
        var actual = await _context.Categorias.FindAsync(id);
        if (actual is null) return false;
        Validar(valor);
        actual.Nombre = valor.Nombre;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var actual = await _context.Categorias.FindAsync(id);
        if (actual is null) return false;
        if (await _context.Productos.AnyAsync(p => p.Categoriaid == id))
            throw new InvalidOperationException("No se puede eliminar una categoría con productos asociados.");
        _context.Categorias.Remove(actual);
        await _context.SaveChangesAsync();
        return true;
    }

    private static void Validar(Categoria valor)
    {
        if (string.IsNullOrWhiteSpace(valor.Nombre))
            throw new ArgumentException("El nombre de la categoría es obligatorio.");
        if (valor.Nombre?.Length > 100)
            throw new ArgumentException("Nombre admite hasta 100 caracteres.");
    }
}
