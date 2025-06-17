

using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Dto;

public class LoanInsertDto
{
    [Required]
    public int BookId { get; set; }

    [Required]
    public int UserId { get; set; }
}