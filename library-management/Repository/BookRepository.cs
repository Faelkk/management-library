using LibraryManagement.Contexts;
using LibraryManagement.Dto;
using LibraryManagement.Models;
using LibraryManagement.Models.LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly IDatabaseContext databaseContext;

        public BookRepository(IDatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public IEnumerable<BookResponseDto> GetAll()
        {
            var books = databaseContext.Books
                .Include(b => b.Loans)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .ToList();

            return books.Select(b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                PublishYear = b.PublishYear,
                Description = b.Description,
                Available = b.Available,
                ImageUrl = b.ImageUrl,
                Quantity = b.Quantity,
                Genres = b.BookGenres.Select(bg => new GenreResponseDto
                {
                    Id = bg.Genre.Id,
                    Name = bg.Genre.Name
                }).ToList(),
                Loans = b.Loans.Select(loan => new LoanResponseDto
                {
                    Id = loan.Id,
                    BookId = loan.BookId,
                    UserId = loan.UserId,
                    LoanDate = loan.LoanDate,
                    ReturnDate = loan.ReturnDate,
                    ReturnAt = loan.ReturnAt
                }).ToList()
            });
        }

        public BookResponseDto GetById(int bookId)
        {
            var b = databaseContext.Books
                .Include(b => b.Loans)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .FirstOrDefault(b => b.Id == bookId);

            if (b == null) return null;

            return new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                PublishYear = b.PublishYear,
                Description = b.Description,
                Available = b.Available,
                ImageUrl = b.ImageUrl,
                Quantity = b.Quantity,
                Genres = b.BookGenres.Select(bg => new GenreResponseDto
                {
                    Id = bg.Genre.Id,
                    Name = bg.Genre.Name
                }).ToList(),
                Loans = b.Loans.Select(loan => new LoanResponseDto
                {
                    Id = loan.Id,
                    BookId = loan.BookId,
                    UserId = loan.UserId,
                    LoanDate = loan.LoanDate,
                    ReturnDate = loan.ReturnDate,
                    ReturnAt = loan.ReturnAt
                }).ToList()
            };
        }

        public BookResponseDto Create(BookInsertDto bookDto)
        {
            var book = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                PublishYear = bookDto.PublishYear,
                Description = bookDto.Description,
                Quantity = bookDto.Quantity,
                ImageUrl = bookDto.ImageUrl,
                BookGenres = bookDto.GenreIds.Select(id => new BookGenre
                {
                    GenreId = id
                }).ToList()
            };

            databaseContext.Books.Add(book);
            databaseContext.SaveChanges();

            return GetById(book.Id);
        }

        public BookResponseDto Update(int id, BookUpdateDto bookDto)
        {
            var book = databaseContext.Books
                .Include(b => b.BookGenres)
                .FirstOrDefault(b => b.Id == id);

            if (book == null)
            {

                throw new Exception("Book not found");
            }

            book.Title = bookDto.Title ?? book.Title;
            book.Author = bookDto.Author ?? book.Author;
            book.PublishYear = bookDto.PublishYear ?? book.PublishYear;
            book.Description = bookDto.Description ?? book.Description;
            book.ImageUrl = bookDto.ImageUrl ?? book.ImageUrl;

            if (bookDto.Quantity.HasValue)
                book.Quantity = bookDto.Quantity.Value;

            if (bookDto.GenreIds != null)
            {
                foreach (var genreId in bookDto.GenreIds)
                {
                    var alreadyExists = book.BookGenres.Any(bg => bg.GenreId == genreId);

                    if (!alreadyExists)
                    {
                        book.BookGenres.Add(new BookGenre
                        {
                            GenreId = genreId,
                            BookId = book.Id
                        });
                    }
                }
            }

            databaseContext.SaveChanges();

            return GetById(book.Id);
        }


        public async Task Remove(int bookId)
        {
            var book = databaseContext.Books
                .Include(b => b.BookGenres)
                .FirstOrDefault(b => b.Id == bookId);

            if (book == null)
            {
                throw new Exception("Book not found");
            }

            databaseContext.BookGenres.RemoveRange(book.BookGenres);
            databaseContext.Books.Remove(book);

            await databaseContext.SaveChangesAsync();
        }
    }
}
