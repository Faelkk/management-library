namespace LibraryManagement.Controllers;


using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("loan")]
public class LoanController : Controller
{
    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            return Ok(new { Loans = "testando" });
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
            return Ok(new { Loan = $"Testando ID {id}" });
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpPost]
    public IActionResult Post()
    {
        try
        {
            return Ok(new { Loans = "testando" });
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
            return Ok(new { Loan = $"Atualizado ID {id}" });
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            return Ok(new { Loan = $"Deletado ID {id}" });
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }
}
