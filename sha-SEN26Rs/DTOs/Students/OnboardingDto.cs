using System.ComponentModel.DataAnnotations;

namespace sha_SEN26Rs.DTOs.Students;

public class OnboardingDto
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    public string? Nickname { get; set; }

    [StringLength(500)]
    public string? Bio { get; set; }

    public string? Phone { get; set; }
    public string? Location { get; set; }
    public string? Website { get; set; }
    public string? GraduationProjectSpecialty { get; set; }
    [AllowedValues("public", "students_only", "private")]
    public string PrivacySetting { get; set; } = "public";
    public int? TeamNumber { get; set; }
    public List<SocialLinkInputDto> SocialLinks { get; set; } = [];
}

public class SocialLinkInputDto
{
    [Required]
    public string Platform { get; set; } = string.Empty;

    [Required]
    public string Url { get; set; } = string.Empty;
}
