using Grupo06_Caso01.Models;
using Microsoft.EntityFrameworkCore;

namespace Grupo06_Caso01.Repositories.Implementations;

public class CategoriaRepository(ApplicationDbContext context)
    : GenericRepository<Categoria>(context), ICategoriaRepository
{
    public async Task<Categoria?> GetByNameAsync(string nombre) =>
        await Entities.AsNoTracking().FirstOrDefaultAsync(c => c.Nombre == nombre);
}
