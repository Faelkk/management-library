using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Contexts;

public interface IDatabaseContext
{
    DbSet<Book> Books { get; }
    DbSet<User> Users { get; }
    DbSet<Loan> Loans { get; }
    DbSet<Genre> Genres { get; }
    DbSet<BookGenre> BookGenres { get; }

    DbSet<Client> clients { get; }
    DbSet<PasswordResetToken> PasswordResetTokens { get; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
