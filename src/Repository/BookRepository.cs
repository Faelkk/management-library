
using LibraryManagement.Contexts;
using LibraryManagement.Dto;
using LibraryManagement.Models;

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
            return databaseContext.Books.Select(b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                PublishYear = b.PublishYear,
                Description = b.Description,
                Available = b.Available,
                ImageUrl = b.ImageUrl,
                Quantity = b.Quantity,
            }).ToList();
        }

        public BookResponseDto GetById(int bookId)
        {
            var b = databaseContext.Books.FirstOrDefault(b => b.Id == bookId);

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
                ImageUrl = bookDto.ImageUrl
            };

            databaseContext.Books.Add(book);
            databaseContext.SaveChanges();

            return new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                PublishYear = book.PublishYear,
                Description = book.Description,
                Available = book.Available,
                ImageUrl = book.ImageUrl,
                Quantity = book.Quantity,
            };
        }

        public BookResponseDto Update(int id, BookUpdateDto bookDto)
        {
            var book = databaseContext.Books.FirstOrDefault(b => b.Id == id);

            if (book == null) return null;

            book.Title = bookDto.Title ?? book.Title;
            book.Author = bookDto.Author ?? book.Author;
            book.PublishYear = bookDto.PublishYear ?? book.PublishYear;
            book.Description = bookDto.Description ?? book.Description;
            book.ImageUrl = bookDto.ImageUrl ?? book.ImageUrl;

            if (bookDto.Quantity.HasValue)
            {
                book.Quantity = bookDto.Quantity.Value;
            }

            databaseContext.SaveChanges();

            return new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                PublishYear = book.PublishYear,
                Description = book.Description,
                Available = book.Available,
                ImageUrl = book.ImageUrl,
                Quantity = book.Quantity
            };
        }


        public async Task Remove(int bookId)
        {
            var book = databaseContext.Books.FirstOrDefault(b => b.Id == bookId);

            if (book == null)
                throw new Exception("Book not found");

            databaseContext.Books.Remove(book);
            await databaseContext.SaveChangesAsync();
        }
    }
}
