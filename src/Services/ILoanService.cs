using LibraryManagement.Dto;

namespace LibraryManagement.Services;

public interface ILoanService
{

    IEnumerable<LoanResponseDto> GetAll();
    LoanResponseDto GetById(int id);
    LoanResponseDto Create(LoanInsertDto loanInsertDto);
    LoanResponseDto Update(int id);
    Task Remove(int id);
}