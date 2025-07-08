using LibraryManagement.Dto;
using LibraryManagement.UserRepository;

namespace LibraryManagement.Services;

public class PasswordService : IPasswordService
{
    private readonly IUserRepository userRepository;
    private readonly IEmailService emailService;

    public PasswordService(IUserRepository userRepository, IEmailService emailService)
    {
        this.userRepository = userRepository;
        this.emailService = emailService;
    }

    public async Task<UserResponseMessageDto> ProcessPasswordRecovery(
        UserRecoveryPasswordDto recoveryDto
    )
    {
        var user = await userRepository.GetUserByEmail(recoveryDto.email);
        if (user == null)
        {
            return new UserResponseMessageDto
            {
                Message = "If an account with that email exists, we've sent a password reset link.",
            };
        }
        var token = await userRepository.CreatePasswordResetToken(user);
        var resetLink = $"SEU_FRONTEND_URL/reset-password?token={token.Token}";

        string messageText =
            $"<h4>Recuperação de Senha</h4><p>Olá, {user.Name},</p><p>Você solicitou a recuperação da sua senha. Clique no link abaixo para redefinir sua senha:</p><p><a href='{resetLink}'>{resetLink}</a></p><p>Este link é válido por 24 horas.</p>";

        Message message = new Message
        {
            Title = "Recuperação de Senha",
            Text = messageText,
            MailTo = user.Email,
        };

        emailService.Send(message);

        return new UserResponseMessageDto
        {
            Message = "If an account with that email exists, we've sent a password reset link.",
        };
    }

    public async Task<UserResponseMessageDto> ProcessPasswordReset(
        UserResetPasswordDto resetDto,
        string token
    )
    {
        var resetToken = await userRepository.GetPasswordResetToken(token);
        if (resetToken == null)
        {
            return new UserResponseMessageDto { Message = "Invalid or expired reset token." };
        }

        var user = resetToken.User;
        await userRepository.UpdateUserPassword(user, resetDto.password);
        await userRepository.RemovePasswordResetToken(resetToken);

        return new UserResponseMessageDto { Message = "Password reset successfully." };
    }
}
