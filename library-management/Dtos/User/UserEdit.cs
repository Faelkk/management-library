using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Dto
{
    public class UserEditDto
    {
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string? Name { get; set; }

        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string? Email { get; set; }

        [RegularExpression(
            @"^\(?\d{2}\)?\s?\d{4,5}\-?\d{4}$",
            ErrorMessage = "O número deve estar no formato (11) 91234-5678 ou 11912345678"
        )]
        public string? PhoneNumber { get; set; }

        [RegularExpression(@"^(Admin|User)$", ErrorMessage = "O papel deve ser 'Admin' ou 'User'.")]
        public string? Role { get; set; }
    }
}
