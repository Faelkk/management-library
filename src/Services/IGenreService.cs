using LibraryManagement.Dto;

namespace LibraryManagement.Services;

public interface IGenreService
{
    IEnumerable<GenreResponseDto> GetAll();
    GenreResponseDto GetById(int id);
    GenreResponseDto Create(GenreInsertDto genreInsertDto);
    GenreResponseDto Update(int id, GenreUpdateDto genre);
    void Delete(int id);
}
