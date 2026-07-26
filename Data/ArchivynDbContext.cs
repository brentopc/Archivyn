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
    public DbSet<KeywordSet> KeywordSets => Set<KeywordSet>();
    public DbSet<KeywordSetKeyType> KeywordSetKeyTypes =>
        Set<KeywordSetKeyType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KeyType>(entity =>
        {
            entity.ToTable("KEYTYPETABLE");

            entity.HasKey(e => e.KeyTypeNum);

            entity.Property(e => e.KeyTypeNum)
                .HasColumnName("keytypenum");

            entity.Property(e => e.KeyTypeName)
                .HasColumnName("keytypename")
                .HasMaxLength(51);

            entity.HasIndex(e => e.KeyTypeName)
                .IsUnique();

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

        modelBuilder.Entity<KeywordSet>(entity =>
        {
            entity.ToTable("KEYWORDSET");

            entity.HasKey(e => e.KeySetTableNum);

            entity.Property(e => e.KeySetTableNum)
                .HasColumnName("keysettablenum")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.KeySetName)
                .HasColumnName("keysetname")
                .HasMaxLength(80)
                .IsRequired();

            entity.Property(e => e.IsKeyTypeGroup)
                .HasColumnName("iskeytypegroup");

            entity.Property(e => e.Flags)
                .HasColumnName("flags");

            entity.HasIndex(e => e.KeySetName)
                .IsUnique();
        });

        modelBuilder.Entity<KeywordSetKeyType>(entity =>
        {
            entity.ToTable("KEYWORDSETKEYTYPE");

            entity.HasKey(e => new
            {
                e.KeySetTableNum,
                e.KeyTypeNum
            });

            entity.Property(e => e.KeySetTableNum)
                .HasColumnName("keysettablenum");

            entity.Property(e => e.KeyTypeNum)
                .HasColumnName("keytypenum");

            entity.Property(e => e.DisplayOrder)
                .HasColumnName("displayorder");

            entity.HasOne(e => e.KeywordSet)
                .WithMany(e => e.KeywordTypeMemberships)
                .HasForeignKey(e => e.KeySetTableNum)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.KeyType)
                .WithMany(e => e.KeywordSetMemberships)
                .HasForeignKey(e => e.KeyTypeNum)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new
            {
                e.KeySetTableNum,
                e.DisplayOrder
            });
        });
    }
}