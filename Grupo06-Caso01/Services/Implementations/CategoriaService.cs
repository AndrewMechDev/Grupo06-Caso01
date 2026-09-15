using Grupo06_Caso01.Models;
using Grupo06_Caso01.Repositories;

namespace Grupo06_Caso01.Services.Implementations;

public class CategoriaService : ICategoriaService
{
    private readonly IUnitOfWork _uow;

    public CategoriaService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<Categoria>> ListarAsync()
    {
        return (await _uow.Categorias.GetAllAsync()).OrderBy(x => x.Categoriaid);
    }

    public async Task<Categoria?> ObtenerPorIdAsync(int id)
    {
        return await _uow.Categorias.GetByIdAsync(id);
    }

    public async Task<Categoria> CrearAsync(Categoria valor)
    {
        ArgumentNullException.ThrowIfNull(valor);
        Validar(valor);
        var nuevo = new Categoria
        {
            Nombre = valor.Nombre
        };
        await _uow.Categorias.AddAsync(nuevo);
        await _uow.SaveChangesAsync();
        return nuevo;
    }

    public async Task<bool> ActualizarAsync(int id, Categoria valor)
    {
        ArgumentNullException.ThrowIfNull(valor);
        var actual = await _uow.Categorias.GetByIdAsync(id);
        if (actual is null) return false;
        Validar(valor);
        actual.Nombre = valor.Nombre;
        await _uow.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var actual = await _uow.Categorias.GetByIdAsync(id);
        if (actual is null) return false;
        if (await _uow.Productos.ExisteParaCategoriaAsync(id))
            throw new InvalidOperationException("No se puede eliminar una categoría con productos asociados.");
        await _uow.Categorias.DeleteAsync(id);
        await _uow.SaveChangesAsync();
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
