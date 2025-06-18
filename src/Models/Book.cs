

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    using System.Collections.Generic;
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

            public string ImageUrl { get; set; }

            [Required]
            public int Quantity { get; set; }

            [NotMapped]
            public bool Available => Quantity > 0;

            public ICollection<Loan> Loans { get; set; } = new List<Loan>();

            public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
        }
    }

}
