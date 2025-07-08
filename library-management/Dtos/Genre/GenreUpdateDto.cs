using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Dto
{
    public class GenreUpdateDto
    {
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string? Name { get; set; }

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string? Description { get; set; }
    }
}
