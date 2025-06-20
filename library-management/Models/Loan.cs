
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagement.Models;

namespace LibraryManagement.Models
{
    public class Loan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }


        [ForeignKey("Book")]
        public int BookId { get; set; }

        public Book Book { get; set; }

        [Required]
        public DateTime LoanDate { get; set; } = DateTime.Now;


        [Required]
        public DateTime ReturnDate { get; set; } = DateTime.Now.AddDays(7);

        public DateTime? ReturnedAt { get; set; }
    }
}
