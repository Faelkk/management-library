using LibraryManagement.Contexts;
using LibraryManagement.Dto;
using LibraryManagement.Models;

namespace LibraryManagement.Repository;

public class GenreRepository : IGenreRepository
{
    private readonly IDatabaseContext context;

    public GenreRepository(IDatabaseContext context)
    {
        this.context = context;
    }

    public IEnumerable<GenreResponseDto> GetAll()
    {
        return context
            .Genres.Select(g => new GenreResponseDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
            })
            .ToList();
    }

    public GenreResponseDto? GetById(int id)
    {
        var genre = context.Genres.FirstOrDefault(g => g.Id == id);
        if (genre == null)
        {
            throw new Exception("genre not found");
        }

        return new GenreResponseDto
        {
            Id = genre.Id,
            Name = genre.Name,
            Description = genre.Description,
        };
    }

    public GenreResponseDto Create(GenreInsertDto genre)
    {
        var newGenre = new Genre { Name = genre.Name, Description = genre.Description };

        context.Genres.Add(newGenre);
        context.SaveChanges();

        return new GenreResponseDto
        {
            Id = newGenre.Id,
            Name = newGenre.Name,
            Description = newGenre.Description,
        };
    }

    public GenreResponseDto Update(int id, GenreUpdateDto genreUpdateDto)
    {
        var loan = context.Genres.FirstOrDefault(g => g.Id == id);
        if (loan == null)
        {
            throw new Exception("genre not found");
        }

        if (genreUpdateDto.Name != null)
            loan.Name = genreUpdateDto.Name;

        if (genreUpdateDto.Description != null)
            loan.Description = genreUpdateDto.Description;

        context.SaveChanges();

        return new GenreResponseDto
        {
            Id = loan.Id,
            Name = loan.Name,
            Description = loan.Description,
        };
    }

    public async Task<bool> Delete(int id)
    {
        var genre = context.Genres.FirstOrDefault(g => g.Id == id);

        if (genre == null)
        {
            throw new Exception("genre not found");
        }

        context.Genres.Remove(genre);
        await context.SaveChangesAsync();

        return true;
    }
}
