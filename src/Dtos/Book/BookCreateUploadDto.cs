

using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Dto
{
    public class BookCreateRequest
    {
        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres")]
        public string Title { get; set; }

        [Required(ErrorMessage = "O autor é obrigatório")]
        [StringLength(100, ErrorMessage = "O autor deve ter no máximo 100 caracteres")]
        public string Author { get; set; }


        [Range(1500, 2100, ErrorMessage = "Ano de publicação inválido")]
        public int PublishYear { get; set; }


        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Pelo menos um gênero é obrigatório")]
        public List<int> GenreIds { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória")]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade deve ser maior ou igual a zero")]

        public int Quantity { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; }
    }


}
