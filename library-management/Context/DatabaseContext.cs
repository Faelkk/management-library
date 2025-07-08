using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Contexts
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<Client> clients { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) { }

        public override int SaveChanges() => base.SaveChanges();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            base.SaveChangesAsync(cancellationToken);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var server = Environment.GetEnvironmentVariable("DBSERVER");
            var database = Environment.GetEnvironmentVariable("DBNAME");
            var dbuser = Environment.GetEnvironmentVariable("DBUSER");
            var dbpass = Environment.GetEnvironmentVariable("DBPASSWORD");

            var connectionString =
                $"Server={server};Database={database};User Id={dbuser};Password={dbpass};TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(
                connectionString,
                options =>
                {
                    options.EnableRetryOnFailure();
                }
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookGenre>().HasKey(bg => new { bg.BookId, bg.GenreId });

            modelBuilder
                .Entity<BookGenre>()
                .HasOne(bg => bg.Book)
                .WithMany(b => b.BookGenres)
                .HasForeignKey(bg => bg.BookId);

            modelBuilder
                .Entity<BookGenre>()
                .HasOne(bg => bg.Genre)
                .WithMany(g => g.BookGenres)
                .HasForeignKey(bg => bg.GenreId);

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("book");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Title).HasColumnName("title");
                entity.Property(e => e.Author).HasColumnName("author");
                entity.Property(e => e.PublishYear).HasColumnName("publish_year");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.ImageUrl).HasColumnName("image_url");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
            });

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.ToTable("loan");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ClientId).HasColumnName("client_id");
                entity.Property(e => e.BookId).HasColumnName("book_id");
                entity.Property(e => e.LoanDate).HasColumnName("loan_date");
                entity.Property(e => e.ReturnDate).HasColumnName("return_date");
                entity.Property(e => e.ReturnedAt).HasColumnName("returned_at");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genre");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Description).HasColumnName("description");
            });

            modelBuilder.Entity<BookGenre>(entity =>
            {
                entity.ToTable("book_genre");
                entity.Property(bg => bg.BookId).HasColumnName("book_id");
                entity.Property(bg => bg.GenreId).HasColumnName("genre_id");
            });

            modelBuilder.Entity<PasswordResetToken>(entity =>
            {
                entity.ToTable("password_reset_token");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Token).HasColumnName("token");
                entity.Property(e => e.ExpirationDate).HasColumnName("expiration_date");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.Role).HasColumnName("role");
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");

                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.PhoneNumber).IsUnique();
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("client");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");

                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.PhoneNumber).IsUnique();
            });
        }
    }
}
