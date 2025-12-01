# schema normalization notes

## database schema normalization approach (3nf)

this document outlines the normalization decisions made in the sunfead ecommerce database schema and explains the rationale behind each choice.

## normalized tables (3nf compliance)

### 1. pricing normalization

**tables**: `price`, `price_history`

**normalization**:
- prices separated from `product_variant` table
- historical price changes tracked in `price_history`
- one active price per variant enforced via unique partial index

**benefits**:
- eliminates repeated price groups (1nf compliance)
- allows promotional pricing without data loss
- supports price audit and compliance requirements
- enables time-based pricing queries

**denormalization choice**:
- `order_item` stores `unit_price_snapshot` and `mrp_snapshot` for immutability
- justified: orders must remain financially accurate even if product prices change

### 2. tax rate normalization

**table**: `tax_rate`

**normalization**:
- centralized tax configuration with effective date ranges
- supports hsn/sac codes for compliance
- referenced by `price` and `price_history`

**benefits**:
- single source of truth for tax rates
- handles tax rate changes over time (e.g., gst rate modifications)
- prevents repeated tax data in every price record

**denormalization choice**:
- `order_item` stores `tax_rate_snapshot_id` (foreign key, not full copy)
- justified: maintains reference to exact tax rate used while keeping order immutable

### 3. inventory batch-level tracking

**tables**: `inventory_batch`, `inventory_transaction`, `inventory_snapshot`

**normalization**:
- inventory separated from `product_variant` into batch-level tracking
- all stock movements recorded as signed transactions
- no denormalized `inventory_count` field in variant

**benefits**:
- enables expiry date management (fifo/fefo allocation)
- complete audit trail for stock movements
- supports multi-warehouse inventory
- batch-level quality control and recalls

**denormalization choice**:
- `inventory_snapshot` table provides aggregated read performance
- justified: complex aggregation queries (sum of batches) would be slow
- implementation: materialized view or scheduled job to refresh snapshots

### 4. order immutability via snapshots

**tables**: `order_item`, `order_address_snapshot`

**normalization**:
- product details (name, sku, variant name) copied to `order_item` at order time
- address copied to `order_address_snapshot` for immutability
- foreign keys to `product_variant` and `tax_rate` retained for analytics

**benefits**:
- orders remain accurate even if products are renamed or deleted
- shipping address preserved even if customer updates their address
- supports legal and financial compliance

**partial denormalization**:
- intentional: orders are immutable business documents
- snapshots are minimal (only display fields, not entire entities)
- foreign keys retained enable post-order analytics without losing referential integrity

### 5. user and address separation

**tables**: `user`, `address`

**normalization**:
- addresses stored in separate table
- one-to-many relationship (user can have multiple addresses)
- nullable `user_id` supports guest checkout

**benefits**:
- addresses reusable across orders
- supports guest orders without creating user accounts
- eliminates address repetition in user table

**note**: application-level constraint recommended to enforce one default address per user (unique index on `user_id, is_default where is_default = true`).

### 6. payment transaction ledger

**table**: `transaction`

**normalization**:
- financial transactions separated from `order` table
- immutable ledger approach
- supports refunds, reversals, chargebacks independently

**benefits**:
- financial data remains intact for audit
- supports multiple payment attempts per order
- enables reconciliation workflows
- complies with financial record retention requirements

### 7. coupon usage tracking

**tables**: `coupon`, `coupon_usage`

**normalization**:
- usage tracking separated into junction table
- tracks per-user and per-order usage

**benefits**:
- prevents coupon abuse (usage limits)
- enables usage analytics
- supports per-user limits without denormalizing coupon data

### 8. warehouse and location

**tables**: `warehouse`, `warehouse_location`

**normalization**:
- warehouse can service multiple pincodes/locations
- one-to-many relationship

**benefits**:
- flexible warehouse service area configuration
- supports location-based inventory allocation

## intentional denormalization

### 1. inventory snapshot table

**rationale**: aggregating inventory from batches and transactions is computationally expensive for real-time queries.

**implementation**: maintain as materialized view or scheduled job. source of truth remains `inventory_batch` and `inventory_transaction`.

**tradeoff**: slight staleness (acceptable for inventory checks) vs real-time performance.

### 2. order item snapshots

**rationale**: legal and financial compliance requires immutable order records.

**implementation**: copy product name, sku, variant name, price to `order_item`. retain foreign keys for analytics.

**tradeoff**: data duplication vs order immutability. justified by business requirement.

### 3. order address snapshot

**rationale**: shipping address must not change if customer updates their profile.

**implementation**: full address text copied to `order_address_snapshot`.

**tradeoff**: address duplication vs order immutability. justified by shipping accuracy.

## where normalization was NOT applied

### cart price snapshot

**table**: `cart_item` references `price.id` via `unit_price_snapshot_id`

**rationale**: avoid price changes during checkout process. cart references active price at time of add-to-cart.

**alternative considered**: fully denormalized price in cart. rejected because price updates (via admin) should reflect in cart before checkout.

