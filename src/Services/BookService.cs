using LibraryManagement.Dto;
using LibraryManagement.Repository;

namespace LibraryManagement.Services;



public class BookService : IBookService
{

    private readonly IBookRepository bookRepository;

    public BookService(
        IBookRepository bookRepository)
    {
        this.bookRepository = bookRepository;
    }


    public IEnumerable<BookResponseDto> GetAll()
    {
        var books = bookRepository.GetAll();

        return books.Select(b => new BookResponseDto
        {
            Id = b.Id,
            Author = b.Author,
            Available = b.Available,
            Description = b.Description,
            Quantity = b.Quantity,
            ImageUrl = b.ImageUrl,
            Loans = b.Loans,
            PublishYear = b.PublishYear,
            Title = b.Title
        });
    }

    public BookResponseDto GetById(int id)
    {
        var book = bookRepository.GetById(id);

        if (book == null)
            throw new Exception("Book not found");

        return new BookResponseDto
        {
            Id = book.Id,
            Author = book.Author,
            Available = book.Available,
            Quantity = book.Quantity,
            Description = book.Description,
            ImageUrl = book.ImageUrl,
            Loans = book.Loans,
            PublishYear = book.PublishYear,
            Title = book.Title
        };
    }

    public BookResponseDto Create(BookInsertDto bookInsertDto)
    {
        var newBook = bookRepository.Create(bookInsertDto);

        return new BookResponseDto
        {
            Id = newBook.Id,
            Author = newBook.Author,
            Available = newBook.Available,
            Description = newBook.Description,
            ImageUrl = newBook.ImageUrl,
            Loans = newBook.Loans,
            PublishYear = newBook.PublishYear,
            Title = newBook.Title,
            Quantity = newBook.Quantity

        };
    }


    public BookResponseDto Update(int id, BookUpdateDto bookUpdateDto)
    {
        var updatedBook = bookRepository.Update(id, bookUpdateDto);

        if (updatedBook == null)
            throw new Exception("Book not found for update");

        return new BookResponseDto
        {
            Id = updatedBook.Id,
            Author = updatedBook.Author,
            Available = updatedBook.Available,
            Description = updatedBook.Description,
            ImageUrl = updatedBook.ImageUrl,
            Loans = updatedBook.Loans,
            PublishYear = updatedBook.PublishYear,
            Title = updatedBook.Title,
            Quantity = updatedBook.Quantity

        };
    }


    public async Task Remove(int id)
    {
        var book = bookRepository.GetById(id);

        if (book == null)
            throw new Exception("Book not found");

        await bookRepository.Remove(id);
    }


}