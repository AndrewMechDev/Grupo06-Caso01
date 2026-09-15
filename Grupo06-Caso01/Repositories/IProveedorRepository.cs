using Grupo06_Caso01.Models;

namespace Grupo06_Caso01.Repositories;

public interface IProveedorRepository : IGenericRepository<Proveedore>
{
    Task<Proveedore?> GetByEmailAsync(string email);
}
