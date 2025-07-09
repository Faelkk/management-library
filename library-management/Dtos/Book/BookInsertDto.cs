using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Dto
{
    public class BookInsertDto
    {
        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres")]
        public string Title { get; set; }

        [Required(ErrorMessage = "O autor é obrigatório")]
        [StringLength(100, ErrorMessage = "O autor deve ter no máximo 100 caracteres")]
        public string Author { get; set; }

        [Required]
        public int PublishYear { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Pelo menos um gênero é obrigatório")]
        public List<int> GenreIds { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória")]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade deve ser maior ou igual a zero")]
        public int Quantity { get; set; }

        [Url(ErrorMessage = "URL da imagem inválida")]
        public string ImageUrl { get; set; }
    }
}
