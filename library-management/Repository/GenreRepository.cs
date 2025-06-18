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
        return context.Genres
            .Select(g => new GenreResponseDto
            {
                Id = g.Id,
                Name = g.Name
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
            Name = genre.Name
        };
    }

    public GenreResponseDto Create(GenreInsertDto genre)
    {
        var newGenre = new Genre
        {
            Name = genre.Name,
        };

        context.Genres.Add(newGenre);
        context.SaveChanges();

        return new GenreResponseDto
        {
            Id = newGenre.Id,
            Name = newGenre.Name
        };
    }

    public GenreResponseDto Update(int id, GenreUpdateDto genreUpdateDto)
    {
        var loan = context.Genres.FirstOrDefault(g => g.Id == id);
        if (loan == null)
        {
            throw new Exception("genre not found");
        }


        loan.Name = genreUpdateDto.Name;
        context.SaveChanges();

        return new GenreResponseDto
        {
            Id = loan.Id,
            Name = loan.Name
        };
    }

    public void Delete(int id)
    {
        var genre = context.Genres.FirstOrDefault(g => g.Id == id);


        if (genre == null)
        {
            throw new Exception("genre not found");
        }

        context.Genres.Remove(genre);
        context.SaveChanges();
    }
}
