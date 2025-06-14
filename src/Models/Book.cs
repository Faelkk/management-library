using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Author { get; set; }
        public int PublishYear { get; set; }

        public string Description { get; set; }

        public bool Available { get; set; }

        public string ImageUrl { get; set; }
        public ICollection<Loan> Loans { get; set; }
    }
}
