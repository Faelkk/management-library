
using LibraryManagement.Dto;
using LibraryManagement.Models;

namespace LibraryManagement.UserRepository;


public interface IUserRepository
{
    IEnumerable<UserResponseDto> GetAll();
    UserResponseDto GetById(int userId);
    UserResponseDto Create(UserInsertDto userData);
    UserResponseDto Login(UserLoginDto userLoginData);
    Task Remove(int userId);

    // Aqui idealmente você pode criar DTOs também para o token e usuário
    Task<PasswordResetTokenResponseDto> CreatePasswordResetToken(UserDto user);
    Task<PasswordResetTokenResponseDto> GetPasswordResetToken(string token);
    Task RemovePasswordResetToken(PasswordResetTokenResponseDto token);
    Task UpdateUserPassword(UserDto user, string newPassword);
    Task<UserDto> GetUserByEmail(string email);
}
