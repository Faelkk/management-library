using LibraryManagement.Dto;
using LibraryManagement.LoanRepository;
using LibraryManagement.Repository;

namespace LibraryManagement.Services;

public class LoanService : ILoanService
{
    private readonly ILoanRepository loanRepository;
    private readonly IBookRepository bookRepository;
    private readonly IClientRepository clientRepository;

    public LoanService(
        ILoanRepository loanRepository,
        IBookRepository bookRepository,
        IClientRepository clientRepository
    )
    {
        this.loanRepository = loanRepository;
        this.bookRepository = bookRepository;
        this.clientRepository = clientRepository;
    }

    public IEnumerable<LoanResponseDto> GetAll(int? year = null, int? month = null)
    {
        var loans = loanRepository.GetAll();

        if (year.HasValue && month.HasValue)
        {
            loans = loans.Where(l =>
                l.LoanDate.Year == year.Value && l.LoanDate.Month == month.Value
            );
        }

        return loans;
    }

    public LoanResponseDto GetById(int id)
    {
        var loan = loanRepository.GetById(id);

        if (loan == null)
        {
            throw new Exception("Not found loan");
        }

        return new LoanResponseDto
        {
            BookId = loan.BookId,
            Id = loan.Id,
            ClientId = loan.ClientId,
            LoanDate = loan.LoanDate,
            ReturnedAt = loan.ReturnedAt,
            ReturnDate = loan.ReturnDate,
        };
    }

    public LoanResponseDto Create(LoanInsertDto loanInsertDto)
    {
        var book = bookRepository.GetById(loanInsertDto.BookId);

        if (book == null)
        {
            throw new Exception("Book not found");
        }

        if (loanInsertDto.ReturnDate <= loanInsertDto.LoanDate)
        {
            throw new Exception("ReturnDate must be after LoanDate.");
        }

        if (book.Quantity <= 0)
        {
            throw new Exception("Book is not available for loan");
        }

        var user = clientRepository.GetById(loanInsertDto.ClientId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var newLoan = loanRepository.Create(loanInsertDto);

        var updatedBookDto = new BookUpdateDto { Quantity = book.Quantity - 1 };
        bookRepository.Update(book.Id, updatedBookDto);

        return new LoanResponseDto
        {
            BookId = newLoan.BookId,
            Id = newLoan.Id,
            ClientId = newLoan.ClientId,
            LoanDate = newLoan.LoanDate,
            ReturnedAt = newLoan.ReturnedAt,
            ReturnDate = newLoan.ReturnDate,
        };
    }

    public LoanResponseDto Update(int id, LoanUpdateDto LoanUpdateDto)
    {
        if (LoanUpdateDto.LoanDate != null && LoanUpdateDto.ReturnDate != null)
        {
            if (LoanUpdateDto.ReturnDate <= LoanUpdateDto.LoanDate)
            {
                throw new Exception("ReturnDate must be after LoanDate.");
            }
        }

        var updatedLoan = loanRepository.Update(id, LoanUpdateDto);

        if (updatedLoan == null)
        {
            throw new Exception("Not found loan for update");
        }

        var book = bookRepository.GetById(updatedLoan.BookId);
        if (book == null)
        {
            throw new Exception("Book not found");
        }

        var updatedBookDto = new BookUpdateDto { Quantity = book.Quantity + 1 };
        bookRepository.Update(book.Id, updatedBookDto);

        return new LoanResponseDto
        {
            BookId = updatedLoan.BookId,
            Id = updatedLoan.Id,
            ClientId = updatedLoan.ClientId,
            LoanDate = updatedLoan.LoanDate,
            ReturnedAt = updatedLoan.ReturnedAt,
            ReturnDate = updatedLoan.ReturnDate,
        };
    }

    public async Task Remove(int id)
    {
        var loanById = loanRepository.GetById(id);

        if (loanById == null)
        {
            throw new Exception("loan by id not found");
        }

        await loanRepository.Remove(id);
    }
}
