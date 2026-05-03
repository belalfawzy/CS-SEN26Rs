using Microsoft.EntityFrameworkCore;
using sha_SEN26Rs.Data;
using sha_SEN26Rs.Models;

namespace sha_SEN26Rs.Repositories;

public class UserImageRepository(AppDbContext context) : IUserImageRepository
{
    public async Task<List<UserImage>> GetByStudentIdAsync(Guid studentId) =>
        await context.UserImages
            .Where(i => i.StudentId == studentId)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();

    public async Task<UserImage?> GetByIdAsync(Guid id) =>
        await context.UserImages.FindAsync(id);

    public async Task<UserImage> CreateAsync(UserImage image)
    {
        image.Id = Guid.NewGuid();
        image.CreatedAt = DateTime.UtcNow;
        context.UserImages.Add(image);
        await context.SaveChangesAsync();
        return image;
    }

    public async Task DeleteAsync(UserImage image)
    {
        context.UserImages.Remove(image);
        await context.SaveChangesAsync();
    }
}
