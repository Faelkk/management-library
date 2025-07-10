using System.Threading.Tasks;
using LibraryManagement.Dto;
using LibraryManagement.Models;
using LibraryManagement.Repository;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Http;
using Moq;

public class BookServiceTest
{
    private readonly Mock<IBookRepository> bookRepositoryMock;
    private readonly Mock<IGenreRepository> genreRepositoryMock;
    private readonly Mock<IUploadFileService> uploadFileServiceMock;
    private readonly IBookService bookService;

    public BookServiceTest()
    {
        bookRepositoryMock = new Mock<IBookRepository>();
        uploadFileServiceMock = new Mock<IUploadFileService>();
        genreRepositoryMock = new Mock<IGenreRepository>();

        bookService = new BookService(
            bookRepositoryMock.Object,
            uploadFileServiceMock.Object,
            genreRepositoryMock.Object
        );
    }

    [Fact]
    public void Should_return_all_books_()
    {
        var books = new List<BookResponseDto>
        {
            new BookResponseDto
            {
                Id = 1,
                Title = "Book One",
                Author = "Author A",
                Quantity = 5,
                Description = "Description One",
                ImageUrl = "image1.jpg",
                PublishYear = 2020,
                Genres = new List<GenreResponseDto>(),
            },
            new BookResponseDto
            {
                Id = 2,
                Title = "Book Two",
                Author = "Author B",
                Quantity = 0,
                Description = "Description Two",
                ImageUrl = "image2.jpg",
                PublishYear = 2021,
                Genres = new List<GenreResponseDto>(),
            },
        };

        bookRepositoryMock.Setup(repo => repo.GetAll()).Returns(books);

        var result = bookService.GetAll();

        Assert.Collection(
            result,
            book => Assert.Equal(1, book.Id),
            book => Assert.Equal(2, book.Id)
        );
    }

    [Fact]
    public void Should_return_book_by_id()
    {
        var book = new BookResponseDto
        {
            Id = 1,
            Title = "Book One",
            Author = "Author A",
            Quantity = 5,
            Description = "Description One",
            ImageUrl = "image1.jpg",
            PublishYear = 2020,
            Genres = new List<GenreResponseDto>(),
        };

        bookRepositoryMock.Setup(repo => repo.GetById(1)).Returns(book);
        var result = bookService.GetById(1);

        Assert.Equal(1, result.Id);
        Assert.Equal("Book One", result.Title);
        Assert.Equal("Author A", result.Author);
        Assert.Equal(5, result.Quantity);
        Assert.Equal("Description One", result.Description);
        Assert.Equal("image1.jpg", result.ImageUrl);
        Assert.Equal(2020, result.PublishYear);
        Assert.Empty(result.Genres);
    }

    [Fact]
    public async Task Create_ValidRequest_ReturnsBookResponseDto()
    {
        var request = new BookCreateRequest
        {
            Title = "Test Book",
            Author = "Test Author",
            PublishYear = 2020,
            Description = "Test Description",
            Quantity = 10,
            GenreIds = new List<int> { 1 },
            ImageFile = new FormFile(new MemoryStream(), 0, 0, "Data", "test.png"),
        };

        // Mock do gênero
        genreRepositoryMock
            .Setup(g => g.GetById(1))
            .Returns(new GenreResponseDto { Id = 1, Name = "Fiction" });

        // Mock do upload da imagem
        uploadFileServiceMock
            .Setup(u => u.UploadFileAsync(It.IsAny<IFormFile>()))
            .ReturnsAsync("https://fakeurl.com/image.png");

        // Mock do repository Create
        bookRepositoryMock
            .Setup(b => b.Create(It.IsAny<BookInsertDto>()))
            .Returns(
                new BookResponseDto
                {
                    Id = 1,
                    Title = "Test Book",
                    Author = "Test Author",
                    PublishYear = 2020,
                    Description = "Test Description",
                    Quantity = 10,
                    ImageUrl = "https://fakeurl.com/image.png",
                    Genres = new List<GenreResponseDto>
                    {
                        new GenreResponseDto { Id = 1, Name = "Fiction" },
                    },
                    Loans = new List<LoanResponseDto>(),
                }
            );

        var result = await bookService.Create(request);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Book", result.Title);
        Assert.Equal("Test Author", result.Author);
        Assert.Equal(2020, result.PublishYear);
        Assert.Equal("Test Description", result.Description);
        Assert.Equal(10, result.Quantity);
        Assert.Equal("https://fakeurl.com/image.png", result.ImageUrl);
        Assert.Single(result.Genres);
        Assert.Equal(1, result.Genres.First().Id);
        Assert.Equal("Fiction", result.Genres.First().Name);
    }

    [Fact]
    public async Task Update_ExistingBook_ReturnsUpdatedBookResponseDto()
    {
        var bookUpdateRequest = new BookUpdateRequest { Title = "Updated Book" };

        bookRepositoryMock
            .Setup(repo => repo.Update(1, It.IsAny<BookUpdateDto>()))
            .Returns(
                new BookResponseDto
                {
                    Id = 1,
                    Title = "Updated Book",
                    Author = "Author A",
                    Quantity = 5,
                    Description = "Description One",
                    ImageUrl = "image1.jpg",
                    PublishYear = 2020,
                    Genres = new List<GenreResponseDto>(),
                }
            );

        var result = await bookService.Update(1, bookUpdateRequest);

        Assert.Equal(1, result.Id);
        Assert.Equal("Updated Book", result.Title);
    }

    [Fact]
    public async Task Remove_ExistingBook_ReturnsTrue()
    {
        var book = new BookResponseDto
        {
            Id = 1,
            Title = "Book One",
            Author = "Author A",
            Quantity = 5,
            Description = "Description One",
            ImageUrl = "image1.jpg",
            PublishYear = 2020,
            Genres = new List<GenreResponseDto>(),
            Loans = new List<LoanResponseDto>(),
        };

        bookRepositoryMock.Setup(r => r.GetById(1)).Returns(book);
        uploadFileServiceMock
            .Setup(u => u.DeleteFileAsync("image1.jpg"))
            .Returns(Task.CompletedTask);
        bookRepositoryMock.Setup(r => r.Remove(1)).ReturnsAsync(true);

        var result = await bookService.Remove(1);

        Assert.True(result);
    }
}
