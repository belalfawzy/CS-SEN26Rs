using sha_SEN26Rs.Models;

namespace sha_SEN26Rs.Repositories;

public interface IMessageRepository
{
    Task<Message?> GetByIdAsync(Guid id);
    Task<List<Message>> GetReceivedMessagesAsync(Guid userId);
    Task<List<Message>> GetSentMessagesAsync(Guid userId);
    Task<Message> CreateAsync(Message message);
    Task<Message> UpdateAsync(Message message);
    Task DeleteAsync(Message message);
}
