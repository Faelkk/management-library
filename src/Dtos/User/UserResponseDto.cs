
namespace LibraryManagement.Dto;

public class UserResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string Email { get; set; }

    public string Role { get; set; }

    public ICollection<LoanResponseDto> Loans { get; set; }
}