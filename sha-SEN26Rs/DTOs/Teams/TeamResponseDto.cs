namespace sha_SEN26Rs.DTOs.Teams;

public class TeamResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ProjectName { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? CoverUrl { get; set; }
    public int? TeamNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<TeamMemberDto> Members { get; set; } = [];
}

public class TeamMemberDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Nickname { get; set; }
    public string? AvatarUrl { get; set; }
    public string? GraduationProjectSpecialty { get; set; }
}
