using System.ComponentModel.DataAnnotations;

namespace sha_SEN26Rs.DTOs.Students;

public class UpdateStudentDto
{
    [StringLength(100, MinimumLength = 2)]
    public string? FullName { get; set; }

    public string? Nickname { get; set; }

    [StringLength(500)]
    public string? Bio { get; set; }

    public string? AvatarUrl { get; set; }
    public string? CoverUrl { get; set; }
    public string? Phone { get; set; }
    public string? Location { get; set; }
    public string? Website { get; set; }
    public string? GraduationProjectSpecialty { get; set; }
    public string? PrivacySetting { get; set; }
}
