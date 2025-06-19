using LibraryManagement.Models;
using LibraryManagement.Dto;

namespace LibraryManagement.Repository
{
    public interface IBookRepository
    {
        IEnumerable<BookResponseDto> GetAll();
        BookResponseDto GetById(int bookId);
        BookResponseDto Create(BookInsertDto bookDto);
        BookResponseDto Update(int id, BookUpdateDto bookDto);
        Task<bool> Remove(int id);
    }
}
