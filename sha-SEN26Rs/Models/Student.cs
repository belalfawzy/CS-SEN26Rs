namespace sha_SEN26Rs.Models;

public class Student
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Nickname { get; set; }
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
    public string? CoverUrl { get; set; }
    public int GraduationYear { get; set; } = 2026;
    public string? GraduationProjectSpecialty { get; set; }
    public bool IsOnboarded { get; set; } = false;
    public string? Phone { get; set; }
    public string? Location { get; set; }
    public string? Website { get; set; }
    public string PrivacySetting { get; set; } = "public";
    public Guid? TeamId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Team? Team { get; set; }
    public MemoryBoard? MemoryBoard { get; set; }
    public ICollection<StudentSpecialty> StudentSpecialties { get; set; } = [];
    public ICollection<SocialLink> SocialLinks { get; set; } = [];
    public ICollection<UserImage> Images { get; set; } = [];
    public ICollection<Message> SentMessages { get; set; } = [];
    public ICollection<Message> ReceivedMessages { get; set; } = [];
}
