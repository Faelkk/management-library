using LibraryManagement.Dto;

namespace LibraryManagement.Services;

public interface IBookService
{
    IEnumerable<BookResponseDto> GetAll();
    BookResponseDto GetById(int id);
    BookResponseDto Create(BookInsertDto bookInsertDto);
    BookResponseDto Update(int id, BookUpdateDto bookUpdateDto);
    Task Remove(int id);
}