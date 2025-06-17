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
    private readonly IUploadFileService _uploadFileService;

    public BookController(IBookService bookService, IUploadFileService uploadFileService)
    {
        this.bookService = bookService;
        this._uploadFileService = uploadFileService;
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

        var imageUrl = await _uploadFileService.UploadFileAsync(request.ImageFile);

        var bookInsertDto = new BookInsertDto
        {
            Title = request.Title,
            Author = request.Author,
            PublishYear = request.PublishYear,
            Description = request.Description,
            Quantity = request.Quantity,
            ImageUrl = imageUrl
        };

        var response = bookService.Create(bookInsertDto);
        return Created("", response);
    }



    [Authorize(Policy = "Authenticated")]
    [Authorize(Policy = "Admin")]
    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, [FromForm] BookUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? imageUrl = null;

        if (request.ImageFile != null)
        {
            imageUrl = await _uploadFileService.UploadFileAsync(request.ImageFile);
        }

        var bookUpdateDto = new BookUpdateDto
        {
            Title = request.Title,
            Author = request.Author,
            PublishYear = request.PublishYear,
            Description = request.Description,
            Quantity = request.Quantity,
            ImageUrl = imageUrl
        };

        var updatedBook = bookService.Update(id, bookUpdateDto);

        return Ok(updatedBook);
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
