using System.ComponentModel.DataAnnotations;

public class ClientInsertDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Formato de email inválido.")]
    public string Email { get; set; }

    [Required]
    [RegularExpression(
        @"^\(?\d{2}\)?\s?\d{4,5}\-?\d{4}$",
        ErrorMessage = "O número deve estar no formato (11) 91234-5678 ou 11912345678"
    )]
    public string PhoneNumber { get; set; }
}
