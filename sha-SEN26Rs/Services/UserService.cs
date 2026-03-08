using sha_SEN26Rs.DTOs;
using sha_SEN26Rs.Repositories;

namespace sha_SEN26Rs.Services;

public interface IUserService
{
    Task<List<UserResponseDto>> GetAllUsersAsync();
    Task<UserResponseDto> GetUserByUsernameAsync(string username);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(u => new UserResponseDto
        {
            Id = u.Id,
            Name = u.Name,
            Username = u.Username,
            Email = u.Email,
            CreatedAt = u.CreatedAt
        }).ToList();
    }

    public async Task<UserResponseDto> GetUserByUsernameAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username)
            ?? throw new KeyNotFoundException("User not found.");

        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Username = user.Username,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }
}
