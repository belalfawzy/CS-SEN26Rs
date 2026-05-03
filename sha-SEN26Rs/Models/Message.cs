namespace sha_SEN26Rs.Models;

public class Message
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Content { get; set; } = string.Empty;
    public Guid? SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public bool IsAnonymous { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Student? Sender { get; set; }
    public Student Receiver { get; set; } = null!;
}
