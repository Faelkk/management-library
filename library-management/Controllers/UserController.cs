using LibraryManagement.Dto;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Policy = "Authenticated")]
    [Authorize(Policy = "Admin")]
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

    [Authorize(Policy = "Authenticated")]
    [Authorize(Policy = "Admin")]
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

    [Authorize(Policy = "Authenticated")]
    [HttpGet("validate-token")]
    public IActionResult ValidateToken()
    {
        return Ok(new { isValid = true });
    }

    [Authorize(Policy = "Authenticated")]
    [Authorize(Policy = "Admin")]
    [HttpPost("create")]
    public IActionResult Create([FromBody] UserInsertDto userInsertDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var userCreated = userService.Create(userInsertDto);
            return Created("", userCreated);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpPost("login")]
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
    public async Task<IActionResult> ResetPassword(
        [FromBody] UserResetPasswordDto userData,
        [FromQuery] string token
    )
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

    [Authorize(Policy = "Authenticated")]
    [Authorize(Policy = "Admin")]
    [HttpPatch("{id}")]
    public async Task<IActionResult> EditUser(int id, [FromBody] UserEditDto userData)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var response = await userService.EditUser(id, userData);
            return Ok(response);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [Authorize(Policy = "Authenticated")]
    [Authorize(Policy = "Admin")]
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
