using sha_SEN26Rs.Models;

namespace sha_SEN26Rs.Repositories;

public interface ISpecialtyRepository
{
    Task<List<Specialty>> GetAllAsync();
    Task<Specialty?> GetByIdAsync(long id);
    Task<Specialty> CreateAsync(Specialty specialty);
    Task<List<Specialty>> GetByIdsAsync(List<long> ids);
}
