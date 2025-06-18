using LibraryManagement.Dto;

namespace LibraryManagement.Repository;

public interface IGenreRepository
{
    IEnumerable<GenreResponseDto> GetAll();
    GenreResponseDto? GetById(int id);
    GenreResponseDto Create(GenreInsertDto genre);
    GenreResponseDto? Update(int id, GenreUpdateDto genre);
    void Delete(int id);
}
