using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Contexts;

public class DatabaseContext : DbContext, IDatabaseContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Book> Books { get; set; }

    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }


    public int SaveChanges()
    {
        return base.SaveChanges();
    }

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }


    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var server = Environment.GetEnvironmentVariable("DBSERVER");
        var database = Environment.GetEnvironmentVariable("DBNAME");
        var dbuser = Environment.GetEnvironmentVariable("DBUSER");
        var dbpass = Environment.GetEnvironmentVariable("DBPASSWORD");

        var connectionString = $"Server={server};Database={database};User Id={dbuser};Password={dbpass};TrustServerCertificate=True";

        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("book");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Author).HasColumnName("author");
            entity.Property(e => e.PublishYear).HasColumnName("publish_year");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Available).HasColumnName("available");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Role).HasColumnName("role");
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.ToTable("loan");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.LoanDate).HasColumnName("loan_date");
            entity.Property(e => e.ReturnDate).HasColumnName("return_date");
            entity.Property(e => e.ReturnAt).HasColumnName("return_at");
        });
    }
}
