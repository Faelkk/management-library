namespace LibraryManagement.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("book")]

public class BookController : Controller
{
    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            return Ok(new { books = "testando" });
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
            return Ok(new { book = $"testando {id}" });
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
            return Ok(new { book = "criado" });
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
            return Ok(new { book = $"atualizado {id}" });
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
            return Ok(new { book = $"deletado {id}" });
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }
}
