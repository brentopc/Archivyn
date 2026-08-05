using Archivyn.Models;
using Microsoft.EntityFrameworkCore;

namespace Archivyn.Data;

public class ArchivynDbContext : DbContext
{
    public ArchivynDbContext(
        DbContextOptions<ArchivynDbContext> options)
        : base(options)
    {
    }

    public DbSet<KeyType> KeyTypes => Set<KeyType>();
    public DbSet<KeywordSet> KeywordSets => Set<KeywordSet>();
    public DbSet<KeywordSetKeyType> KeywordSetKeyTypes => Set<KeywordSetKeyType>();

    public DbSet<ItemTypeGroup> ItemTypeGroups => Set<ItemTypeGroup>();
    public DbSet<DocumentType> DocumentTypes => Set<DocumentType>();

    public DbSet<DocumentTypeKeyType> DocumentTypeKeyTypes => Set<DocumentTypeKeyType>();
    public DbSet<DocumentTypeKeywordTypeGroup> DocumentTypeKeywordTypeGroups => Set<DocumentTypeKeywordTypeGroup>();

    public DbSet<ItemData> ItemData => Set<ItemData>();

    public DbSet<KeyItem> KeyItems => Set<KeyItem>();

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

            entity.Property(e => e.DataType)
                .HasColumnName("datatype");

            entity.Property(e => e.KeyTypeLen)
                .HasColumnName("keytypelen");

            entity.Property(x => x.IsSystem)
                .HasDefaultValue(false);
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

            entity.Property(x => x.IsSystem)
                .HasDefaultValue(false);
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

            entity.Property(x => x.IsSystem)
                .HasDefaultValue(false);
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

            entity.Property(x => x.IsSystem)
                .HasDefaultValue(false);
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

