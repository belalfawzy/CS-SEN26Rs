using sha_SEN26Rs.DTOs.Students;
using sha_SEN26Rs.Models;
using sha_SEN26Rs.Repositories;
using System.Text.RegularExpressions;

namespace sha_SEN26Rs.Services;

public interface IStudentService
{
    Task<List<StudentResponseDto>> GetAllAsync();
    Task<List<StudentResponseDto>> SearchAsync(string query);
    Task<StudentResponseDto> GetByUsernameAsync(string username);
    Task<StudentResponseDto> GetByIdAsync(Guid id);
    Task<StudentResponseDto> UpdateAsync(Guid studentId, UpdateStudentDto dto);
    Task<StudentResponseDto> OnboardAsync(Guid studentId, OnboardingDto dto);
    Task JoinTeamAsync(Guid studentId, Guid teamId);
    Task LeaveTeamAsync(Guid studentId);
    Task AddSpecialtyAsync(Guid studentId, long specialtyId);
    Task RemoveSpecialtyAsync(Guid studentId, long specialtyId);
    Task<SocialLinkDto> AddSocialLinkAsync(Guid studentId, SocialLinkInputDto dto);
    Task RemoveSocialLinkAsync(Guid studentId, long linkId);
}

public class StudentService(
    IStudentRepository studentRepo,
    ITeamRepository teamRepo) : IStudentService
{
    public async Task<List<StudentResponseDto>> GetAllAsync()
    {
        var students = await studentRepo.GetAllAsync();
        return students.Select(AuthService.MapToDto).ToList();
    }

    public async Task<List<StudentResponseDto>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            throw new InvalidOperationException("Search query cannot be empty.");

        var students = await studentRepo.SearchAsync(query);
        return students.Select(AuthService.MapToDto).ToList();
    }

    public async Task<StudentResponseDto> GetByUsernameAsync(string username)
    {
        var student = await studentRepo.GetByUsernameAsync(username)
            ?? throw new KeyNotFoundException("Student not found.");
        return AuthService.MapToDto(student);
    }

    public async Task<StudentResponseDto> GetByIdAsync(Guid id)
    {
        var student = await studentRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Student not found.");
        return AuthService.MapToDto(student);
    }

    public async Task<StudentResponseDto> UpdateAsync(Guid studentId, UpdateStudentDto dto)
    {
        var student = await studentRepo.GetByIdAsync(studentId)
            ?? throw new KeyNotFoundException("Student not found.");

        bool changed = false;

        if (dto.FullName is not null && dto.FullName != student.FullName)
            { student.FullName = dto.FullName; changed = true; }

        if (dto.Nickname is not null && dto.Nickname != student.Nickname)
            { student.Nickname = dto.Nickname; changed = true; }

        if (dto.Bio is not null && dto.Bio != student.Bio)
            { student.Bio = string.IsNullOrWhiteSpace(dto.Bio) ? null : dto.Bio; changed = true; }

        if (dto.AvatarUrl is not null && dto.AvatarUrl != student.AvatarUrl)
            { student.AvatarUrl = dto.AvatarUrl; changed = true; }

        if (dto.CoverUrl is not null && dto.CoverUrl != student.CoverUrl)
            { student.CoverUrl = dto.CoverUrl; changed = true; }

        if (dto.Phone is not null && dto.Phone != student.Phone)
            { student.Phone = string.IsNullOrWhiteSpace(dto.Phone) ? null : dto.Phone; changed = true; }

        if (dto.Location is not null && dto.Location != student.Location)
            { student.Location = string.IsNullOrWhiteSpace(dto.Location) ? null : dto.Location; changed = true; }

        if (dto.Website is not null && dto.Website != student.Website)
            { student.Website = string.IsNullOrWhiteSpace(dto.Website) ? null : dto.Website; changed = true; }

        if (dto.GraduationProjectSpecialty is not null && dto.GraduationProjectSpecialty != student.GraduationProjectSpecialty)
            { student.GraduationProjectSpecialty = dto.GraduationProjectSpecialty; changed = true; }

        if (dto.PrivacySetting is not null && dto.PrivacySetting != student.PrivacySetting)
            { student.PrivacySetting = dto.PrivacySetting; changed = true; }

        if (dto.TeamNumber.HasValue && student.Team?.TeamNumber != dto.TeamNumber.Value)
        {
            var team = await teamRepo.GetByNumberAsync(dto.TeamNumber.Value)
                ?? await teamRepo.CreateAsync(new Team
                {
                    Name = $"Team {dto.TeamNumber.Value}",
                    TeamNumber = dto.TeamNumber.Value
                });
            student.TeamId = team.Id;
            changed = true;
        }

        if (dto.SocialLinks is not null)
        {
            var currentSet = student.SocialLinks
                .Select(l => $"{l.Platform.ToLower()}|{l.Url}").ToHashSet();
            var newSet = dto.SocialLinks
                .Select(l => $"{l.Platform.ToLower()}|{l.Url}").ToHashSet();

            if (!currentSet.SetEquals(newSet))
            {
                student.SocialLinks = dto.SocialLinks.Select(l => new SocialLink
                {
                    StudentId = student.Id,
                    Platform = l.Platform,
                    Url = l.Url,
                    CreatedAt = DateTime.UtcNow
                }).ToList();
                changed = true;
            }
        }

        if (!changed)
            throw new InvalidOperationException("No changes detected.");

        var updated = await studentRepo.UpdateAsync(student);
        return AuthService.MapToDto(updated);
    }

    public async Task<StudentResponseDto> OnboardAsync(Guid studentId, OnboardingDto dto)
    {
        var student = await studentRepo.GetByIdAsync(studentId)
            ?? throw new KeyNotFoundException("Student not found.");

        student.FullName = dto.FullName;
        student.Nickname = dto.Nickname;
        student.Bio = dto.Bio;
        student.Phone = dto.Phone;
        student.Location = dto.Location;
        student.Website = dto.Website;
        student.GraduationProjectSpecialty = dto.GraduationProjectSpecialty;
        student.PrivacySetting = dto.PrivacySetting;
        student.IsOnboarded = true;

        var baseUsername = Regex.Replace(
            (dto.Nickname ?? dto.FullName).ToLower().Trim(), @"[^a-z0-9]+", "_").Trim('_');
        var username = baseUsername;
        var counter = 2;
        while (await studentRepo.UsernameExistsAsync(username) &&
               (await studentRepo.GetByUsernameAsync(username))?.Id != student.Id)
            username = $"{baseUsername}_{counter++}";
        student.Username = username;

        if (dto.TeamNumber.HasValue)
        {
            var team = await teamRepo.GetByNumberAsync(dto.TeamNumber.Value)
                ?? await teamRepo.CreateAsync(new Team
                {
                    Name = $"Team {dto.TeamNumber.Value}",
                    TeamNumber = dto.TeamNumber.Value
                });
            student.TeamId = team.Id;
        }

        student.SocialLinks = dto.SocialLinks.Select(l => new SocialLink
        {
            StudentId = student.Id,
            Platform = l.Platform,
            Url = l.Url,
            CreatedAt = DateTime.UtcNow
        }).ToList();

        var updated = await studentRepo.UpdateAsync(student);
        return AuthService.MapToDto(updated);
    }

    public async Task JoinTeamAsync(Guid studentId, Guid teamId)
    {
        var student = await studentRepo.GetByIdAsync(studentId)
            ?? throw new KeyNotFoundException("Student not found.");
        student.TeamId = teamId;
        await studentRepo.UpdateAsync(student);
    }

    public async Task LeaveTeamAsync(Guid studentId)
    {
        var student = await studentRepo.GetByIdAsync(studentId)
            ?? throw new KeyNotFoundException("Student not found.");
        student.TeamId = null;
        await studentRepo.UpdateAsync(student);
    }

    public async Task AddSpecialtyAsync(Guid studentId, long specialtyId)
    {
        var student = await studentRepo.GetByIdAsync(studentId)
            ?? throw new KeyNotFoundException("Student not found.");

        if (student.StudentSpecialties.Any(ss => ss.SpecialtyId == specialtyId))
            return;

        student.StudentSpecialties.Add(new StudentSpecialty
        {
            StudentId = studentId,
            SpecialtyId = specialtyId
        });
        await studentRepo.UpdateAsync(student);
    }

    public async Task RemoveSpecialtyAsync(Guid studentId, long specialtyId)
    {
        var student = await studentRepo.GetByIdAsync(studentId)
            ?? throw new KeyNotFoundException("Student not found.");

        var entry = student.StudentSpecialties.FirstOrDefault(ss => ss.SpecialtyId == specialtyId);
        if (entry is not null)
        {
            student.StudentSpecialties.Remove(entry);
            await studentRepo.UpdateAsync(student);
        }
    }

    public async Task<SocialLinkDto> AddSocialLinkAsync(Guid studentId, SocialLinkInputDto dto)
    {
        var student = await studentRepo.GetByIdAsync(studentId)
            ?? throw new KeyNotFoundException("Student not found.");

        var link = new SocialLink
        {
            StudentId = studentId,
            Platform = dto.Platform,
            Url = dto.Url,
            CreatedAt = DateTime.UtcNow
        };
        student.SocialLinks.Add(link);
        await studentRepo.UpdateAsync(student);

        return new SocialLinkDto { Id = link.Id, Platform = link.Platform, Url = link.Url };
    }

    public async Task RemoveSocialLinkAsync(Guid studentId, long linkId)
    {
        var student = await studentRepo.GetByIdAsync(studentId)
            ?? throw new KeyNotFoundException("Student not found.");

        var link = student.SocialLinks.FirstOrDefault(l => l.Id == linkId)
            ?? throw new KeyNotFoundException("Social link not found.");

        student.SocialLinks.Remove(link);
        await studentRepo.UpdateAsync(student);
    }
}
