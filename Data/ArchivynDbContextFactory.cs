using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Archivyn.Data;

public sealed class ArchivynDbContextFactory
    : IDesignTimeDbContextFactory<ArchivynDbContext>
{
    public ArchivynDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ArchivynDbContext>()
            .UseNpgsql(
                "Host=localhost;" +
                "Port=5432;" +
                "Database=archivyn_design;" +
                "Username=postgres;" +
                "Password=postgres")
            .Options;

        return new ArchivynDbContext(options);
    }
}