using sha_SEN26Rs.Models;
using sha_SEN26Rs.Repositories;

namespace sha_SEN26Rs.Services;

public interface ISpecialtyService
{
    Task<List<SpecialtyDto>> GetAllAsync();
    Task<SpecialtyDto> CreateAsync(string name);
}

public record SpecialtyDto(long Id, string Name);

public class SpecialtyService(ISpecialtyRepository specialtyRepo) : ISpecialtyService
{
    public async Task<List<SpecialtyDto>> GetAllAsync()
    {
        var specialties = await specialtyRepo.GetAllAsync();
        return specialties.Select(s => new SpecialtyDto(s.Id, s.Name)).ToList();
    }

    public async Task<SpecialtyDto> CreateAsync(string name)
    {
        var specialty = await specialtyRepo.CreateAsync(new Specialty { Name = name });
        return new SpecialtyDto(specialty.Id, specialty.Name);
    }
}
