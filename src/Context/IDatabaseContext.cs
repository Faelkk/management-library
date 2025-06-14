using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Contexts;

public interface IDatabaseContext
{

    DbSet<Book> Books { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<Loan> Loans { get; set; }
    DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    public int SaveChanges();
    public Task<int> SaveChangesAsync();
}
