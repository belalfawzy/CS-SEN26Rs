using sha_SEN26Rs.DTOs;
using sha_SEN26Rs.Models;
using sha_SEN26Rs.Repositories;

namespace sha_SEN26Rs.Services;

public interface IMessageService
{
    Task<MessageResponseDto> SendMessageAsync(SendMessageDto dto, Guid senderId);
    Task<List<MessageResponseDto>> GetReceivedMessagesAsync(Guid userId);
    Task<List<MessageResponseDto>> GetSentMessagesAsync(Guid userId);
    Task<MessageResponseDto> UpdateMessageAsync(Guid messageId, UpdateMessageDto dto, Guid userId);
    Task DeleteMessageAsync(Guid messageId, Guid userId);
}

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;

    public MessageService(IMessageRepository messageRepository, IUserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }

    public async Task<MessageResponseDto> SendMessageAsync(SendMessageDto dto, Guid senderId)
    {
        if (dto.ReceiverId == senderId)
            throw new InvalidOperationException("You cannot send a message to yourself.");

        var receiver = await _userRepository.GetByIdAsync(dto.ReceiverId)
            ?? throw new KeyNotFoundException("Receiver not found.");

        var message = new Message
        {
            Content = dto.Content,
            SenderId = dto.IsAnonymous ? null : senderId,
            ReceiverId = dto.ReceiverId,
            IsAnonymous = dto.IsAnonymous
        };

        await _messageRepository.CreateAsync(message);

        // Reload with includes
        var created = await _messageRepository.GetByIdAsync(message.Id);
        return MapToDto(created!);
    }

    public async Task<List<MessageResponseDto>> GetReceivedMessagesAsync(Guid userId)
    {
        var messages = await _messageRepository.GetReceivedMessagesAsync(userId);
        return messages.Select(MapToDto).ToList();
    }

    public async Task<List<MessageResponseDto>> GetSentMessagesAsync(Guid userId)
    {
        var messages = await _messageRepository.GetSentMessagesAsync(userId);
        return messages.Select(MapToDto).ToList();
    }

    public async Task<MessageResponseDto> UpdateMessageAsync(Guid messageId, UpdateMessageDto dto, Guid userId)
    {
        var message = await _messageRepository.GetByIdAsync(messageId)
            ?? throw new KeyNotFoundException("Message not found.");

        if (message.SenderId != userId)
            throw new UnauthorizedAccessException("You can only edit your own messages.");

        if (message.IsAnonymous)
            throw new InvalidOperationException("Anonymous messages cannot be edited.");

        message.Content = dto.Content;
        message.UpdatedAt = DateTime.UtcNow;

        await _messageRepository.UpdateAsync(message);
        return MapToDto(message);
    }

    public async Task DeleteMessageAsync(Guid messageId, Guid userId)
    {
        var message = await _messageRepository.GetByIdAsync(messageId)
            ?? throw new KeyNotFoundException("Message not found.");

        if (message.SenderId != userId)
            throw new UnauthorizedAccessException("You can only delete your own messages.");

        if (message.IsAnonymous)
            throw new InvalidOperationException("Anonymous messages cannot be deleted.");

        await _messageRepository.DeleteAsync(message);
    }

    private static MessageResponseDto MapToDto(Message message)
    {
        return new MessageResponseDto
        {
            Id = message.Id,
            Content = message.Content,
            SenderName = message.IsAnonymous ? "Anonymous" : (message.Sender?.Name ?? "Unknown"),
            ReceiverId = message.ReceiverId,
            ReceiverName = message.Receiver?.Name ?? "Unknown",
            IsAnonymous = message.IsAnonymous,
            CreatedAt = message.CreatedAt,
            UpdatedAt = message.UpdatedAt
        };
    }
}
