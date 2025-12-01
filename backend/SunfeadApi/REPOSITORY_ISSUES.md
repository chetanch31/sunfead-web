# Repository Pattern - Property Name Reference

Due to snake_case naming convention in the database and entities, many repository-specific methods reference properties that don't exist. The repository files have been created but require manual correction of property names to match the actual entity definitions.

## Known Issues

The repository implementations reference properties using PascalCase that don't exist in the entities. You'll need to check each entity file and use the correct property names.

### Common Mismatches

- `OrderDate` → Use `CreatedAt` (from BaseEntity)
- `TransactionDate` → Use `CreatedAt` (from BaseEntity  
- `RequestDate` → Use `CreatedAt` (from BaseEntity)
- `InquiryDate` → Use `CreatedAt` (from BaseEntity)
- `ShippedDate` → Check Shipment entity for actual property name
- `SnapshotDate` → Check InventorySnapshot entity for actual property name
- `ChangedAt` → Check PriceHistory entity for actual property name
- `ReferenceNumber` → Check Transaction entity for actual property name
- `UserId` in Transaction → Check if Transaction entity has UserId
- `IsConverted` in Cart → Check Cart entity for actual property name
- `QtyAvailable` in InventoryBatch → Check for actual property name (might be `QuantityAvailable`)
- `LotNumber` in InventoryBatch → Check for actual property name
- Many other property name mismatches

## Recommendation

For now, use the generic `IRepository<TEntity>` directly and write LINQ queries in your services. The specific repository methods can be added later once you verify the exact property names from each entity file.

Example service usage:

```csharp
public class ProductService  
{
    private readonly IRepository<Product> _productRepo;
    
    public async Task<Product?> GetBySlugAsync(string slug)
    {
        return await _productRepo.FirstOrDefaultAsync(p => p.Slug == slug);
    }
}
```

This approach is cleaner until all entity property names are verified and repository methods are corrected.
