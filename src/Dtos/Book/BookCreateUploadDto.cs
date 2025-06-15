

using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Dto
{
    public class BookCreateRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        public int PublishYear { get; set; }

        public string Description { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; }
    }


}
