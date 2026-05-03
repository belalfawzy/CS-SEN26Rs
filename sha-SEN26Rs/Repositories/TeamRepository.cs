using Microsoft.EntityFrameworkCore;
using sha_SEN26Rs.Data;
using sha_SEN26Rs.Models;

namespace sha_SEN26Rs.Repositories;

public class TeamRepository(AppDbContext context) : ITeamRepository
{
    public async Task<Team?> GetByIdAsync(Guid id) =>
        await context.Teams
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<List<Team>> GetAllAsync() =>
        await context.Teams
            .Include(t => t.Members)
            .OrderBy(t => t.TeamNumber)
            .ToListAsync();

    public async Task<Team> CreateAsync(Team team)
    {
        team.Id = Guid.NewGuid();
        team.CreatedAt = DateTime.UtcNow;
        context.Teams.Add(team);
        await context.SaveChangesAsync();
        return team;
    }

    public async Task<Team> UpdateAsync(Team team)
    {
        context.Teams.Update(team);
        await context.SaveChangesAsync();
        return team;
    }

    public async Task DeleteAsync(Team team)
    {
        context.Teams.Remove(team);
        await context.SaveChangesAsync();
    }

    public async Task<bool> TeamNumberExistsAsync(int teamNumber) =>
        await context.Teams.AnyAsync(t => t.TeamNumber == teamNumber);

    public async Task<Team?> GetByNumberAsync(int teamNumber) =>
        await context.Teams
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.TeamNumber == teamNumber);
}
