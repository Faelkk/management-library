
namespace LibraryManagement.Services;

public interface IPasswordService
{
    Task<UserResponseMessageDto> ProcessPasswordRecovery(UserRecoveryPasswordDto recoveryDto);
    Task<UserResponseMessageDto> ProcessPasswordReset(UserResetPasswordDto resetDto, string token);
}