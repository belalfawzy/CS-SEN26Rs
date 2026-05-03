namespace sha_SEN26Rs.Models;

public class UserImage
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string PublicId { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public int? FileSize { get; set; }
    public string? MimeType { get; set; }
    public string? Caption { get; set; }
    public bool IsPublic { get; set; } = false;
    public DateTime CreatedAt { get; set; }

    public Student Student { get; set; } = null!;
}
