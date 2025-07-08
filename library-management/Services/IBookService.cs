using LibraryManagement.Dto;

namespace LibraryManagement.Services;

public interface IBookService
{
    IEnumerable<BookResponseDto> GetAll();
    BookResponseDto GetById(int id);
    Task<BookResponseDto> Create(BookCreateRequest request);
    Task<BookResponseDto> Update(int id, BookUpdateRequest request);
    Task<bool> Remove(int id);
}
