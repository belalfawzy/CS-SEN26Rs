using sha_SEN26Rs.DTOs.Students;
using sha_SEN26Rs.Models;
using sha_SEN26Rs.Repositories;
using System.Text.RegularExpressions;

namespace sha_SEN26Rs.Services;

public interface IStudentService
{
    Task<List<StudentResponseDto>> GetAllAsync();
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

        if (dto.FullName is not null) student.FullName = dto.FullName;
        if (dto.Nickname is not null) student.Nickname = dto.Nickname;
        if (dto.Bio is not null) student.Bio = dto.Bio;
        if (dto.AvatarUrl is not null) student.AvatarUrl = dto.AvatarUrl;
        if (dto.CoverUrl is not null) student.CoverUrl = dto.CoverUrl;
        if (dto.Phone is not null) student.Phone = dto.Phone;
        if (dto.Location is not null) student.Location = dto.Location;
        if (dto.Website is not null) student.Website = dto.Website;
        if (dto.GraduationProjectSpecialty is not null) student.GraduationProjectSpecialty = dto.GraduationProjectSpecialty;
        if (dto.PrivacySetting is not null) student.PrivacySetting = dto.PrivacySetting;

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
