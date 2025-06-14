namespace LibraryManagement.Controllers;

using LibraryManagement.Dto;
using LibraryManagement.Services;
using LibraryManagement.UserRepository;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("user")]


public class UserController : Controller
{

    private readonly IUserRepository userRepository;
    private readonly IEmailService emailService;

    private readonly IPasswordService passwordService;
    private readonly TokenGenerator tokenGenerator;
    public UserController(IUserRepository userRepository, IEmailService emailService, TokenGenerator tokenGenerator, IPasswordService passwordService)
    {
        this.userRepository = userRepository;
        this.emailService = emailService;
        this.tokenGenerator = tokenGenerator;
        this.passwordService = passwordService;
    }


    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            var users = userRepository.GetAll();
            return Ok(users);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message.ToString() });
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetOne(int id)
    {
        try
        {
            var users = userRepository.GetById(id);
            return Ok(users);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message.ToString() });
        }
    }

    [HttpPost("/create")]
    public IActionResult Create([FromBody] UserInsertDto userInsertDto)
    {
        try
        {
            var userCreated = userRepository.Create(userInsertDto);


            var messageText = "<h4>Cadastro realizado em LibraryManagement </h4>";
            messageText += "<p> Olá: " + userCreated.Name;
            messageText += "<p> Boas vindas a LibraryManagement</p>";

            Message message = new Message
            {
                Title = "LibraryManagement - Cadastro realizado",
                Text = messageText,
                MailTo = userCreated.Email
            };
            emailService.Send(message);


            var token = tokenGenerator.Generate(userCreated);

            return Created("", new { token });

        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message.ToString() });
        }
    }

    [HttpPost("/login")]
    public IActionResult Login([FromBody] UserLoginDto userLoginDto)
    {
        try
        {
            var userLogged = userRepository.Login(userLoginDto);

            var userAgent = HttpContext.Request.Headers.UserAgent;
            var dateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            var messageText = "<h4>Novo login realizado em LibraryManagement</h4>";
            messageText += $"<p>Origem: {userAgent}<br />Data: {dateTime}</p>";
            messageText += "<p>Caso não reconheça este login, revise seus dados de autenticação.</p>";

            Message message = new Message
            {
                Title = "LibraryManagement - Novo login",
                Text = messageText,
                MailTo = userLogged.Email
            };

            emailService.Send(message);

            var token = tokenGenerator.Generate(userLogged);

            return Ok(new { token });

        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpPatch("recover-password")]
    public async Task<IActionResult> RecoverPassword([FromBody] UserRecoveryPasswordDto userData)
    {
        try
        {
            var response = await passwordService.ProcessPasswordRecovery(userData);
            return Ok(response);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message.ToString() });
        }
    }

    [HttpPatch("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordDto userData, [FromQuery] string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest(new { message = "Token de redefinição de senha é obrigatório." });
        }

        try
        {
            var response = await passwordService.ProcessPasswordReset(userData, token);
            return Ok(response);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message.ToString() });
        }
    }
}
