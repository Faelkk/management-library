
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
        return databaseContext.Loans.Select(L => new LoanResponseDto
        {
            Id = L.Id,
            BookId = L.BookId,
            LoanDate = L.LoanDate,
            ReturnAt = L.ReturnAt,
            ReturnDate = L.ReturnDate,
        }).ToList();
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
            ReturnAt = loan.ReturnAt,
            ReturnDate = loan.ReturnDate,
        };

    }
    public LoanResponseDto Create(LoanInsertDto loanDto)
    {
        var loan = new Loan
        {
            BookId = loanDto.BookId,
            UserId = loanDto.UserId,
            LoanDate = DateTime.Now,
            ReturnDate = DateTime.Now.AddDays(7),
            ReturnAt = null
        };

        databaseContext.Loans.Add(loan);
        databaseContext.SaveChanges();

        return new LoanResponseDto
        {
            Id = loan.Id,
            BookId = loan.BookId,
            LoanDate = loan.LoanDate,
            ReturnAt = loan.ReturnAt,
            ReturnDate = loan.ReturnDate,
        };

    }
    public LoanResponseDto Update(int id)
    {
        var loan = databaseContext.Loans.FirstOrDefault(l => l.Id == id);

        if (loan == null) return null;

        loan.ReturnAt = DateTime.Now;

        databaseContext.SaveChanges();

        return new LoanResponseDto
        {
            Id = loan.Id,
            BookId = loan.BookId,
            LoanDate = loan.LoanDate,
            ReturnAt = loan.ReturnAt,
            ReturnDate = loan.ReturnDate,
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


