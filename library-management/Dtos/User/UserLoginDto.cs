

using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Dto;

public class UserLoginDto
{
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
    public string Password { get; set; }

}
