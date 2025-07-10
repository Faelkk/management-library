using LibraryManagement.Dto;
using LibraryManagement.Models;

namespace LibraryManagement.UserRepository;

public interface IUserRepository
{
    IEnumerable<UserResponseDto> GetAll();
    Task<UserResponseDto?> GetById(int userId);

    Task<User> GetEntityById(int id);
    UserResponseDto Create(UserInsertDto userDto);
    UserResponseDto Login(UserLoginDto userLoginDto);

    Task Update(User user);
    Task Remove(int userId);

    Task<PasswordResetTokenResponseDto> CreatePasswordResetToken(UserDto user);
    Task<PasswordResetTokenResponseDto> GetPasswordResetToken(string token);
    Task RemovePasswordResetToken(PasswordResetTokenResponseDto token);
    Task UpdateUserPassword(UserDto user, string newPassword);
    Task<UserDto> GetUserByEmail(string email);

    Task<bool> ExistsWithEmail(string email, int excludeUserId);
    Task<bool> ExistsWithPhoneNumber(string phoneNumber, int excludeUserId);
}
