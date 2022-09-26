global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Design;
global using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.SqlServer;



namespace Assignment3;

public class KanbanContextFactory : IDesignTimeDbContextFactory<KanbanContext> 
{
    public KanbanContext CreateDbContext(string[] args) 
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        var connectionString = configuration.GetConnectionString("ConnectionString");

        var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new KanbanContext(optionsBuilder.Options);
    }
}