### bulk inquiry csv storage

**table**: `bulk_inquiry.csv_file_url`

**rationale**: storing file url (s3/blob) rather than normalizing csv rows into separate table.

**justification**: bulk inquiries are one-time requests, not transactional data requiring normalization. parsing csv on-demand is acceptable.

## normalization best practices applied

1. **no repeated groups**: all multi-valued attributes (addresses, roles, variants) moved to separate tables
2. **atomic values**: all fields contain single values, no comma-separated lists
3. **no transitive dependencies**: all non-key fields depend directly on primary key
4. **referential integrity**: all foreign keys enforced with appropriate cascade/restrict rules
5. **minimal redundancy**: duplicated data limited to performance optimization (snapshots) or immutability requirements (orders)

## recommended next steps

### 1. implement inventory snapshot refresh

create postgres materialized view:
```sql
create materialized view inventory_snapshots as
select 
    variant_id,
    sum(quantity_available) as total_available_qty,
    sum(quantity_reserved) as total_reserved_qty,
    now() as last_updated_at
from inventory_batches
group by variant_id;

create unique index on inventory_snapshots (variant_id);
```

refresh strategy:
- on-demand: `refresh materialized view inventory_snapshots;`
- scheduled: cron job every 5-15 minutes
- trigger-based: refresh on inventory_batch or inventory_transaction changes (postgres only)

### 2. enforce default address constraint

application-level transaction:
```csharp
public async Task SetDefaultAddress(Guid userId, Guid addressId)
{
    using var transaction = await context.Database.BeginTransactionAsync();
    
    // unset all other defaults
    await context.Addresses
        .Where(a => a.UserId == userId && a.IsDefault)
        .ExecuteUpdateAsync(a => a.SetProperty(x => x.IsDefault, false));
    
    // set new default
    var address = await context.Addresses.FindAsync(addressId);
    address.IsDefault = true;
    
    await context.SaveChangesAsync();
    await transaction.CommitAsync();
}
```

### 3. price history audit trigger

automatically copy price to price_history when price.effective_to is set:

```csharp
// in service layer when updating price
public async Task UpdatePrice(Guid variantId, decimal newSellingPrice)
{
    var currentPrice = await context.Prices
        .FirstAsync(p => p.VariantId == variantId && p.EffectiveTo == null);
    
    // close current price
    currentPrice.EffectiveTo = DateTime.UtcNow;
    
    // archive to history
    context.PriceHistories.Add(new PriceHistory
    {
        VariantId = currentPrice.VariantId,
        Mrp = currentPrice.Mrp,
        SellingPrice = currentPrice.SellingPrice,
        GstRateId = currentPrice.GstRateId,
        EffectiveFrom = currentPrice.EffectiveFrom,
        EffectiveTo = currentPrice.EffectiveTo
    });
    
    // create new active price
    context.Prices.Add(new Price
    {
        VariantId = variantId,
        Mrp = currentPrice.Mrp,
        SellingPrice = newSellingPrice,
        GstRateId = currentPrice.GstRateId,
        EffectiveFrom = DateTime.UtcNow
    });
    
    await context.SaveChangesAsync();
}
```

### 4. inventory fifo allocation

implement batch selection for order fulfillment:

```csharp
public async Task<List<InventoryBatch>> AllocateStock(Guid variantId, int requiredQty)
{
    var batches = await context.InventoryBatches
        .Where(b => b.VariantId == variantId && b.QuantityAvailable > 0)
        .OrderBy(b => b.ExpiryDate) // fifo by expiry
        .ThenBy(b => b.ReceivedAt) // then by receive date
        .ToListAsync();
    
    var allocated = new List<InventoryBatch>();
    var remaining = requiredQty;
    
    foreach (var batch in batches)
    {
        if (remaining <= 0) break;
        
        var allocateQty = Math.Min(batch.QuantityAvailable, remaining);
        batch.QuantityReserved += allocateQty;
        batch.QuantityAvailable -= allocateQty;
        
        allocated.Add(batch);
        remaining -= allocateQty;
    }
    
    if (remaining > 0)
        throw new InsufficientStockException();
    
    await context.SaveChangesAsync();
    return allocated;
}
```

## anti-patterns avoided

1. **eavatribute tables**: no generic key-value storage for product attributes
2. **god tables**: no single products table with all pricing, inventory, tax in one row
3. **comma-separated values**: no lists stored as strings (e.g., "tag1,tag2,tag3")
4. **excessive joins**: snapshot tables balance between normalization and query performance
5. **nullable foreign keys overuse**: nullability used only where business logic requires (guest users, optional brand)

## conclusion

this schema achieves 3nf where practical while making intentional denormalization choices for:
- performance (inventory_snapshot)
- immutability (order snapshots)
- compliance (financial transaction ledger)

all denormalization is documented and justified by business requirements. the schema provides a solid foundation for an ecommerce platform with proper audit trails, historical tracking, and data integrity.
