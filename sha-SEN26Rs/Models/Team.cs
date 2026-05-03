namespace sha_SEN26Rs.Models;

public class Team
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ProjectName { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? CoverUrl { get; set; }
    public int? TeamNumber { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<Student> Members { get; set; } = [];
}
