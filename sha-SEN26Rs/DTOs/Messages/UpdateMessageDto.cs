using System.ComponentModel.DataAnnotations;

namespace sha_SEN26Rs.DTOs.Messages;

public class UpdateMessageDto
{
    [Required]
    [StringLength(2000, MinimumLength = 1)]
    public string Content { get; set; } = string.Empty;
}
