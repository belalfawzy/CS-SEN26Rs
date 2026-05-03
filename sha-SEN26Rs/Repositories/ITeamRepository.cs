using sha_SEN26Rs.Models;

namespace sha_SEN26Rs.Repositories;

public interface ITeamRepository
{
    Task<Team?> GetByIdAsync(Guid id);
    Task<List<Team>> GetAllAsync();
    Task<Team> CreateAsync(Team team);
    Task<Team> UpdateAsync(Team team);
    Task DeleteAsync(Team team);
    Task<bool> TeamNumberExistsAsync(int teamNumber);
    Task<Team?> GetByNumberAsync(int teamNumber);
}
