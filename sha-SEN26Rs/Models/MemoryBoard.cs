namespace sha_SEN26Rs.Models;

public class MemoryBoard
{
    public Guid Id { get; set; }
    public Guid OwnerStudentId { get; set; }
    public string? Title { get; set; }
    public string? BackgroundUrl { get; set; }
    public string? BackgroundColor { get; set; }
    public int Width { get; set; } = 1200;
    public int Height { get; set; } = 1600;
    public DateTime CreatedAt { get; set; }

    public Student Owner { get; set; } = null!;
    public ICollection<BoardItem> Items { get; set; } = [];
}
