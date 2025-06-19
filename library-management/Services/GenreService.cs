using LibraryManagement.Dto;
using LibraryManagement.Models;
using LibraryManagement.Repository;

namespace LibraryManagement.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        this.genreRepository = genreRepository;
    }

    public IEnumerable<GenreResponseDto> GetAll()
    {
        return genreRepository.GetAll()
            .Select(g => new GenreResponseDto { Id = g.Id, Name = g.Name });
    }

    public GenreResponseDto GetById(int id)
    {
        var genre = genreRepository.GetById(id);
        if (genre == null)
            throw new Exception("Genre not found");

        return new GenreResponseDto { Id = genre.Id, Name = genre.Name };
    }

    public GenreResponseDto Create(GenreInsertDto genreInsertDto)
    {
        var created = genreRepository.Create(genreInsertDto);

        return new GenreResponseDto { Id = created.Id, Name = created.Name };
    }

    public GenreResponseDto Update(int id, GenreUpdateDto genre)
    {
        var updated = genreRepository.Update(id, genre);
        if (updated == null)
            throw new Exception("Genre not found");

        return new GenreResponseDto { Id = updated.Id, Name = updated.Name };
    }

    public async Task<bool> Delete(int id)
    {
        return await genreRepository.Delete(id);
    }

}
