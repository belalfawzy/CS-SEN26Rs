using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using sha_SEN26Rs.DTOs.Auth;
using sha_SEN26Rs.DTOs.Students;
using sha_SEN26Rs.Models;
using sha_SEN26Rs.Repositories;

namespace sha_SEN26Rs.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}

public class AuthService(IStudentRepository studentRepo, IConfiguration config) : IAuthService
{
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        if (await studentRepo.EmailExistsAsync(dto.Email))
            throw new InvalidOperationException("Email already registered.");

        var baseUsername = dto.Email.Split('@')[0]
            .ToLower()
            .Replace(".", "_")
            .Replace("-", "_")
            .Replace("+", "_");

        var username = baseUsername;
        var counter = 2;
        while (await studentRepo.UsernameExistsAsync(username))
            username = $"{baseUsername}_{counter++}";

        var student = new Student
        {
            FullName = "New User",
            Username = username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        var created = await studentRepo.CreateAsync(student);
        return new AuthResponseDto { Token = GenerateToken(created), Student = MapToDto(created) };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var student = await studentRepo.GetByEmailAsync(dto.Email)
            ?? throw new UnauthorizedAccessException("Invalid email or password.");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, student.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        return new AuthResponseDto { Token = GenerateToken(student), Student = MapToDto(student) };
    }

    private string GenerateToken(Student student)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, student.Id.ToString()),
            new Claim(ClaimTypes.Name, student.Username),
            new Claim(ClaimTypes.Email, student.Email)
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    internal static StudentResponseDto MapToDto(Student s) => new()
    {
        Id = s.Id,
        Username = s.Username,
        FullName = s.FullName,
        Nickname = s.Nickname,
        Email = s.Email,
        Bio = s.Bio,
        AvatarUrl = s.AvatarUrl,
        CoverUrl = s.CoverUrl,
        GraduationYear = s.GraduationYear,
        GraduationProjectSpecialty = s.GraduationProjectSpecialty,
        IsOnboarded = s.IsOnboarded,
        Phone = s.Phone,
        Location = s.Location,
        Website = s.Website,
        PrivacySetting = s.PrivacySetting,
        TeamId = s.TeamId,
        TeamName = s.Team?.Name,
        Specialties = s.StudentSpecialties.Select(ss => ss.Specialty.Name).ToList(),
        SocialLinks = s.SocialLinks.Select(l => new SocialLinkDto { Id = l.Id, Platform = l.Platform, Url = l.Url }).ToList(),
        CreatedAt = s.CreatedAt
    };
}
