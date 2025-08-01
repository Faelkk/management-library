using System.Threading.Tasks;
using LibraryManagement.Dto;
using LibraryManagement.Models;
using LibraryManagement.UserRepository;

namespace LibraryManagement.Services;

public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    private readonly IEmailService emailService;
    private readonly ITokenGenerator tokenGenerator;
    private readonly IPasswordService passwordService;

    public UserService(
        IUserRepository userRepository,
        IEmailService emailService,
        ITokenGenerator tokenGenerator,
        IPasswordService passwordService
    )
    {
        this.userRepository = userRepository;
        this.emailService = emailService;
        this.tokenGenerator = tokenGenerator;
        this.passwordService = passwordService;
    }

    public IEnumerable<UserResponseDto> GetAll()
    {
        var users = userRepository.GetAll();

        return users.Select(u => new UserResponseDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role,
            PhoneNumber = u.PhoneNumber,
            Loans = u.Loans,
        });
    }

    public async Task<UserResponseDto> GetById(int id)
    {
        var user = await userRepository.GetById(id);

        if (user == null)
            throw new Exception("Usuário não encontrado.");

        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Loans = user.Loans,
            PhoneNumber = user.PhoneNumber,
        };
    }

    public UserResponseDto Create(UserInsertDto userInsertDto)
    {
        var userCreated = userRepository.Create(userInsertDto);

        var messageText =
            $"<h4>Cadastro realizado em LibraryManagement</h4>"
            + $"<p>Olá: {userCreated.Name}</p>"
            + $"<p>Boas vindas a LibraryManagement</p>";

        var message = new Message
        {
            Title = "LibraryManagement - Cadastro realizado",
            Text = messageText,
            MailTo = userCreated.Email,
        };
        emailService.Send(message);

        return new UserResponseDto
        {
            Email = userCreated.Email,
            Id = userCreated.Id,
            Loans = userCreated.Loans,
            Name = userCreated.Name,
            PhoneNumber = userCreated.PhoneNumber,
            Role = userCreated.Role,
        };
    }

    public UserResponseTokenDto Login(UserLoginDto userLoginDto, string userAgent)
    {
        var userLogged = userRepository.Login(userLoginDto);

        var dateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        var messageText =
            $"<h4>Novo login realizado em LibraryManagement</h4>"
            + $"<p>Origem: {userAgent}<br />Data: {dateTime}</p>"
            + $"<p>Caso não reconheça este login, revise seus dados de autenticação.</p>";

        var message = new Message
        {
            Title = "LibraryManagement - Novo login",
            Text = messageText,
            MailTo = userLogged.Email,
        };

        emailService.Send(message);

        var token = tokenGenerator.Generate(userLogged);

        return new UserResponseTokenDto { Token = token };
    }

    public async Task<UserResponseMessageDto> RecoverPassword(
        UserRecoveryPasswordDto userRecoveryDto
    )
    {
        var response = await passwordService.ProcessPasswordRecovery(userRecoveryDto);
        return response;
    }

    public async Task<UserResponseMessageDto> ResetPassword(
        UserResetPasswordDto userResetDto,
        string token
    )
    {
        var response = await passwordService.ProcessPasswordReset(userResetDto, token);
        return response;
    }

    public async Task<UserResponseDto> EditUser(int id, UserEditDto userData)
    {
        var user = await userRepository.GetEntityById(id);
        if (user == null)
            throw new Exception("Usuário não encontrado.");

        if (userData.Email != null)
        {
            var existsWithEmail = await userRepository.ExistsWithEmail(userData.Email, id);
            if (existsWithEmail)
                throw new Exception("Email já está em uso por outro usuário.");

            user.Email = userData.Email;
        }

        if (userData.PhoneNumber != null)
        {
            var existsWithPhone = await userRepository.ExistsWithPhoneNumber(
                userData.PhoneNumber,
                id
            );
            if (existsWithPhone)
                throw new Exception("Telefone já está em uso por outro usuário.");

            user.PhoneNumber = userData.PhoneNumber;
        }

        if (userData.Name != null)
            user.Name = userData.Name;

        if (userData.Role != null)
            user.Role = userData.Role;

        await userRepository.Update(user);

        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            PhoneNumber = user.PhoneNumber,
            Loans = user
                .Loans.Select(loan => new LoanResponseDto
                {
                    Id = loan.Id,
                    BookId = loan.BookId,
                    ClientId = loan.ClientId,
                    LoanDate = loan.LoanDate,
                    ReturnDate = loan.ReturnDate,
                    ReturnedAt = loan.ReturnedAt,
                })
                .ToList(),
        };
    }

    public async Task Remove(int bookId)
    {
        await userRepository.Remove(bookId);
    }
}
