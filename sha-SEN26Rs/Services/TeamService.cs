using sha_SEN26Rs.DTOs.Teams;
using sha_SEN26Rs.Models;
using sha_SEN26Rs.Repositories;

namespace sha_SEN26Rs.Services;

public interface ITeamService
{
    Task<List<TeamResponseDto>> GetAllAsync();
    Task<TeamResponseDto> GetByIdAsync(Guid id);
    Task<TeamResponseDto> CreateAsync(CreateTeamDto dto);
    Task<TeamResponseDto> UpdateAsync(Guid id, UpdateTeamDto dto);
    Task DeleteAsync(Guid id);
}

public class TeamService(ITeamRepository teamRepo) : ITeamService
{
    public async Task<List<TeamResponseDto>> GetAllAsync()
    {
        var teams = await teamRepo.GetAllAsync();
        return teams.Select(MapToDto).ToList();
    }

    public async Task<TeamResponseDto> GetByIdAsync(Guid id)
    {
        var team = await teamRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Team not found.");
        return MapToDto(team);
    }

    public async Task<TeamResponseDto> CreateAsync(CreateTeamDto dto)
    {
        if (dto.TeamNumber.HasValue && await teamRepo.TeamNumberExistsAsync(dto.TeamNumber.Value))
            throw new InvalidOperationException("Team number already exists.");

        var team = new Team
        {
            Name = dto.Name,
            ProjectName = dto.ProjectName,
            Description = dto.Description,
            LogoUrl = dto.LogoUrl,
            CoverUrl = dto.CoverUrl,
            TeamNumber = dto.TeamNumber
        };

        var created = await teamRepo.CreateAsync(team);
        return MapToDto(created);
    }

    public async Task<TeamResponseDto> UpdateAsync(Guid id, UpdateTeamDto dto)
    {
        var team = await teamRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Team not found.");

        if (dto.Name is not null) team.Name = dto.Name;
        if (dto.ProjectName is not null) team.ProjectName = dto.ProjectName;
        if (dto.Description is not null) team.Description = dto.Description;
        if (dto.LogoUrl is not null) team.LogoUrl = dto.LogoUrl;
        if (dto.CoverUrl is not null) team.CoverUrl = dto.CoverUrl;
        if (dto.TeamNumber.HasValue)
        {
            if (await teamRepo.TeamNumberExistsAsync(dto.TeamNumber.Value))
                throw new InvalidOperationException("Team number already exists.");
            team.TeamNumber = dto.TeamNumber;
        }

        var updated = await teamRepo.UpdateAsync(team);
        return MapToDto(updated);
    }

    public async Task DeleteAsync(Guid id)
    {
        var team = await teamRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Team not found.");
        await teamRepo.DeleteAsync(team);
    }

    private static TeamResponseDto MapToDto(Team t) => new()
    {
        Id = t.Id,
        Name = t.Name,
        ProjectName = t.ProjectName,
        Description = t.Description,
        LogoUrl = t.LogoUrl,
        CoverUrl = t.CoverUrl,
        TeamNumber = t.TeamNumber,
        CreatedAt = t.CreatedAt,
        Members = t.Members.Select(m => new TeamMemberDto
        {
            Id = m.Id,
            Username = m.Username,
            FullName = m.FullName,
            Nickname = m.Nickname,
            AvatarUrl = m.AvatarUrl,
            GraduationProjectSpecialty = m.GraduationProjectSpecialty
        }).ToList()
    };
}
