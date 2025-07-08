using LibraryManagement.Dto;
using LibraryManagement.LoanRepository;
using LibraryManagement.Models;
using LibraryManagement.Repository;
using LibraryManagement.Services;
using LibraryManagement.UserRepository;
using Moq;

public class LoanServiceTest
{
    private readonly Mock<ILoanRepository> loanRepositoryMock;
    private readonly Mock<IBookRepository> bookRepositoryMock;
    private readonly ILoanService loanService;
    private readonly Mock<IClientRepository> clientRepositoryMock;

    public LoanServiceTest()
    {
        loanRepositoryMock = new Mock<ILoanRepository>();
        bookRepositoryMock = new Mock<IBookRepository>();
        clientRepositoryMock = new Mock<IClientRepository>();

        loanService = new LoanService(
            loanRepositoryMock.Object,
            bookRepositoryMock.Object,
            clientRepositoryMock.Object
        );
    }

    [Fact]
    public void should_return_all_loans()
    {
        var loans = new List<LoanResponseDto>
        {
            new LoanResponseDto
            {
                Id = 1,
                BookId = 1,
                ClientId = 1,
                LoanDate = DateTime.Now,
                ReturnedAt = null,
                ReturnDate = DateTime.Now.AddDays(7),
            },
            new LoanResponseDto
            {
                Id = 2,
                BookId = 2,
                ClientId = 2,
                LoanDate = DateTime.Now,
                ReturnedAt = null,
                ReturnDate = DateTime.Now.AddDays(7),
            },
        };

        loanRepositoryMock.Setup(repo => repo.GetAll()).Returns(loans);

        var result = loanService.GetAll();

        Assert.Collection(
            result,
            loan => Assert.Equal(1, loan.Id),
            loan => Assert.Equal(2, loan.Id)
        );
    }

    [Fact]
    public void should_return_loan_by_id()
    {
        var loan = new LoanResponseDto
        {
            Id = 1,
            BookId = 1,
            ClientId = 1,
            LoanDate = DateTime.Now,
            ReturnedAt = null,
            ReturnDate = DateTime.Now.AddDays(7),
        };

        loanRepositoryMock.Setup(repo => repo.GetById(1)).Returns(loan);
        var result = loanService.GetById(1);

        Assert.Equal(1, result.Id);
        Assert.Equal(1, result.BookId);
        Assert.Equal(1, result.ClientId);
        Assert.Equal(DateTime.Now.Date, result.LoanDate.Date);
        Assert.Equal(DateTime.Now.AddDays(7).Date, result.ReturnDate.Date);
    }

    [Fact]
    public void should_create_loan()
    {
        var loanInsertDto = new LoanInsertDto
        {
            BookId = 1,
            ClientId = 1,
            LoanDate = DateTime.Now,
            ReturnDate = DateTime.Now.AddDays(7),
        };

        var book = new BookResponseDto
        {
            Id = 1,
            Title = "Test Book",
            Author = "Test Author",
            Quantity = 5,
            Description = "Test Description",
            ImageUrl = "test.jpg",
            PublishYear = 2020,
        };

        var client = new ClientResponseDto
        {
            Id = 1,
            Name = "Test User",
            Email = "test@example.com",
        };

        bookRepositoryMock.Setup(repo => repo.GetById(1)).Returns(book);
        clientRepositoryMock.Setup(repo => repo.GetById(1)).Returns(client);

        loanRepositoryMock
            .Setup(repo => repo.Create(It.IsAny<LoanInsertDto>()))
            .Returns(
                new LoanResponseDto
                {
                    Id = 1,
                    BookId = 1,
                    ClientId = 1,
                    LoanDate = DateTime.Now,
                    ReturnedAt = null,
                    ReturnDate = DateTime.Now.AddDays(7),
                }
            );

        var result = loanService.Create(loanInsertDto);

        Assert.Equal(1, result.Id);
        Assert.Equal(1, result.BookId);
        Assert.Equal(1, result.ClientId);
        Assert.Equal(DateTime.Now.AddDays(7).Date, result.ReturnDate.Date);
        Assert.Equal(DateTime.Now.Date, result.LoanDate.Date);
    }

    [Fact]
    public void should_update_loan()
    {
        var loanUpdateDto = new LoanUpdateDto { ReturnedAt = DateTime.Now };

        var existingLoan = new LoanResponseDto
        {
            Id = 1,
            BookId = 1,
            ClientId = 1,
            LoanDate = DateTime.Now.AddDays(-7),
            ReturnDate = DateTime.Now,
            ReturnedAt = null,
        };

        var updatedLoan = new LoanResponseDto
        {
            Id = 1,
            BookId = 1,
            ClientId = 1,
            LoanDate = existingLoan.LoanDate,
            ReturnedAt = loanUpdateDto.ReturnedAt,
            ReturnDate = existingLoan.ReturnDate,
        };

        var book = new BookResponseDto
        {
            Id = 1,
            Title = "Test Book",
            Author = "Test Author",
            Quantity = 5,
            Description = "Test Description",
            ImageUrl = "test.jpg",
            PublishYear = 2020,
        };

        loanRepositoryMock.Setup(repo => repo.Update(1, loanUpdateDto)).Returns(updatedLoan);
        bookRepositoryMock.Setup(repo => repo.GetById(1)).Returns(book);

        var result = loanService.Update(1, loanUpdateDto);

        Assert.Equal(1, result.Id);
        Assert.NotNull(result.ReturnedAt);
    }

    [Fact]
    public async Task should_remove_loan()
    {
        var loan = new LoanResponseDto
        {
            Id = 1,
            BookId = 1,
            ClientId = 1,
            LoanDate = DateTime.Now,
            ReturnedAt = null,
            ReturnDate = DateTime.Now.AddDays(7),
        };

        loanRepositoryMock.Setup(repo => repo.GetById(1)).Returns(loan);
        loanRepositoryMock.Setup(repo => repo.Remove(1)).Returns(Task.CompletedTask);

        await loanService.Remove(1);

        loanRepositoryMock.Verify(repo => repo.Remove(1), Times.Once);
    }
}
