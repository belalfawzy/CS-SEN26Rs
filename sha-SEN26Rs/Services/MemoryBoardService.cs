using sha_SEN26Rs.DTOs.MemoryBoards;
using sha_SEN26Rs.Models;
using sha_SEN26Rs.Repositories;

namespace sha_SEN26Rs.Services;

public interface IMemoryBoardService
{
    Task<MemoryBoardResponseDto> GetOrCreateAsync(Guid studentId);
    Task<MemoryBoardResponseDto> GetByStudentIdAsync(Guid studentId);
    Task<MemoryBoardResponseDto> UpdateAsync(Guid studentId, UpdateMemoryBoardDto dto);
    Task<BoardItemResponseDto> AddItemAsync(Guid boardId, Guid authorId, CreateBoardItemDto dto);
    Task<BoardItemResponseDto> UpdateItemAsync(Guid itemId, Guid requesterId, UpdateBoardItemDto dto);
    Task DeleteItemAsync(Guid itemId, Guid requesterId);
}

public class MemoryBoardService(IMemoryBoardRepository boardRepo) : IMemoryBoardService
{
    public async Task<MemoryBoardResponseDto> GetOrCreateAsync(Guid studentId)
    {
        var board = await boardRepo.GetByStudentIdAsync(studentId);
        if (board is null)
        {
            board = await boardRepo.CreateAsync(new MemoryBoard { OwnerStudentId = studentId });
            board = await boardRepo.GetByStudentIdAsync(studentId) ?? board;
        }
        return MapToDto(board);
    }

    public async Task<MemoryBoardResponseDto> GetByStudentIdAsync(Guid studentId)
    {
        var board = await boardRepo.GetByStudentIdAsync(studentId)
            ?? throw new KeyNotFoundException("Memory board not found.");
        return MapToDto(board);
    }

    public async Task<MemoryBoardResponseDto> UpdateAsync(Guid studentId, UpdateMemoryBoardDto dto)
    {
        var board = await boardRepo.GetByStudentIdAsync(studentId)
            ?? throw new KeyNotFoundException("Memory board not found.");

        if (dto.Title is not null) board.Title = dto.Title;
        if (dto.BackgroundUrl is not null) board.BackgroundUrl = dto.BackgroundUrl;
        if (dto.BackgroundColor is not null) board.BackgroundColor = dto.BackgroundColor;
        if (dto.Width.HasValue) board.Width = dto.Width.Value;
        if (dto.Height.HasValue) board.Height = dto.Height.Value;

        var updated = await boardRepo.UpdateAsync(board);
        return MapToDto(updated);
    }

    public async Task<BoardItemResponseDto> AddItemAsync(Guid boardId, Guid authorId, CreateBoardItemDto dto)
    {
        var item = new BoardItem
        {
            BoardId = boardId,
            AuthorStudentId = authorId,
            Type = dto.Type,
            Content = dto.Content,
            X = dto.X,
            Y = dto.Y,
            Width = dto.Width,
            Height = dto.Height,
            Rotation = dto.Rotation,
            ZIndex = dto.ZIndex
        };

        var created = await boardRepo.AddItemAsync(item);
        return MapItemToDto(created);
    }

    public async Task<BoardItemResponseDto> UpdateItemAsync(Guid itemId, Guid requesterId, UpdateBoardItemDto dto)
    {
        var item = await boardRepo.GetItemByIdAsync(itemId)
            ?? throw new KeyNotFoundException("Item not found.");

        if (item.AuthorStudentId != requesterId)
            throw new UnauthorizedAccessException("You can only edit your own items.");

        if (dto.Content is not null) item.Content = dto.Content;
        if (dto.X.HasValue) item.X = dto.X.Value;
        if (dto.Y.HasValue) item.Y = dto.Y.Value;
        if (dto.Width.HasValue) item.Width = dto.Width.Value;
        if (dto.Height.HasValue) item.Height = dto.Height.Value;
        if (dto.Rotation.HasValue) item.Rotation = dto.Rotation.Value;
        if (dto.ZIndex.HasValue) item.ZIndex = dto.ZIndex.Value;

        var updated = await boardRepo.UpdateItemAsync(item);
        return MapItemToDto(updated);
    }

    public async Task DeleteItemAsync(Guid itemId, Guid requesterId)
    {
        var item = await boardRepo.GetItemByIdAsync(itemId)
            ?? throw new KeyNotFoundException("Item not found.");

        if (item.AuthorStudentId != requesterId)
            throw new UnauthorizedAccessException("You can only delete your own items.");

        await boardRepo.DeleteItemAsync(item);
    }

    private static MemoryBoardResponseDto MapToDto(MemoryBoard b) => new()
    {
        Id = b.Id,
        OwnerStudentId = b.OwnerStudentId,
        OwnerName = b.Owner?.FullName,
        Title = b.Title,
        BackgroundUrl = b.BackgroundUrl,
        BackgroundColor = b.BackgroundColor,
        Width = b.Width,
        Height = b.Height,
        CreatedAt = b.CreatedAt,
        Items = b.Items.OrderBy(i => i.ZIndex).Select(MapItemToDto).ToList()
    };

    private static BoardItemResponseDto MapItemToDto(BoardItem i) => new()
    {
        Id = i.Id,
        BoardId = i.BoardId,
        AuthorStudentId = i.AuthorStudentId,
        AuthorName = i.Author?.FullName,
        Type = i.Type,
        Content = i.Content,
        X = i.X,
        Y = i.Y,
        Width = i.Width,
        Height = i.Height,
        Rotation = i.Rotation,
        ZIndex = i.ZIndex,
        CreatedAt = i.CreatedAt
    };
}
