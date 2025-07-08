using LibraryManagement.Dto;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers;

[ApiController]
[Route("genre")]
public class GenreController : ControllerBase
{
    private readonly IGenreService genreService;

    public GenreController(IGenreService genreService)
    {
        this.genreService = genreService;
    }

    [Authorize(Policy = "Authenticated")]
    [HttpGet]
    public IActionResult GetAll()
    {
        var genres = genreService.GetAll();
        return Ok(genres);
    }

    [Authorize(Policy = "Authenticated")]
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        try
        {
            var genre = genreService.GetById(id);
            return Ok(genre);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "Authenticated")]
    [Authorize(Policy = "Admin")]
    [HttpPost]
    public IActionResult Create([FromBody] GenreInsertDto genreInsertDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var created = genreService.Create(genreInsertDto);
            return Created("", new { created });
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "Authenticated")]
    [Authorize(Policy = "Admin")]
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] GenreUpdateDto genreUpdateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var updated = genreService.Update(id, genreUpdateDto);
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
    public IActionResult Delete(int id)
    {
        genreService.Delete(id);
        return NoContent();
    }
}
