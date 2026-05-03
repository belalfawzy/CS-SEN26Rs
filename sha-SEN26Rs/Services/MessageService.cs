using sha_SEN26Rs.DTOs.Messages;
using sha_SEN26Rs.Models;
using sha_SEN26Rs.Repositories;

namespace sha_SEN26Rs.Services;

public interface IMessageService
{
    Task<MessageResponseDto> SendAsync(SendMessageDto dto, Guid senderId);
    Task<List<MessageResponseDto>> GetReceivedAsync(Guid studentId);
    Task<List<MessageResponseDto>> GetSentAsync(Guid studentId);
    Task<MessageResponseDto> UpdateAsync(Guid messageId, UpdateMessageDto dto, Guid studentId);
    Task DeleteAsync(Guid messageId, Guid studentId);
}

public class MessageService(IMessageRepository messageRepo, IStudentRepository studentRepo) : IMessageService
{
    public async Task<MessageResponseDto> SendAsync(SendMessageDto dto, Guid senderId)
    {
        if (dto.ReceiverId == senderId)
            throw new InvalidOperationException("You cannot send a message to yourself.");

        var receiver = await studentRepo.GetByIdAsync(dto.ReceiverId)
            ?? throw new KeyNotFoundException("Receiver not found.");

        var message = new Message
        {
            Content = dto.Content,
            SenderId = dto.IsAnonymous ? null : senderId,
            ReceiverId = dto.ReceiverId,
            IsAnonymous = dto.IsAnonymous
        };

        await messageRepo.CreateAsync(message);

        var created = await messageRepo.GetByIdAsync(message.Id);
        return MapToDto(created!);
    }

    public async Task<List<MessageResponseDto>> GetReceivedAsync(Guid studentId)
    {
        var messages = await messageRepo.GetReceivedMessagesAsync(studentId);
        return messages.Select(MapToDto).ToList();
    }

    public async Task<List<MessageResponseDto>> GetSentAsync(Guid studentId)
    {
        var messages = await messageRepo.GetSentMessagesAsync(studentId);
        return messages.Select(MapToDto).ToList();
    }

    public async Task<MessageResponseDto> UpdateAsync(Guid messageId, UpdateMessageDto dto, Guid studentId)
    {
        var message = await messageRepo.GetByIdAsync(messageId)
            ?? throw new KeyNotFoundException("Message not found.");

        if (message.SenderId != studentId)
            throw new UnauthorizedAccessException("You can only edit your own messages.");

        if (message.IsAnonymous)
            throw new InvalidOperationException("Anonymous messages cannot be edited.");

        message.Content = dto.Content;
        message.UpdatedAt = DateTime.UtcNow;

        await messageRepo.UpdateAsync(message);
        return MapToDto(message);
    }

    public async Task DeleteAsync(Guid messageId, Guid studentId)
    {
        var message = await messageRepo.GetByIdAsync(messageId)
            ?? throw new KeyNotFoundException("Message not found.");

        if (message.SenderId != studentId)
            throw new UnauthorizedAccessException("You can only delete your own messages.");

        if (message.IsAnonymous)
            throw new InvalidOperationException("Anonymous messages cannot be deleted.");

        await messageRepo.DeleteAsync(message);
    }

    private static MessageResponseDto MapToDto(Message m) => new()
    {
        Id = m.Id,
        Content = m.Content,
        SenderName = m.IsAnonymous ? null : m.Sender?.FullName,
        SenderUsername = m.IsAnonymous ? null : m.Sender?.Username,
        SenderAvatarUrl = m.IsAnonymous ? null : m.Sender?.AvatarUrl,
        ReceiverId = m.ReceiverId,
        ReceiverName = m.Receiver?.FullName ?? string.Empty,
        IsAnonymous = m.IsAnonymous,
        CreatedAt = m.CreatedAt,
        UpdatedAt = m.UpdatedAt
    };
}
