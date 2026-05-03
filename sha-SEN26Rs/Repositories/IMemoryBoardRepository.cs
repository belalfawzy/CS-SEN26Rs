using sha_SEN26Rs.Models;

namespace sha_SEN26Rs.Repositories;

public interface IMemoryBoardRepository
{
    Task<MemoryBoard?> GetByIdAsync(Guid id);
    Task<MemoryBoard?> GetByStudentIdAsync(Guid studentId);
    Task<MemoryBoard> CreateAsync(MemoryBoard board);
    Task<MemoryBoard> UpdateAsync(MemoryBoard board);
    Task<BoardItem> AddItemAsync(BoardItem item);
    Task<BoardItem?> GetItemByIdAsync(Guid itemId);
    Task<BoardItem> UpdateItemAsync(BoardItem item);
    Task DeleteItemAsync(BoardItem item);
}
