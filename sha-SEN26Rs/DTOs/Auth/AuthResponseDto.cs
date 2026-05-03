using sha_SEN26Rs.DTOs.Students;

namespace sha_SEN26Rs.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public StudentResponseDto Student { get; set; } = null!;
}
