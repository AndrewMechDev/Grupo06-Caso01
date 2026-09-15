using Grupo06_Caso01.Models;
using Grupo06_Caso01.Repositories;

namespace Grupo06_Caso01.Services.Implementations;

public class ProveedorService : IProveedorService
{
    private readonly IUnitOfWork _uow;

    public ProveedorService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<Proveedore>> ListarAsync()
    {
        return (await _uow.Proveedores.GetAllAsync()).OrderBy(x => x.Proveedorid);
    }

    public async Task<Proveedore?> ObtenerPorIdAsync(int id)
    {
        return await _uow.Proveedores.GetByIdAsync(id);
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
        await _uow.Proveedores.AddAsync(nuevo);
        await _uow.SaveChangesAsync();
        return nuevo;
    }

    public async Task<bool> ActualizarAsync(int id, Proveedore valor)
    {
        ArgumentNullException.ThrowIfNull(valor);
        var actual = await _uow.Proveedores.GetByIdAsync(id);
        if (actual is null) return false;
        Validar(valor);
        actual.Nombre = valor.Nombre;
        actual.Contacto = valor.Contacto;
        actual.Telefono = valor.Telefono;
        actual.Email = valor.Email;
        await _uow.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var actual = await _uow.Proveedores.GetByIdAsync(id);
        if (actual is null) return false;
        if (await _uow.Pedidos.ExisteParaProveedorAsync(id))
            throw new InvalidOperationException("No se puede eliminar un proveedor con pedidos asociados.");
        await _uow.Proveedores.DeleteAsync(id);
        await _uow.SaveChangesAsync();
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
