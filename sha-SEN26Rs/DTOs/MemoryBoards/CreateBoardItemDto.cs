using System.ComponentModel.DataAnnotations;

namespace sha_SEN26Rs.DTOs.MemoryBoards;

public class CreateBoardItemDto
{
    [Required]
    public string Type { get; set; } = string.Empty;

    public string? Content { get; set; }
    public decimal X { get; set; } = 0;
    public decimal Y { get; set; } = 0;
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }
    public decimal Rotation { get; set; } = 0;
    public int ZIndex { get; set; } = 0;
}
