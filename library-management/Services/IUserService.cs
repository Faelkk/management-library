
using LibraryManagement.Dto;

namespace LibraryManagement.Services;

public interface IUserService
{

    IEnumerable<UserResponseDto> GetAll();
    UserResponseDto GetById(int id);
    UserResponseTokenDto Create(UserInsertDto userInsertDto);
    UserResponseTokenDto Login(UserLoginDto userLoginDto, string userAgent);
    Task<UserResponseMessageDto> RecoverPassword(UserRecoveryPasswordDto userRecoveryDto);
    Task<UserResponseMessageDto> ResetPassword(UserResetPasswordDto userResetDto, string token);
    Task Remove(int bookId);
}
