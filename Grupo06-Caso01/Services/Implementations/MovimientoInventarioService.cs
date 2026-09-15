using System.Data;
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
        return await _context.Movimientosinventarios.AsNoTracking()
            .OrderByDescending(m => m.Fecha).ThenByDescending(m => m.Movimientoid).ToListAsync();
    }

    public async Task<Movimientosinventario?> ObtenerPorIdAsync(int id)
    {
        return await _context.Movimientosinventarios.AsNoTracking()
            .FirstOrDefaultAsync(m => m.Movimientoid == id);
    }

    public async Task<Movimientosinventario> CrearAsync(Movimientosinventario movimiento)
    {
        ArgumentNullException.ThrowIfNull(movimiento);
        var tipo = Validar(movimiento);
        await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var producto = await ObtenerProductoAsync(movimiento.Productoid);
        var stock = CalcularStock(producto.Stockactual, Efecto(tipo, movimiento.Cantidad));
        var nuevo = new Movimientosinventario
        {
            Productoid = movimiento.Productoid,
            Tipomovimiento = tipo,
            Cantidad = movimiento.Cantidad,
            Fecha = movimiento.Fecha == default ? DateTime.Now : movimiento.Fecha
        };
        producto.Stockactual = stock;
        _context.Movimientosinventarios.Add(nuevo);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return nuevo;
    }

    public async Task<bool> ActualizarAsync(int id, Movimientosinventario movimiento)
    {
        ArgumentNullException.ThrowIfNull(movimiento);
        var tipo = Validar(movimiento);
        await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var actual = await _context.Movimientosinventarios.FindAsync(id);
        if (actual is null) return false;

        var anterior = await ObtenerProductoAsync(actual.Productoid);
        var destino = await ObtenerProductoAsync(movimiento.Productoid);
        var efectoAnterior = Efecto(Validar(actual), actual.Cantidad);
        var efectoNuevo = Efecto(tipo, movimiento.Cantidad);

        // Revertir el movimiento anterior y aplicar el nuevo en una sola transacción.
        if (anterior.Productoid == destino.Productoid)
        {
            anterior.Stockactual = CalcularStock(anterior.Stockactual, (long)efectoNuevo - efectoAnterior);
        }
        else
        {
            var stockAnterior = CalcularStock(anterior.Stockactual, -(long)efectoAnterior);
            var stockDestino = CalcularStock(destino.Stockactual, efectoNuevo);
            anterior.Stockactual = stockAnterior;
            destino.Stockactual = stockDestino;
        }

        actual.Productoid = movimiento.Productoid;
        actual.Tipomovimiento = tipo;
        actual.Cantidad = movimiento.Cantidad;
        if (movimiento.Fecha != default) actual.Fecha = movimiento.Fecha;
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var actual = await _context.Movimientosinventarios.FindAsync(id);
        if (actual is null) return false;
        var producto = await ObtenerProductoAsync(actual.Productoid);
        producto.Stockactual = CalcularStock(producto.Stockactual, -(long)Efecto(Validar(actual), actual.Cantidad));
        _context.Movimientosinventarios.Remove(actual);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return true;
    }

    private async Task<Producto> ObtenerProductoAsync(int id)
    {
        return await _context.Productos.FindAsync(id)
            ?? throw new ArgumentException("El producto indicado no existe.");
    }

    private static string Validar(Movimientosinventario movimiento)
    {
        if (movimiento.Cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor que cero.");
        if (movimiento.Fecha.Kind == DateTimeKind.Utc)
            throw new ArgumentException("La fecha debe ser local, sin zona horaria UTC.");
        var tipo = movimiento.Tipomovimiento?.Trim();
        if (string.Equals(tipo, "Entrada", StringComparison.OrdinalIgnoreCase)) return "Entrada";
        if (string.Equals(tipo, "Salida", StringComparison.OrdinalIgnoreCase)) return "Salida";
        throw new ArgumentException("El tipo de movimiento debe ser Entrada o Salida.");
    }

    private static int Efecto(string tipo, int cantidad) => tipo == "Entrada" ? cantidad : -cantidad;

    private static int CalcularStock(int actual, long cambio)
    {
        var resultado = actual + cambio;
        if (resultado < 0)
            throw new InvalidOperationException("El movimiento dejaría el stock en negativo.");
        if (resultado > int.MaxValue)
            throw new InvalidOperationException("El stock supera el límite permitido.");
        return (int)resultado;
    }
}
