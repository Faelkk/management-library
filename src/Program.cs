using LibraryManagement.Contexts;
using LibraryManagement.DatabaseSeeder;
using LibraryManagement.LoanRepository;
using LibraryManagement.Repository;
using LibraryManagement.Services;
using LibraryManagement.UserRepository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 50 * 1024 * 1024;
});



builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddScoped<IDatabaseContext, DatabaseContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<TokenGenerator>();
builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();


DatabaseSeeder.ApplyMigrationsAndSeed(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
