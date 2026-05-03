using System.ComponentModel.DataAnnotations;

namespace sha_SEN26Rs.DTOs.Teams;

public class CreateTeamDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    public string? ProjectName { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? CoverUrl { get; set; }
    public int? TeamNumber { get; set; }
}
