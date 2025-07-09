using LibraryManagement.Dto;
using LibraryManagement.Repository;

namespace LibraryManagement.Services;

public class BookService : IBookService
{
    private readonly IBookRepository bookRepository;
    private readonly IUploadFileService uploadFileService;
    private readonly IGenreRepository genreRepository;

    public BookService(
        IBookRepository bookRepository,
        IUploadFileService uploadFileService,
        IGenreRepository genreRepository
    )
    {
        this.bookRepository = bookRepository;
        this.uploadFileService = uploadFileService;
        this.genreRepository = genreRepository;
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
            Title = b.Title,
            Genres = b.Genres,
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
            Title = book.Title,
            Genres = book.Genres,
        };
    }

    public async Task<BookResponseDto> Create(BookCreateRequest request)
    {
        foreach (var genreId in request.GenreIds)
        {
            var genre =
                genreRepository.GetById(genreId)
                ?? throw new Exception($"Genre with id {genreId} does not exist.");
        }

        var imageUrl = await uploadFileService.UploadFileAsync(request.ImageFile);

        var bookInsertDto = new BookInsertDto
        {
            GenreIds = request.GenreIds,
            Title = request.Title,
            Author = request.Author,
            PublishYear = request.PublishYear,
            Description = request.Description,
            Quantity = request.Quantity,
            ImageUrl = imageUrl,
        };

        var newBook = bookRepository.Create(bookInsertDto);

        return new BookResponseDto
        {
            Genres = newBook.Genres,
            Id = newBook.Id,
            Author = newBook.Author,
            Available = newBook.Available,
            Description = newBook.Description,
            ImageUrl = newBook.ImageUrl,
            Loans = newBook.Loans,
            PublishYear = newBook.PublishYear,
            Title = newBook.Title,
            Quantity = newBook.Quantity,
        };
    }

    public async Task<BookResponseDto> Update(int id, BookUpdateRequest request)
    {
        if (request.GenreIds != null)
        {
            foreach (var genreId in request.GenreIds)
            {
                var genre =
                    genreRepository.GetById(genreId)
                    ?? throw new Exception($"Genre with id {genreId} does not exist.");
            }
        }

        string? imageUrl = null;
        if (request.ImageFile != null)
        {
            imageUrl = await uploadFileService.UploadFileAsync(request.ImageFile);
        }

        var bookUpdateDto = new BookUpdateDto
        {
            GenreIds = request.GenreIds,
            Title = request.Title,
            Author = request.Author,
            PublishYear = request.PublishYear,
            Description = request.Description,
            Quantity = request.Quantity,
            ImageUrl = imageUrl,
        };

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
            Genres = updatedBook.Genres,
            Loans = updatedBook.Loans,
            PublishYear = updatedBook.PublishYear,
            Title = updatedBook.Title,
            Quantity = updatedBook.Quantity,
        };
    }

    public async Task<bool> Remove(int id)
    {
        var book = bookRepository.GetById(id);
        if (book == null)
            throw new Exception("Book not found");

        if (!string.IsNullOrEmpty(book.ImageUrl))
        {
            var fileName = Path.GetFileName(book.ImageUrl);
            await uploadFileService.DeleteFileAsync(fileName);
        }

        return await bookRepository.Remove(id);
    }
}
