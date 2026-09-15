using Grupo06_Caso01.Models;

namespace Grupo06_Caso01.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(
        ApplicationDbContext context,
        ICategoriaRepository categorias,
        IProductoRepository productos,
        IProveedorRepository proveedores,
        IPedidoRepository pedidos,
        IMovimientoInventarioRepository movimientosInventario)
    {
        _context = context;
        Categorias = categorias;
        Productos = productos;
        Proveedores = proveedores;
        Pedidos = pedidos;
        MovimientosInventario = movimientosInventario;
    }

    public ICategoriaRepository Categorias { get; }
    public IProductoRepository Productos { get; }
    public IProveedorRepository Proveedores { get; }
    public IPedidoRepository Pedidos { get; }
    public IMovimientoInventarioRepository MovimientosInventario { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
