namespace LibraryManagement.Dto;

public class LoanUpdateDto
{
    public int? BookId { get; set; }

    public int? ClientId { get; set; }

    public DateTime? LoanDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public DateTime? ReturnedAt { get; set; }
}
