using Grupo06_Caso01.Models;
using Grupo06_Caso01.Repositories;

namespace Grupo06_Caso01.Services.Implementations;

public class ProductoService : IProductoService
{
    private readonly IUnitOfWork _uow;

    public ProductoService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<Producto>> ListarAsync()
    {
        return (await _uow.Productos.GetAllAsync()).OrderBy(x => x.Productoid);
    }

    public async Task<Producto?> ObtenerPorIdAsync(int id)
    {
        return await _uow.Productos.GetByIdAsync(id);
    }

    public async Task<Producto> CrearAsync(Producto valor)
    {
        ArgumentNullException.ThrowIfNull(valor);
        await ValidarAsync(valor);
        var nuevo = new Producto
        {
            Nombre = valor.Nombre,
            Descripcion = valor.Descripcion,
            Precio = valor.Precio,
            Stockactual = valor.Stockactual,
            Stockminimo = valor.Stockminimo,
            Categoriaid = valor.Categoriaid
        };
        await _uow.Productos.AddAsync(nuevo);
        await _uow.SaveChangesAsync();
        return nuevo;
    }

    public async Task<bool> ActualizarAsync(int id, Producto valor)
    {
        ArgumentNullException.ThrowIfNull(valor);
        var actual = await _uow.Productos.GetByIdAsync(id);
        if (actual is null) return false;
        await ValidarAsync(valor);
        actual.Nombre = valor.Nombre;
        actual.Descripcion = valor.Descripcion;
        actual.Precio = valor.Precio;
        actual.Stockactual = valor.Stockactual;
        actual.Stockminimo = valor.Stockminimo;
        actual.Categoriaid = valor.Categoriaid;
        await _uow.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var actual = await _uow.Productos.GetByIdAsync(id);
        if (actual is null) return false;
        if (await _uow.MovimientosInventario.ExisteParaProductoAsync(id))
            throw new InvalidOperationException("No se puede eliminar un producto con movimientos asociados.");
        await _uow.Productos.DeleteAsync(id);
        await _uow.SaveChangesAsync();
        return true;
    }

    private async Task ValidarAsync(Producto valor)
    {
        if (string.IsNullOrWhiteSpace(valor.Nombre))
            throw new ArgumentException("El nombre del producto es obligatorio.");
        if (valor.Precio < 0 || valor.Stockactual < 0 || valor.Stockminimo < 0)
            throw new ArgumentException("El precio y los valores de stock no pueden ser negativos.");
        if ((await _uow.Categorias.GetByIdAsync(valor.Categoriaid)) is null)
            throw new ArgumentException("La categoría indicada no existe.");
        if (valor.Nombre?.Length > 100)
            throw new ArgumentException("Nombre admite hasta 100 caracteres.");
    }
}
