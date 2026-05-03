using Microsoft.EntityFrameworkCore;
using sha_SEN26Rs.Data;
using sha_SEN26Rs.Models;

namespace sha_SEN26Rs.Repositories;

public class MemoryBoardRepository(AppDbContext context) : IMemoryBoardRepository
{
    public async Task<MemoryBoard?> GetByIdAsync(Guid id) =>
        await context.MemoryBoards
            .Include(b => b.Owner)
            .Include(b => b.Items).ThenInclude(i => i.Author)
            .FirstOrDefaultAsync(b => b.Id == id);

    public async Task<MemoryBoard?> GetByStudentIdAsync(Guid studentId) =>
        await context.MemoryBoards
            .Include(b => b.Owner)
            .Include(b => b.Items).ThenInclude(i => i.Author)
            .FirstOrDefaultAsync(b => b.OwnerStudentId == studentId);

    public async Task<MemoryBoard> CreateAsync(MemoryBoard board)
    {
        board.Id = Guid.NewGuid();
        board.CreatedAt = DateTime.UtcNow;
        context.MemoryBoards.Add(board);
        await context.SaveChangesAsync();
        return board;
    }

    public async Task<MemoryBoard> UpdateAsync(MemoryBoard board)
    {
        context.MemoryBoards.Update(board);
        await context.SaveChangesAsync();
        return board;
    }

    public async Task<BoardItem> AddItemAsync(BoardItem item)
    {
        item.Id = Guid.NewGuid();
        item.CreatedAt = DateTime.UtcNow;
        context.BoardItems.Add(item);
        await context.SaveChangesAsync();
        return item;
    }

    public async Task<BoardItem?> GetItemByIdAsync(Guid itemId) =>
        await context.BoardItems
            .Include(i => i.Author)
            .FirstOrDefaultAsync(i => i.Id == itemId);

    public async Task<BoardItem> UpdateItemAsync(BoardItem item)
    {
        context.BoardItems.Update(item);
        await context.SaveChangesAsync();
        return item;
    }

    public async Task DeleteItemAsync(BoardItem item)
    {
        context.BoardItems.Remove(item);
        await context.SaveChangesAsync();
    }
}
