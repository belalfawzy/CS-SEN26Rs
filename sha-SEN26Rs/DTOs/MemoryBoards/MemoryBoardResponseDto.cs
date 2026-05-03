namespace sha_SEN26Rs.DTOs.MemoryBoards;

public class MemoryBoardResponseDto
{
    public Guid Id { get; set; }
    public Guid OwnerStudentId { get; set; }
    public string? OwnerName { get; set; }
    public string? Title { get; set; }
    public string? BackgroundUrl { get; set; }
    public string? BackgroundColor { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<BoardItemResponseDto> Items { get; set; } = [];
}

public class BoardItemResponseDto
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public Guid? AuthorStudentId { get; set; }
    public string? AuthorName { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? Content { get; set; }
    public decimal X { get; set; }
    public decimal Y { get; set; }
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }
    public decimal Rotation { get; set; }
    public int ZIndex { get; set; }
    public DateTime CreatedAt { get; set; }
}
