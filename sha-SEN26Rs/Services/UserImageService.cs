using sha_SEN26Rs.Models;
using sha_SEN26Rs.Repositories;

namespace sha_SEN26Rs.Services;

public interface IUserImageService
{
    Task<UserImageDto> UploadAsync(Guid studentId, IFormFile file, string? caption, bool isPublic);
    Task<List<UserImageDto>> GetMyImagesAsync(Guid studentId);
    Task<List<CommunityImageDto>> GetCommunityImagesAsync();
    Task<List<CommunityImageDto>> GetPublicByStudentIdAsync(Guid studentId);
    Task DeleteAsync(Guid imageId, Guid studentId);
}

public record UserImageDto(
    Guid Id,
    string Url,
    string FileName,
    string? Caption,
    bool IsPublic,
    DateTime CreatedAt);

public record CommunityImageDto(
    Guid Id,
    string Url,
    string? Caption,
    DateTime CreatedAt,
    ImageUploaderDto Uploader);

public record ImageUploaderDto(
    Guid Id,
    string FullName,
    string Username,
    string? AvatarUrl);

public class UserImageService(
    IUserImageRepository imageRepo,
    ICloudinaryService cloudinary) : IUserImageService
{
    private static readonly string[] AllowedTypes = ["image/jpeg", "image/png", "image/webp", "image/gif"];
    private const long MaxBytes = 5 * 1024 * 1024;

    public async Task<UserImageDto> UploadAsync(Guid studentId, IFormFile file, string? caption, bool isPublic)
    {
        if (file.Length == 0)
            throw new InvalidOperationException("File is empty.");

        if (file.Length > MaxBytes)
            throw new InvalidOperationException("File exceeds 5MB limit.");

        if (!AllowedTypes.Contains(file.ContentType.ToLower()))
            throw new InvalidOperationException("Only JPEG, PNG, WebP, and GIF are allowed.");

        var (publicId, url) = await cloudinary.UploadAsync(file, $"sha-sen26rs/students/{studentId}");

        var image = await imageRepo.CreateAsync(new UserImage
        {
            StudentId = studentId,
            PublicId = publicId,
            Url = url,
            FileName = file.FileName,
            FileSize = (int)file.Length,
            MimeType = file.ContentType,
            Caption = caption,
            IsPublic = isPublic
        });

        return MapToDto(image);
    }

    public async Task<List<UserImageDto>> GetMyImagesAsync(Guid studentId)
    {
        var images = await imageRepo.GetByStudentIdAsync(studentId);
        return images.Select(MapToDto).ToList();
    }

    public async Task DeleteAsync(Guid imageId, Guid studentId)
    {
        var image = await imageRepo.GetByIdAsync(imageId)
            ?? throw new KeyNotFoundException("Image not found.");

        if (image.StudentId != studentId)
            throw new UnauthorizedAccessException("You can only delete your own images.");

        await cloudinary.DeleteAsync(image.PublicId);
        await imageRepo.DeleteAsync(image);
    }

    public async Task<List<CommunityImageDto>> GetCommunityImagesAsync()
    {
        var images = await imageRepo.GetPublicAllAsync();
        return images.Select(MapToCommunityDto).ToList();
    }

    public async Task<List<CommunityImageDto>> GetPublicByStudentIdAsync(Guid studentId)
    {
        var images = await imageRepo.GetPublicByStudentIdAsync(studentId);
        return images.Select(MapToCommunityDto).ToList();
    }

    private static UserImageDto MapToDto(UserImage i) =>
        new(i.Id, i.Url, i.FileName, i.Caption, i.IsPublic, i.CreatedAt);

    private static CommunityImageDto MapToCommunityDto(UserImage i) =>
        new(i.Id, i.Url, i.Caption, i.CreatedAt,
            new ImageUploaderDto(i.Student.Id, i.Student.FullName, i.Student.Username, i.Student.AvatarUrl));
}
