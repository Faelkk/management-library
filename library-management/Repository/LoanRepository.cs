using LibraryManagement.Contexts;
using LibraryManagement.Dto;
using LibraryManagement.Models;
using LibraryManagement.UserRepository;

namespace LibraryManagement.LoanRepository;

public class LoanRepository : ILoanRepository
{
    private readonly IDatabaseContext databaseContext;

    public LoanRepository(IDatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    public IEnumerable<LoanResponseDto> GetAll()
    {
        return databaseContext
            .Loans.Select(L => new LoanResponseDto
            {
                Id = L.Id,
                BookId = L.BookId,
                ClientId = L.ClientId,
                LoanDate = L.LoanDate,
                ReturnedAt = L.ReturnedAt,
                ReturnDate = L.ReturnDate,
            })
            .ToList();
    }

    public LoanResponseDto? GetById(int loanId)
    {
        var loan = databaseContext.Loans.FirstOrDefault(l => l.Id == loanId);

        if (loan == null)
        {
            return null;
        }

        return new LoanResponseDto
        {
            BookId = loan.BookId,
            Id = loan.Id,
            LoanDate = loan.LoanDate,
            ClientId = loan.ClientId,
            ReturnedAt = null,
            ReturnDate = loan.ReturnDate,
        };
    }

    public LoanResponseDto Create(LoanInsertDto loanDto)
    {
        var loan = new Loan
        {
            BookId = loanDto.BookId,
            ClientId = loanDto.ClientId,
            LoanDate = loanDto.LoanDate,
            ReturnDate = loanDto.ReturnDate,
            ReturnedAt = null,
        };

        databaseContext.Loans.Add(loan);
        databaseContext.SaveChanges();

        return new LoanResponseDto
        {
            Id = loan.Id,
            BookId = loan.BookId,
            LoanDate = loan.LoanDate,
            ClientId = loan.ClientId,
            ReturnedAt = loan.ReturnedAt,
            ReturnDate = loan.ReturnDate,
        };
    }

    public LoanResponseDto Update(int id, LoanUpdateDto loanUpdateDto)
    {
        var loan = databaseContext.Loans.FirstOrDefault(l => l.Id == id);

        if (loan == null)
            throw new Exception("Loan not found");

        if (loanUpdateDto.BookId != null)
            loan.BookId = loanUpdateDto.BookId.Value;

        if (loanUpdateDto.ClientId != null)
            loan.ClientId = loanUpdateDto.ClientId.Value;

        if (loanUpdateDto.LoanDate != null)
            loan.LoanDate = loanUpdateDto.LoanDate.Value;

        if (loanUpdateDto.ReturnDate != null)
            loan.ReturnDate = loanUpdateDto.ReturnDate.Value;

        if (loanUpdateDto.ReturnedAt != null)
            loan.ReturnedAt = loanUpdateDto.ReturnedAt;

        databaseContext.SaveChanges();

        return new LoanResponseDto
        {
            Id = loan.Id,
            ClientId = loan.ClientId,
            BookId = loan.BookId,
            LoanDate = loan.LoanDate,
            ReturnDate = loan.ReturnDate,
            ReturnedAt = loan.ReturnedAt,
        };
    }

    public async Task Remove(int loanId)
    {
        var loan = databaseContext.Loans.FirstOrDefault(l => l.Id == loanId);

        if (loan == null)
            throw new Exception("loan not found");

        databaseContext.Loans.Remove(loan);
        await databaseContext.SaveChangesAsync();
    }
};
