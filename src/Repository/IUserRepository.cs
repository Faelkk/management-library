
using LibraryManagement.Dto;
using LibraryManagement.Models;

namespace LibraryManagement.UserRepository;


public interface IUserRepository
{
    IEnumerable<UserResponseDto> GetAll();
    UserResponseDto GetById(int userId);
    UserResponseDto Create(UserInsertDto userDto);
    UserResponseDto Login(UserLoginDto userLoginDto);
    Task Remove(int userId);

    Task<PasswordResetTokenResponseDto> CreatePasswordResetToken(UserDto user);
    Task<PasswordResetTokenResponseDto> GetPasswordResetToken(string token);
    Task RemovePasswordResetToken(PasswordResetTokenResponseDto token);
    Task UpdateUserPassword(UserDto user, string newPassword);
    Task<UserDto> GetUserByEmail(string email);
}
