using Grupo06_Caso01.Models;
using Microsoft.EntityFrameworkCore;

namespace Grupo06_Caso01.Repositories.Implementations;

public class ProveedorRepository(ApplicationDbContext context)
    : GenericRepository<Proveedore>(context), IProveedorRepository
{
    public async Task<Proveedore?> GetByEmailAsync(string email) =>
        await Entities.AsNoTracking().FirstOrDefaultAsync(p => p.Email == email);
}
