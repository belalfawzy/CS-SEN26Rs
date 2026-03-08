using System.ComponentModel.DataAnnotations;

namespace sha_SEN26Rs.DTOs;

public class SendMessageDto
{
    [Required]
    [StringLength(2000, MinimumLength = 1)]
    public string Content { get; set; } = string.Empty;

    [Required]
    public Guid ReceiverId { get; set; }

    public bool IsAnonymous { get; set; }
}
