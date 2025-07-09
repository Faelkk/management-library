using LibraryManagement.Dto;

namespace LibraryManagement.Services;

public interface IUserService
{
    IEnumerable<UserResponseDto> GetAll();
    Task<UserResponseDto> GetById(int id);
    UserResponseDto Create(UserInsertDto userInsertDto);
    UserResponseTokenDto Login(UserLoginDto userLoginDto, string userAgent);
    Task<UserResponseMessageDto> RecoverPassword(UserRecoveryPasswordDto userRecoveryDto);
    Task<UserResponseMessageDto> ResetPassword(UserResetPasswordDto userResetDto, string token);

    Task<UserResponseDto> EditUser(int id, UserEditDto userData);

    Task Remove(int bookId);
}
