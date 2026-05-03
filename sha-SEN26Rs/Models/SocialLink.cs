namespace sha_SEN26Rs.Models;

public class SocialLink
{
    public long Id { get; set; }
    public Guid StudentId { get; set; }
    public string Platform { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public Student Student { get; set; } = null!;
}
