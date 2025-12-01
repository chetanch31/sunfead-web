using Microsoft.EntityFrameworkCore;
using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data;

/// <summary>
/// seed data for database initialization
/// idempotent seeding - checks existence before inserting
/// todo: hash password using proper hashing library (bcrypt/argon2) before production use
/// </summary>
public static class SeedData
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // seed roles
        await SeedRolesAsync(context);
        
        // seed tax rates
        await SeedTaxRatesAsync(context);
        
        // seed categories
        await SeedCategoriesAsync(context);
        
        // seed brands
        await SeedBrandsAsync(context);
        
        // seed products with variants, prices, and inventory
        await SeedProductsAsync(context);
        
        // seed admin user
        await SeedAdminUserAsync(context);
        
        await context.SaveChangesAsync();
    }
    
    private static async Task SeedRolesAsync(ApplicationDbContext context)
    {
        if (await context.Roles.AnyAsync()) return;
        
        var roles = new[]
        {
            new Role
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "system administrator with full access",
                CreatedAt = DateTime.UtcNow
            },
            new Role
            {
                Id = Guid.NewGuid(),
                Name = "Customer",
                NormalizedName = "CUSTOMER",
                Description = "regular customer account",
                CreatedAt = DateTime.UtcNow
            },
            new Role
            {
                Id = Guid.NewGuid(),
                Name = "SalesRep",
                NormalizedName = "SALESREP",
                Description = "sales representative for bulk orders",
                CreatedAt = DateTime.UtcNow
            }
        };
        
        await context.Roles.AddRangeAsync(roles);
    }
    
    private static async Task SeedTaxRatesAsync(ApplicationDbContext context)
    {
        if (await context.TaxRates.AnyAsync()) return;
        
        var taxRates = new[]
        {
            new TaxRate
            {
                Id = Guid.NewGuid(),
                Name = "GST 5%",
                RatePercent = 5.0m,
                HsnSacCode = "1905",
                EffectiveFrom = new DateTime(2023, 1, 1),
                CreatedAt = DateTime.UtcNow
            },
            new TaxRate
            {
                Id = Guid.NewGuid(),
                Name = "GST 12%",
                RatePercent = 12.0m,
                HsnSacCode = "2106",
                EffectiveFrom = new DateTime(2023, 1, 1),
                CreatedAt = DateTime.UtcNow
            },
            new TaxRate
            {
                Id = Guid.NewGuid(),
                Name = "GST 18%",
                RatePercent = 18.0m,
                HsnSacCode = "1806",
                EffectiveFrom = new DateTime(2023, 1, 1),
                CreatedAt = DateTime.UtcNow
            }
        };
        
        await context.TaxRates.AddRangeAsync(taxRates);
    }
    
    private static async Task SeedCategoriesAsync(ApplicationDbContext context)
    {
        if (await context.Categories.AnyAsync()) return;
        
        var categories = new[]
        {
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Namkeen & Savory",
                Slug = "namkeen-savory",
                Description = "traditional indian savory snacks",
                DisplayOrder = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Sweet Snacks",
                Slug = "sweet-snacks",
                Description = "sweet treats and confections",
                DisplayOrder = 2,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Chips & Crackers",
                Slug = "chips-crackers",
                Description = "crispy chips and crackers",
                DisplayOrder = 3,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Nuts & Seeds",
                Slug = "nuts-seeds",
                Description = "roasted nuts and healthy seeds",
                DisplayOrder = 4,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Healthy Snacks",
                Slug = "healthy-snacks",
                Description = "nutritious guilt-free snacking",
                DisplayOrder = 5,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };
        
        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }
    
    private static async Task SeedBrandsAsync(ApplicationDbContext context)
    {
        if (await context.Brands.AnyAsync()) return;
        
        var brands = new[]
        {
            new Brand
            {
                Id = Guid.NewGuid(),
                Name = "Haldiram's",
                Description = "iconic indian snacks since 1937",
                CreatedAt = DateTime.UtcNow
            },
            new Brand
            {
                Id = Guid.NewGuid(),
                Name = "Bikano",
                Description = "authentic taste of rajasthan",
                CreatedAt = DateTime.UtcNow
            },
            new Brand
            {
                Id = Guid.NewGuid(),
                Name = "SunFeast",
                Description = "delicious snacks from itc",
                CreatedAt = DateTime.UtcNow
            },
            new Brand
            {
                Id = Guid.NewGuid(),
                Name = "Kurkure",
                Description = "crunchy fun for everyone",
                CreatedAt = DateTime.UtcNow
            }
        };
        
        await context.Brands.AddRangeAsync(brands);
        await context.SaveChangesAsync();
    }
    
    private static async Task SeedProductsAsync(ApplicationDbContext context)
    {
        if (await context.Products.AnyAsync()) return;
        
        var namkeenCategory = await context.Categories.FirstAsync(c => c.Slug == "namkeen-savory");
        var chipsCategory = await context.Categories.FirstAsync(c => c.Slug == "chips-crackers");
        var sweetCategory = await context.Categories.FirstAsync(c => c.Slug == "sweet-snacks");
        var nutsCategory = await context.Categories.FirstAsync(c => c.Slug == "nuts-seeds");
        var healthyCategory = await context.Categories.FirstAsync(c => c.Slug == "healthy-snacks");
        
        var haldirams = await context.Brands.FirstAsync(b => b.Name == "Haldiram's");
        var bikano = await context.Brands.FirstAsync(b => b.Name == "Bikano");
        var sunfeast = await context.Brands.FirstAsync(b => b.Name == "SunFeast");
        var kurkure = await context.Brands.FirstAsync(b => b.Name == "Kurkure");
        
        var gst5 = await context.TaxRates.FirstAsync(t => t.Name == "GST 5%");
        var gst12 = await context.TaxRates.FirstAsync(t => t.Name == "GST 12%");
        
        // product 1: aloo bhujia
        var alooBhujia = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Aloo Bhujia",
            Slug = "aloo-bhujia",
            ShortDescription = "crispy potato noodles with spices",
            FullDescription = "<p>classic aloo bhujia made from premium potatoes and aromatic spices. perfect tea-time snack.</p>",
            CategoryId = namkeenCategory.Id,
            BrandId = haldirams.Id,
            MetaTitle = "Buy Aloo Bhujia Online - Haldiram's",
            MetaDescription = "crispy and delicious aloo bhujia snacks online",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        await context.Products.AddAsync(alooBhujia);
        await context.SaveChangesAsync();
        
        var alooBhujiaVariant200g = new ProductVariant
        {
            Id = Guid.NewGuid(),
            ProductId = alooBhujia.Id,
            Sku = "ALB-200G",
            VariantName = "200g",
            WeightInGrams = 200,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            RowVersion = new byte[] { 1 }
        };
        
        var alooBhujiaVariant500g = new ProductVariant
        {
            Id = Guid.NewGuid(),
            ProductId = alooBhujia.Id,
            Sku = "ALB-500G",
            VariantName = "500g",
            WeightInGrams = 500,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            RowVersion = new byte[] { 1 }
        };
        
        await context.ProductVariants.AddRangeAsync(alooBhujiaVariant200g, alooBhujiaVariant500g);
        await context.SaveChangesAsync();
        
        // prices
        await context.Prices.AddRangeAsync(
            new Price
            {
                Id = Guid.NewGuid(),
                VariantId = alooBhujiaVariant200g.Id,
                Currency = "INR",
                Mrp = 60m,
                SellingPrice = 55m,
                GstRateId = gst12.Id,
                EffectiveFrom = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            },
            new Price
            {
                Id = Guid.NewGuid(),
                VariantId = alooBhujiaVariant500g.Id,
                Currency = "INR",
                Mrp = 140m,
                SellingPrice = 130m,
                GstRateId = gst12.Id,
                EffectiveFrom = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            }
        );
        
        // inventory batches
        await context.InventoryBatches.AddRangeAsync(
            new InventoryBatch
            {
                Id = Guid.NewGuid(),
                VariantId = alooBhujiaVariant200g.Id,
                BatchNumber = "BATCH-001",
                QuantityReceived = 500,
                QuantityAvailable = 500,
                QuantityReserved = 0,
                ReceivedAt = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                CreatedAt = DateTime.UtcNow,
                RowVersion = new byte[] { 1 }
            },
            new InventoryBatch
            {
                Id = Guid.NewGuid(),
                VariantId = alooBhujiaVariant500g.Id,
                BatchNumber = "BATCH-002",
                QuantityReceived = 300,
                QuantityAvailable = 300,
                QuantityReserved = 0,
                ReceivedAt = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                CreatedAt = DateTime.UtcNow,
                RowVersion = new byte[] { 1 }
            }
        );
        
        // product 2-12 (abbreviated for brevity - similar structure)
        var products = new[]
        {
            ("Moong Dal", "moong-dal", "crispy fried moong dal", namkeenCategory.Id, (Guid?)haldirams.Id),
            ("Masala Peanuts", "masala-peanuts", "roasted peanuts with spices", nutsCategory.Id, (Guid?)bikano.Id),
            ("Baked Chips", "baked-chips", "healthy baked potato chips", healthyCategory.Id, (Guid?)sunfeast.Id),
            ("Soan Papdi", "soan-papdi", "flaky sweet indian dessert", sweetCategory.Id, (Guid?)haldirams.Id),
            ("Namkeen Mixture", "namkeen-mixture", "mixed savory snacks", namkeenCategory.Id, (Guid?)bikano.Id),
            ("Masala Makhana", "masala-makhana", "roasted fox nuts", healthyCategory.Id, (Guid?)null),
            ("Cheese Balls", "cheese-balls", "crunchy cheese flavored balls", chipsCategory.Id, (Guid?)kurkure.Id),
            ("Cashew Nuts", "cashew-nuts", "premium roasted cashews", nutsCategory.Id, (Guid?)null),
            ("Rusk Toast", "rusk-toast", "crispy wheat rusks", chipsCategory.Id, (Guid?)sunfeast.Id),
            ("Khakhra", "khakhra", "roasted gujarati flatbread", healthyCategory.Id, (Guid?)null)
        };
        
        foreach (var (name, slug, desc, catId, brandId) in products)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = name,
                Slug = slug,
                ShortDescription = desc,
                CategoryId = catId,
                BrandId = brandId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            
            var variant = new ProductVariant
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                Sku = $"{slug.ToUpper()}-250G",
                VariantName = "250g",
                WeightInGrams = 250,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                RowVersion = new byte[] { 1 }
            };
            await context.ProductVariants.AddAsync(variant);
            await context.SaveChangesAsync();
            
            await context.Prices.AddAsync(new Price
            {
                Id = Guid.NewGuid(),
                VariantId = variant.Id,
                Currency = "INR",
                Mrp = 100m,
                SellingPrice = 90m,
                GstRateId = gst12.Id,
                EffectiveFrom = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            });
            
            await context.InventoryBatches.AddAsync(new InventoryBatch
            {
                Id = Guid.NewGuid(),
                VariantId = variant.Id,
                QuantityReceived = 200,
                QuantityAvailable = 200,
                QuantityReserved = 0,
                ReceivedAt = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                CreatedAt = DateTime.UtcNow,
                RowVersion = new byte[] { 1 }
            });
        }
        
        await context.SaveChangesAsync();
    }
    
    private static async Task SeedAdminUserAsync(ApplicationDbContext context)
    {
        if (await context.Users.AnyAsync()) return;
        
        var adminRole = await context.Roles.FirstAsync(r => r.NormalizedName == "ADMIN");
        
        // todo: replace with proper password hashing (bcrypt/argon2)
        // for now using placeholder hash - do not use in production
        var adminUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "admin",
            Email = "admin@sunfead.com",
            Phone = "+919999999999",
            PasswordHash = "PLACEHOLDER_HASH_DO_NOT_USE_IN_PRODUCTION",
            Salt = "PLACEHOLDER_SALT",
            IsEmailConfirmed = true,
            PreferredLanguage = "en",
            CreatedAt = DateTime.UtcNow,
            RowVersion = new byte[] { 1 }
        };
        
        await context.Users.AddAsync(adminUser);
        await context.SaveChangesAsync();
        
        await context.UserRoles.AddAsync(new UserRole
        {
            UserId = adminUser.Id,
            RoleId = adminRole.Id,
            AssignedAt = DateTime.UtcNow
        });
        
        await context.SaveChangesAsync();
    }
}
