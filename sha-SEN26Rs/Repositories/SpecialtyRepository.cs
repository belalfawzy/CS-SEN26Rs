using Microsoft.EntityFrameworkCore;
using sha_SEN26Rs.Data;
using sha_SEN26Rs.Models;

namespace sha_SEN26Rs.Repositories;

public class SpecialtyRepository(AppDbContext context) : ISpecialtyRepository
{
    public async Task<List<Specialty>> GetAllAsync() =>
        await context.Specialties.OrderBy(s => s.Name).ToListAsync();

    public async Task<Specialty?> GetByIdAsync(long id) =>
        await context.Specialties.FindAsync(id);

    public async Task<Specialty> CreateAsync(Specialty specialty)
    {
        context.Specialties.Add(specialty);
        await context.SaveChangesAsync();
        return specialty;
    }

    public async Task<List<Specialty>> GetByIdsAsync(List<long> ids) =>
        await context.Specialties.Where(s => ids.Contains(s.Id)).ToListAsync();
}
