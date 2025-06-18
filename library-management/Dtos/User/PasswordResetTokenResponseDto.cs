
namespace LibraryManagement.Dto;

public class PasswordResetTokenResponseDto
{

    public int Id { get; set; }
    public string Token { get; set; }

    public DateTime ExpirationDate { get; set; }

    public int UserId { get; set; }

    public UserDto User { get; set; }
}
