using LibraryManagement.Dto;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers;


[ApiController]
[Route("user")]


public class UserController : Controller
{
    private readonly IUserService userService;

    public UserController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            var users = userService.GetAll();
            return Ok(users);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetOne(int id)
    {
        try
        {
            var user = userService.GetById(id);
            return Ok(user);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }


    [HttpPost("/create")]
    public IActionResult Create([FromBody] UserInsertDto userInsertDto)
    {

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var response = userService.Create(userInsertDto);
            return Created("", response);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpPost("/login")]

    public IActionResult Login([FromBody] UserLoginDto userLoginDto)
    {

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var userAgent = HttpContext.Request.Headers.UserAgent.ToString();
            var response = userService.Login(userLoginDto, userAgent);
            return Ok(response);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpPatch("recover-password")]
    public async Task<IActionResult> RecoverPassword([FromBody] UserRecoveryPasswordDto userData)
    {

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var response = await userService.RecoverPassword(userData);
            return Ok(response);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpPatch("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordDto userData, [FromQuery] string token)
    {

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrEmpty(token))
            return BadRequest(new { message = "Token de redefinição de senha é obrigatório." });

        try
        {
            var response = await userService.ResetPassword(userData, token);
            return Ok(response);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        try
        {
            await userService.Remove(id);
            return NoContent();
        }
        catch (Exception Err)
        {
            return BadRequest(new { message = Err.Message });
        }
    }
}
