using LibraryManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("client")]
public class ClientController : ControllerBase
{
    private readonly IClientService clientService;

    public ClientController(IClientService clientService)
    {
        this.clientService = clientService;
    }

    [Authorize(Policy = "Authenticated")]
    [Authorize(Policy = "Admin")]
    [HttpGet]
    public ActionResult<IEnumerable<ClientResponseDto>> GetAll()
    {
        try
        {
            var clients = clientService.GetAll();
            return Ok(clients);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [Authorize(Policy = "Authenticated")]
    [Authorize(Policy = "Admin")]
    [HttpGet("{id}")]
    public ActionResult<ClientResponseDto> GetById(int id)
    {
        try
        {
            var client = clientService.GetById(id);
            return Ok(client);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [Authorize(Policy = "Authenticated")]
    [Authorize(Policy = "Admin")]
    [HttpPost]
    public IActionResult Create(ClientInsertDto clientInsertDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var response = clientService.Create(clientInsertDto);
            return Created("", response);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [Authorize(Policy = "Authenticated")]
    [Authorize(Policy = "Admin")]
    [HttpPatch("{id}")]
    public ActionResult<ClientResponseDto> Update(int id, ClientUpdateDto clientUpdateDto)
    {
        try
        {
            var updated = clientService.Update(id, clientUpdateDto);
            return Ok(updated);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "Authenticated")]
    [Authorize(Policy = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await clientService.Delete(id);
            return NoContent();
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }
}
