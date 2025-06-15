

using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Dto;

public interface LoanInsertDto
{
    [Required]
    public int BookId { get; set; }

    [Required]
    public int UserId { get; set; }
}