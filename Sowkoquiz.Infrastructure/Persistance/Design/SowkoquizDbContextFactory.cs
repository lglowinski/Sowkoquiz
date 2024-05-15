using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sowkoquiz.Infrastructure.Persistance.Design;

public class SowkoquizDbContextFactory : IDesignTimeDbContextFactory<SowkoquizDbContext>
{
    public SowkoquizDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SowkoquizDbContext>();
        optionsBuilder.UseSqlite();

        return new SowkoquizDbContext(optionsBuilder.Options, null);
    }
}