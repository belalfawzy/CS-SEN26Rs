using sha_SEN26Rs.Models;

namespace sha_SEN26Rs.Repositories;

public interface IUserImageRepository
{
    Task<List<UserImage>> GetByStudentIdAsync(Guid studentId);
    Task<List<UserImage>> GetPublicAllAsync();
    Task<List<UserImage>> GetPublicByStudentIdAsync(Guid studentId);
    Task<UserImage?> GetByIdAsync(Guid id);
    Task<UserImage> CreateAsync(UserImage image);
    Task DeleteAsync(UserImage image);
}
