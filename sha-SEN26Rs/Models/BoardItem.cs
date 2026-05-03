namespace sha_SEN26Rs.Models;

public class BoardItem
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public Guid? AuthorStudentId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? Content { get; set; }
    public decimal X { get; set; } = 0;
    public decimal Y { get; set; } = 0;
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }
    public decimal Rotation { get; set; } = 0;
    public int ZIndex { get; set; } = 0;
    public DateTime CreatedAt { get; set; }

    public MemoryBoard Board { get; set; } = null!;
    public Student? Author { get; set; }
}
