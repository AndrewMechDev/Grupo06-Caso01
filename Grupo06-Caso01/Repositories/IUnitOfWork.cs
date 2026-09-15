using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Grupo06_Caso01.Repositories;

public interface IUnitOfWork
{
    ICategoriaRepository Categorias { get; }
    IProductoRepository Productos { get; }
    IProveedorRepository Proveedores { get; }
    IPedidoRepository Pedidos { get; }
    IMovimientoInventarioRepository MovimientosInventario { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IDbContextTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.Serializable,
        CancellationToken cancellationToken = default);
}
