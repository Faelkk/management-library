using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using LibraryManagement.Contexts;
using LibraryManagement.DatabaseSeeder;
using LibraryManagement.LoanRepository;
using LibraryManagement.Repository;
using LibraryManagement.Services;
using LibraryManagement.UserRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;

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
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IUploadFileService, UploadFileService>();
builder.Services.AddScoped<TokenGenerator>();
builder.Services.AddControllers();


builder.Services.Configure<TokenOptions>(
    builder.Configuration.GetSection(TokenOptions.Token)
);

var tokenOptions = builder.Configuration.GetSection(TokenOptions.Token);


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = builder.Configuration["Token:Secret"] ?? Environment.GetEnvironmentVariable("JWT_KEY");

    if (string.IsNullOrEmpty(key))
    {
        throw new Exception("JWT key is missing");
    }

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Admin"));
    options.AddPolicy("Authenticated", policy =>
        policy.RequireAuthenticatedUser());
});


builder.Services.AddOpenApi();


builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", limiterOptions =>
    {
        limiterOptions.PermitLimit = 5;
        limiterOptions.Window = TimeSpan.FromSeconds(10);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 0;
    });
});


var port = builder.Configuration["APIPORT"];
builder.WebHost.UseUrls($"http://*:{port}");

// 🚀 Build app
var app = builder.Build();


var uploadsPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Uploads"));

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});


DatabaseSeeder.ApplyMigrationsAndSeed(app.Services);


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseRateLimiter();
app.UseAuthorization();


app.MapControllers().RequireRateLimiting("fixed");

// 🚀 Executa
app.Run();
