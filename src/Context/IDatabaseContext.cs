using LibraryManagement.Models;
using LibraryManagement.Models.LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Contexts;

public interface IDatabaseContext
{
    DbSet<Book> Books { get; }
    DbSet<User> Users { get; }
    DbSet<Loan> Loans { get; }
    DbSet<Genre> Genres { get; }
    DbSet<BookGenre> BookGenres { get; }
    DbSet<PasswordResetToken> PasswordResetTokens { get; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
