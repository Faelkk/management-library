namespace LibraryManagement.Controllers;

using LibraryManagement.Dto;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("loan")]
public class LoanController : Controller
{
    private readonly ILoanService loanService;

    public LoanController(ILoanService loanService)
    {
        this.loanService = loanService;
    }



    [Authorize(Policy = "Authenticated")]
    [Authorize(Policy = "Admin")]
    [HttpGet]
    public IActionResult GetAll([FromQuery] int? year, [FromQuery] int? month)
    {
        try
        {
            var loans = loanService.GetAll(year, month);
            return Ok(loans);
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
            var loans = loanService.GetById(id);
            return Ok(loans);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpPost]
    public IActionResult Post([FromBody] LoanInsertDto loanInsertDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var response = loanService.Create(loanInsertDto);
            return Created("", response);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpPatch("{id}")]
    public IActionResult Patch(int id)
    {
        try
        {
            var response = loanService.Update(id);
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
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await loanService.Remove(id);
            return NoContent();
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }

    }
}
