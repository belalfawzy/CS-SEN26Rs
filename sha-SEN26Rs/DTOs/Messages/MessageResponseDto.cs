namespace sha_SEN26Rs.DTOs.Messages;

public class MessageResponseDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? SenderName { get; set; }
    public string? SenderUsername { get; set; }
    public string? SenderAvatarUrl { get; set; }
    public Guid ReceiverId { get; set; }
    public string ReceiverName { get; set; } = string.Empty;
    public bool IsAnonymous { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
