using System.ComponentModel.DataAnnotations;

namespace sha_SEN26Rs.DTOs;

public class UpdateMessageDto
{
    [Required]
    [StringLength(6000, MinimumLength = 1)]
    public string Content { get; set; } = string.Empty;
}
