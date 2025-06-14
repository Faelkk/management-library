
using LibraryManagement.Dto;

namespace LibraryManagement.Services;

public interface IUserService
{

    IEnumerable<UserResponseDto> GetAllUsers();
    UserResponseDto GetUserById(int id);
    UserResponseTokenDto CreateUser(UserInsertDto userInsertDto);
    UserResponseTokenDto LoginUser(UserLoginDto userLoginDto, string userAgent);
    Task<UserResponseMessageDto> RecoverPassword(UserRecoveryPasswordDto userRecoveryDto);
    Task<UserResponseMessageDto> ResetPassword(UserResetPasswordDto userResetDto, string token);
}
