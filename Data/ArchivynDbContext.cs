using Archivyn.Models;
using Microsoft.EntityFrameworkCore;

namespace Archivyn.Data;

public class ArchivynDbContext : DbContext
{
    public ArchivynDbContext(DbContextOptions<ArchivynDbContext> options)
        : base(options)
    {
    }

    public DbSet<KeyType> KeyTypes => Set<KeyType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KeyType>(entity =>
        {
            entity.ToTable("KEYTYPETABLE");

            entity.HasKey(e => e.KeyTypeNum);

            entity.Property(e => e.KeyTypeNum)
                .HasColumnName("keytypenum");

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(51);

            entity.Property(e => e.KeyTypeMask)
                .HasColumnName("keytypemask")
                .HasMaxLength(51);

            entity.Property(e => e.KeyTypeFlags)
                .HasColumnName("keytypeflags");

            entity.Property(e => e.DataType)
                .HasColumnName("datatype");

            entity.Property(e => e.KeyTypeLen)
                .HasColumnName("keytypelen");
        });
    }
}