using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Dto
{
    public class GenreUpdateDto
    {
        [StringLength(100, ErrorMessage = "O Name deve ter no máximo 100 caracteres")]
        public string Name { get; set; }
    }
}