            entity.Property(x => x.IsSystem)
                .HasDefaultValue(false);
        });

        modelBuilder.Entity<DocumentTypeKeyType>(entity =>
        {
            entity.ToTable("DOCTYPEKEYTYPE");

            entity.HasKey(assignment => new
            {
                assignment.ItemTypeNum,
                assignment.KeyTypeNum
            });

            entity.Property(assignment => assignment.ItemTypeNum)
                .HasColumnName("itemtypenum");

            entity.Property(assignment => assignment.KeyTypeNum)
                .HasColumnName("keytypenum");

            entity.Property(assignment => assignment.DisplayOrder)
                .HasColumnName("displayorder");

            entity.HasOne(assignment => assignment.DocumentType)
                .WithMany(documentType =>
                    documentType.KeywordTypeAssignments)
                .HasForeignKey(assignment => assignment.ItemTypeNum)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(assignment => assignment.KeyType)
                .WithMany(keyword => keyword.DocumentTypeAssignments)
                .HasForeignKey(assignment => assignment.KeyTypeNum)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(assignment => new
            {
                assignment.ItemTypeNum,
                assignment.DisplayOrder
            });

            entity.Property(x => x.IsSystem)
                .HasDefaultValue(false);
        });

        modelBuilder.Entity<DocumentTypeKeywordTypeGroup>(entity =>
        {
            entity.ToTable("DOCTYPEKEYTYPEGROUP");

            entity.HasKey(assignment => new
            {
                assignment.ItemTypeNum,
                assignment.KeySetTableNum
            });

            entity.Property(assignment => assignment.ItemTypeNum)
                .HasColumnName("itemtypenum");

            entity.Property(assignment => assignment.KeySetTableNum)
                .HasColumnName("keysettablenum");

            entity.Property(assignment => assignment.DisplayOrder)
                .HasColumnName("displayorder");

            entity.HasOne(assignment => assignment.DocumentType)
                .WithMany(documentType =>
                    documentType.KeywordTypeGroupAssignments)
                .HasForeignKey(assignment => assignment.ItemTypeNum)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(assignment => assignment.KeywordTypeGroup)
                .WithMany(group => group.DocumentTypeAssignments)
                .HasForeignKey(assignment => assignment.KeySetTableNum)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(assignment => new
            {
                assignment.ItemTypeNum,
                assignment.DisplayOrder
            });

            entity.Property(x => x.IsSystem)
                .HasDefaultValue(false);
        });

        modelBuilder.Entity<ItemData>(entity =>
        {
            entity.ToTable("itemdata");

            entity.HasKey(x => x.ItemNum);

            entity.Property(x => x.ItemNum)
                .HasColumnName("itemnum")
                .ValueGeneratedOnAdd();

            entity.Property(x => x.ItemName)
                .HasColumnName("itemname")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(x => x.Status)
                .HasColumnName("status");

            entity.Property(x => x.ItemTypeGroupNum)
                .HasColumnName("itemtypegroupnum");

            entity.Property(x => x.ItemTypeNum)
                .HasColumnName("itemtypenum");

            entity.Property(x => x.ItemDate)
                .HasColumnName("itemdate");

            entity.Property(x => x.DateStored)
                .HasColumnName("datestored");

            entity.Property(x => x.UserNum)
                .HasColumnName("usernum");

            entity.Property(x => x.OriginalFileName)
                .HasColumnName("originalfilename")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(x => x.FileExtension)
                .HasColumnName("fileextension")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.FileSize)
                .HasColumnName("filesize");

            entity.HasOne(x => x.DocumentType)
                .WithMany()
                .HasForeignKey(x => x.ItemTypeNum)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.ItemTypeGroup)
                .WithMany()
                .HasForeignKey(x => x.ItemTypeGroupNum)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<KeyItem>(entity =>
        {
            entity.ToTable("keyitem");

            entity.HasKey(x => x.KeyItemNum);

            entity.Property(x => x.KeyItemNum)
                .HasColumnName("keyitemnum")
                .ValueGeneratedOnAdd();

            entity.Property(x => x.ItemNum)
                .HasColumnName("itemnum");

            entity.Property(x => x.KeyTypeNum)
                .HasColumnName("keytypenum");

            entity.Property(x => x.KeyValueChar)
                .HasColumnName("keyvaluechar")
                .HasMaxLength(250);

            entity.Property(x => x.KeyValueNum)
                .HasColumnName("keyvaluenum");

            entity.Property(x => x.KeyValueDate)
                .HasColumnName("keyvaluedate");

            entity.HasOne(x => x.ItemData)
                .WithMany(x => x.KeywordValues)
                .HasForeignKey(x => x.ItemNum)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.KeyType)
                .WithMany()
                .HasForeignKey(x => x.KeyTypeNum)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => new
            {
                x.ItemNum,
                x.KeyTypeNum
            });

            entity.HasIndex(x => new
            {
                x.KeyTypeNum,
                x.KeyValueChar
            });

            entity.HasIndex(x => new
            {
                x.KeyTypeNum,
                x.KeyValueNum
            });

            entity.HasIndex(x => new
            {
                x.KeyTypeNum,
                x.KeyValueDate
            });
        });

        ConfigureSystemManagedEntities(modelBuilder);
        ConfigureSystemData(modelBuilder);
    }

    private static void ConfigureSystemManagedEntities(ModelBuilder modelBuilder)
    {
        var systemManagedEntityTypes = modelBuilder.Model
            .GetEntityTypes()
            .Where(entityType =>
                typeof(ISystemManagedEntity)
                    .IsAssignableFrom(entityType.ClrType));

        foreach (var entityType in systemManagedEntityTypes)
        {
            modelBuilder.Entity(entityType.ClrType)
                .Property(nameof(ISystemManagedEntity.IsSystem))
                .HasDefaultValue(false);
        }
    }
    private static void ConfigureSystemData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ItemTypeGroup>().HasData(
            new
            {
                ItemTypeGroupNum = 1L,
                ItemTypeGroupName = "System Documents",
                Flags = 0L,
                IsSystem = true
            });

        modelBuilder.Entity<DocumentType>().HasData(
            new
            {
                ItemTypeNum = 1L,
                ItemTypeName = "Unindexed",
                ItemTypeGroupNum = 1L,
                Flags = 0L,
                IsSystem = true
            });

        modelBuilder.Entity<KeyType>().HasData(
            new
            {
                KeyTypeNum = 1L,
                KeyTypeName = ">> Document Date",
                DataType = KeyType.DataTypes.Date,
                KeyTypeLen = 10L,
                IsSystem = true,
                AddToAllDocumentTypes = true,
                IsRequiredOnAllDocumentTypes = true,
                AllDocumentTypesDisplayOrder = 1
            });
        modelBuilder.Entity<KeyType>().HasData(
            new
            {
                KeyTypeNum = 2L,
                KeyTypeName = "Description",
                DataType = KeyType.DataTypes.Text,
                KeyTypeLen = 250L,
                IsSystem = false,
                AddToAllDocumentTypes = true,
                IsRequiredOnAllDocumentTypes = false,
                AllDocumentTypesDisplayOrder = 2
            });

        modelBuilder.Entity<DocumentTypeKeyType>().HasData(
            new
            {
                ItemTypeNum = 1L,
                KeyTypeNum = 2L,
                DisplayOrder = 2,
                IsRequired = true,
                IsSystem = true
            });
    }

    private void ProtectSystemConfiguration()
    {
        ChangeTracker.DetectChanges();

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is not ISystemManagedEntity)
            {
                continue;
            }

            if (entry.State is not EntityState.Modified
                and not EntityState.Deleted)
            {
                continue;
            }

            var isSystemProperty = entry.Property(
                nameof(ISystemManagedEntity.IsSystem));

            var wasSystem =
                isSystemProperty.OriginalValue is true;

            var isSystemNow =
                isSystemProperty.CurrentValue is true;

            if (!wasSystem && !isSystemNow)
            {
                continue;
            }

            throw new InvalidOperationException(
                $"{GetEntityDescription(entry)} is managed by " +
                "Archivyn and cannot be modified or deleted.");
        }
    }
    private static string GetEntityDescription(
    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
    {
        var entityName = entry.Metadata.ClrType.Name;
        var primaryKey = entry.Metadata.FindPrimaryKey();

        if (primaryKey is null)
        {
            return entityName;
        }

        var keyParts = primaryKey.Properties.Select(property =>
        {
            var value = entry.Property(property.Name).CurrentValue;

            return $"{property.Name}={value}";
        });

        return $"{entityName} ({string.Join(", ", keyParts)})";
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        AddSystemKeywordsToNewDocumentTypes();
        ProtectSystemConfiguration();

        return base.SaveChanges(
            acceptAllChangesOnSuccess);
    }
    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        await AddSystemKeywordsToNewDocumentTypesAsync(cancellationToken);

        ProtectSystemConfiguration();

        return await base.SaveChangesAsync(
            acceptAllChangesOnSuccess,
            cancellationToken);
    }

    private void AddSystemKeywordsToNewDocumentTypes()
    {
        ChangeTracker.DetectChanges();

        List<DocumentType> newDocumentTypes = ChangeTracker
            .Entries<DocumentType>()
            .Where(entry => entry.State == EntityState.Added)
            .Select(entry => entry.Entity)
            .ToList();

        if (newDocumentTypes.Count == 0)
        {
            return;
        }

        List<KeyType> systemKeywords = Set<KeyType>()
            .AsNoTracking()
            .Where(keyword =>
                keyword.IsSystem ||
                keyword.AddToAllDocumentTypes)
            .OrderBy(keyword =>
                keyword.AllDocumentTypesDisplayOrder)
            .ToList();

        if (systemKeywords.Count == 0)
        {
            return;
        }

        List<DocumentTypeKeyType> trackedAssignments =
            ChangeTracker
                .Entries<DocumentTypeKeyType>()
                .Where(entry =>
                    entry.State != EntityState.Deleted)
                .Select(entry => entry.Entity)
                .ToList();

        foreach (DocumentType documentType in newDocumentTypes)
        {
            foreach (KeyType keywordType in systemKeywords)
            {
                bool alreadyAssigned = trackedAssignments.Any(
                    assignment =>
                        ReferenceEquals(
                            assignment.DocumentType,
                            documentType)
                        &&
                        assignment.KeyTypeNum ==
                            keywordType.KeyTypeNum);

                if (alreadyAssigned)
                {
                    continue;
                }

                var assignment = DocumentTypeKeyType.CreateSystemAssignment(documentType,keywordType);

                Set<DocumentTypeKeyType>().Add(assignment);
                trackedAssignments.Add(assignment);
            }
        }
    }
    private async Task AddSystemKeywordsToNewDocumentTypesAsync(CancellationToken cancellationToken)
    {
        ChangeTracker.DetectChanges();

        List<DocumentType> newDocumentTypes = ChangeTracker
            .Entries<DocumentType>()
            .Where(entry => entry.State == EntityState.Added)
            .Select(entry => entry.Entity)
            .ToList();

        if (newDocumentTypes.Count == 0)
        {
            return;
        }

        List<KeyType> systemKeywords =
            await Set<KeyType>()
                .AsNoTracking()
                .Where(keyword =>
                    keyword.IsSystem ||
                    keyword.AddToAllDocumentTypes)
                .OrderBy(keyword =>
                    keyword.AllDocumentTypesDisplayOrder)
                .ToListAsync(cancellationToken);

        if (systemKeywords.Count == 0)
        {
            return;
        }

        List<DocumentTypeKeyType> trackedAssignments =
            ChangeTracker
                .Entries<DocumentTypeKeyType>()
                .Where(entry =>
                    entry.State != EntityState.Deleted)
                .Select(entry => entry.Entity)
                .ToList();

        foreach (DocumentType documentType in newDocumentTypes)
        {
            foreach (KeyType keywordType in systemKeywords)
            {
                bool alreadyAssigned = trackedAssignments.Any(
                    assignment =>
                        ReferenceEquals(
                            assignment.DocumentType,
                            documentType)
                        &&
                        assignment.KeyTypeNum ==
                            keywordType.KeyTypeNum);

                if (alreadyAssigned)
                {
                    continue;
                }

                var assignment = DocumentTypeKeyType.CreateSystemAssignment(documentType, keywordType);

                Set<DocumentTypeKeyType>().Add(assignment);
                trackedAssignments.Add(assignment);
            }
        }
    }
    public async Task EnsureSystemKeywordsOnAllDocumentTypesAsync(CancellationToken cancellationToken = default)
    {
        var documentTypes = await Set<DocumentType>()
            .ToListAsync(cancellationToken);

        var systemKeywords = await Set<KeyType>()
            .Where(keyType =>
                keyType.IsSystem ||
                keyType.AddToAllDocumentTypes)
            .OrderBy(keyType =>
                keyType.AllDocumentTypesDisplayOrder)
            .ToListAsync(cancellationToken);

        if (documentTypes.Count == 0 ||
            systemKeywords.Count == 0)
        {
            return;
        }

        var existingAssignments =
            await Set<DocumentTypeKeyType>()
                .AsNoTracking()
                .Select(assignment => new
                {
                    assignment.ItemTypeNum,
                    assignment.KeyTypeNum
                })
                .ToListAsync(cancellationToken);

        var existingPairs = existingAssignments
            .Select(assignment => (
                assignment.ItemTypeNum,
                assignment.KeyTypeNum))
            .ToHashSet();

        foreach (var documentType in documentTypes)
        {
            foreach (var keyType in systemKeywords)
            {
                var pair = (
                    documentType.ItemTypeNum,
                    keyType.KeyTypeNum);

                if (existingPairs.Contains(pair))
                {
                    continue;
                }

                var assignment =
                    DocumentTypeKeyType.CreateSystemAssignment(
                        documentType,
                        keyType);

                Set<DocumentTypeKeyType>().Add(assignment);

                existingPairs.Add(pair);
            }
        }

        await SaveChangesAsync(cancellationToken);
    }
}