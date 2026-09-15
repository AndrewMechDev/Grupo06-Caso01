using Grupo06_Caso01.Models;

namespace Grupo06_Caso01.Repositories;

public interface ICategoriaRepository : IGenericRepository<Categoria>
{
    Task<Categoria?> GetByNameAsync(string nombre);
}
