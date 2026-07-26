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
    public DbSet<KeywordSetKeyType> KeywordSetKeyTypes => Set<KeywordSetKeyType>();

    public DbSet<ItemTypeGroup> ItemTypeGroups => Set<ItemTypeGroup>();

    public DbSet<DocumentType> DocumentTypes => Set<DocumentType>();

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

        modelBuilder.Entity<ItemTypeGroup>(entity =>
        {
            entity.ToTable("ITEMTYPEGROUP");

            entity.HasKey(group =>
                group.ItemTypeGroupNum);

            entity.Property(group =>
                    group.ItemTypeGroupNum)
                .HasColumnName("itemtypegroupnum")
                .ValueGeneratedOnAdd();

            entity.Property(group =>
                    group.ItemTypeGroupName)
                .HasColumnName("itemtypegroupname")
                .HasMaxLength(66)
                .IsRequired();

            entity.Property(group =>
                    group.Flags)
                .HasColumnName("flags")
                .HasDefaultValue(0L);

            entity.HasIndex(group =>
                    group.ItemTypeGroupName)
                .IsUnique();
        });

        modelBuilder.Entity<DocumentType>(entity =>
        {
            entity.ToTable("DOCTYPE");

            entity.HasKey(documentType =>
                documentType.ItemTypeNum);

            entity.Property(documentType =>
                    documentType.ItemTypeNum)
                .HasColumnName("itemtypenum")
                .ValueGeneratedOnAdd();

            entity.Property(documentType =>
                    documentType.ItemTypeName)
                .HasColumnName("itemtypename")
                .HasMaxLength(66)
                .IsRequired();

            entity.Property(documentType =>
                    documentType.ItemTypeGroupNum)
                .HasColumnName("itemtypegroupnum");

            entity.Property(documentType =>
                    documentType.AutoNameString)
                .HasColumnName("autonamestring")
                .HasMaxLength(150);

            entity.HasIndex(documentType =>
                    documentType.ItemTypeName)
                .IsUnique();

            entity.HasOne(documentType =>
                    documentType.ItemTypeGroup)
                .WithMany(group =>
                    group.DocumentTypes)
                .HasForeignKey(documentType =>
                    documentType.ItemTypeGroupNum)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}