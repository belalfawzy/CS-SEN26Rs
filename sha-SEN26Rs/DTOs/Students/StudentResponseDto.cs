namespace sha_SEN26Rs.DTOs.Students;

public class StudentResponseDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Nickname { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
    public string? CoverUrl { get; set; }
    public int GraduationYear { get; set; }
    public string? GraduationProjectSpecialty { get; set; }
    public bool IsOnboarded { get; set; }
    public string? Phone { get; set; }
    public string? Location { get; set; }
    public string? Website { get; set; }
    public string PrivacySetting { get; set; } = string.Empty;
    public Guid? TeamId { get; set; }
    public string? TeamName { get; set; }
    public List<string> Specialties { get; set; } = [];
    public List<SocialLinkDto> SocialLinks { get; set; } = [];
    public int ImageCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SocialLinkDto
{
    public long Id { get; set; }
    public string Platform { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}
