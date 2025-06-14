
using LibraryManagement.Contexts;

namespace LibraryManagement.DatabaseSeeder;

public class DatabaseSeeder
{

    public static void ApplyMigrationsAndSeed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetService<DatabaseContext>();
    }
}