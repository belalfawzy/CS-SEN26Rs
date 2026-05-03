using Microsoft.EntityFrameworkCore;
using sha_SEN26Rs.Data;
using sha_SEN26Rs.Models;

namespace sha_SEN26Rs.Repositories;

public class MessageRepository(AppDbContext context) : IMessageRepository
{
    public async Task<Message?> GetByIdAsync(Guid id) =>
        await context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .FirstOrDefaultAsync(m => m.Id == id);

    public async Task<List<Message>> GetReceivedMessagesAsync(Guid studentId) =>
        await context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m => m.ReceiverId == studentId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

    public async Task<List<Message>> GetSentMessagesAsync(Guid studentId) =>
        await context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m => m.SenderId == studentId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

    public async Task<Message> CreateAsync(Message message)
    {
        context.Messages.Add(message);
        await context.SaveChangesAsync();
        return message;
    }

    public async Task<Message> UpdateAsync(Message message)
    {
        context.Messages.Update(message);
        await context.SaveChangesAsync();
        return message;
    }

    public async Task DeleteAsync(Message message)
    {
        context.Messages.Remove(message);
        await context.SaveChangesAsync();
    }
}
