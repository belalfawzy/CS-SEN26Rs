namespace sha_SEN26Rs.DTOs.MemoryBoards;

public class UpdateBoardItemDto
{
    public string? Content { get; set; }
    public decimal? X { get; set; }
    public decimal? Y { get; set; }
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }
    public decimal? Rotation { get; set; }
    public int? ZIndex { get; set; }
}
