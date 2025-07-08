using LibraryManagement.Dto;

namespace LibraryManagement.LoanRepository;

public interface ILoanRepository
{
    IEnumerable<LoanResponseDto> GetAll();
    LoanResponseDto GetById(int loanId);
    LoanResponseDto Create(LoanInsertDto loanDto);
    LoanResponseDto Update(int id, LoanUpdateDto loanUpdateDto);
    Task Remove(int loanId);
};
