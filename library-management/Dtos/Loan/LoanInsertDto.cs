using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Dto;

public class LoanInsertDto
{
    [Required]
    public int BookId { get; set; }

    [Required]
    public int ClientId { get; set; }

    [Required]
    public DateTime LoanDate { get; set; }

    [Required]
    public DateTime ReturnDate { get; set; }
}
