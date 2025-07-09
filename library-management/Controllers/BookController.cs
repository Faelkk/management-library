using LibraryManagement.Dto;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers;

[ApiController]
[Route("book")]
public class BookController : Controller
{
    private readonly IBookService bookService;

    public BookController(IBookService bookService)
    {
        this.bookService = bookService;
    }

    [Authorize(Policy = "Authenticated")]
    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            var books = bookService.GetAll();
            return Ok(books);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [Authorize(Policy = "Authenticated")]
    [HttpGet("{id}")]
    public IActionResult GetOne(int id)
    {
        try
        {
            var book = bookService.GetById(id);
            return Ok(book);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [Authorize(Policy = "Authenticated")]
    [Authorize(Policy = "Admin")]
    [DisableRequestSizeLimit]
    [HttpPost]
    public async Task<IActionResult> Post([FromForm] BookCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var response = await bookService.Create(request);
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
    public async Task<IActionResult> Patch(int id, [FromForm] BookUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updatedBook = await bookService.Update(id, request);
            return Ok(updatedBook);
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
            await bookService.Remove(id);
            return NoContent();
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }
}
