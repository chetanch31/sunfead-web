# sunfead ecommerce database schema

normalized ef core database schema for snacks ecommerce platform.

## overview

this project contains a fully normalized (3nf) database schema designed for an ecommerce platform specializing in snacks. the schema separates concerns properly: pricing history, inventory batches, tax rates, and order snapshots are all normalized to maintain data integrity and historical accuracy.

## key normalization decisions

### pricing separated from product
- **tables**: `price`, `price_history`
- **rationale**: allows tracking price changes over time for promotions, compliance, and analytics. one active price per variant with full history preserved.

### inventory batch-level tracking
- **tables**: `inventory_batch`, `inventory_transaction`, `inventory_snapshot`
- **rationale**: enables expiry management (fifo/fefo), batch tracking, and proper audit trail. inventory_snapshot provides denormalized read performance while actual source of truth lives in batches + transactions.

### tax rate master table
- **table**: `tax_rate`
- **rationale**: centralizes tax configuration with effective date ranges. supports gst/vat changes over time and hsn/sac code compliance.

### order immutability via snapshots
- **tables**: `order_item` (with snapshots), `order_address_snapshot`
- **rationale**: preserves historical accuracy. product names, skus, prices, and addresses snapshot at order time. foreign keys to variant retained for analytics.

### financial data isolation
- **table**: `transaction`
- **rationale**: immutable payment ledger separated from orders. supports refunds, reversals, and reconciliation independently.

## technology stack

- **.net**: 8.0
- **ef core**: 8.0.x
- **database**: postgresql (recommended for production - supports jsonb for raw_payload fields)
- **naming convention**: snake_case via EFCore.NamingConventions

## setup instructions

### 1. install ef core tools

```bash
dotnet tool install --global dotnet-ef --version 8.*
```

### 2. configure connection string

add your postgres connection string to `appsettings.json` or `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=sunfead_db;Username=postgres;Password=your_password"
  }
}
```

**important**: never commit connection strings with real passwords to source control. use environment variables or dotnet user-secrets:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=sunfead_db;Username=postgres;Password=your_password"
```

### 3. register dbcontext in program.cs

add to your `Program.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using SunfeadApi.Data;

var builder = WebApplication.CreateBuilder(args);

// add dbcontext with postgres provider and snake_case naming
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseSnakeCaseNamingConvention());

// ... rest of your services

var app = builder.Build();

// optional: seed database on startup (development only)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
    await SeedData.SeedAsync(context);
}

app.Run();
```

### 4. create initial migration

```bash
dotnet ef migrations add InitialCreate --output-dir Data/Migrations
```

### 5. apply migration and seed database

```bash
dotnet ef database update
```

the seed data will populate:
- 3 roles (admin, customer, salesrep)
- 3 gst tax rates (5%, 12%, 18%)
- 5 product categories
- 4 brands
- 12 sample products with variants, prices, and inventory batches
- 1 admin user (username: `admin`, email: `admin@sunfead.com`)

**security note**: the seeded admin password is a placeholder stub. before production:
1. implement proper password hashing (bcrypt or argon2)
2. change the admin password immediately
3. consider removing auto-seeding from production deployments

### 6. verify database

connect to your postgres database and check tables:

```sql
\dt  -- list all tables
select * from users;
select * from products;
select * from inventory_snapshots;
```

## schema highlights

### soft delete
all entities inheriting from `BaseEntity` implement soft delete via `is_deleted` flag. global query filter automatically excludes deleted records.

### optimistic concurrency
entities with frequent concurrent updates include `row_version` timestamp:
- `user`
- `product_variant`
- `inventory_batch`
- `order`

### audit fields
all major entities track:
- `created_at`, `created_by`
- `updated_at`, `updated_by`
- `deleted_at`, `deleted_by`

todo: wire up current user context to populate `*_by` fields automatically.

### indexes
comprehensive indexing on:
- unique constraints: product.slug, variant.sku, coupon.code, order.order_number
- foreign keys: all relationships indexed
- composite indexes: status+date for orders/transactions, variant+expiry for inventory
- search optimization: product.name, category.slug, addresses.pincode

### inventory snapshot
`inventory_snapshot` table is designed as a denormalized read cache. two implementation options:

**option 1: postgres materialized view**
```sql
create materialized view inventory_snapshots as
select 
    variant_id,
    sum(quantity_available) as total_available_qty,
    sum(quantity_reserved) as total_reserved_qty,
    now() as last_updated_at
from inventory_batches
group by variant_id;

-- refresh periodically
refresh materialized view inventory_snapshots;
```

**option 2: background job**
implement a scheduled task (hangfire/quartz) to recalculate snapshot from batches + transactions.

## next steps

### business logic implementation

the schema is complete but requires application-layer logic:

1. **inventory reservation**: implement atomic reserve/release during checkout (service layer transaction)
2. **payment processing**: integrate payment gateway (razorpay/stripe) and update transaction records
3. **order fulfillment**: implement state machine for order status transitions
4. **pricing engine**: calculate bulk discounts from price_tiers, apply coupons
5. **tax calculation**: use tax_rate to compute order tax amounts
6. **stock allocation**: implement fifo/fefo logic using inventory_batch.expiry_date

### recommended architecture

```
controllers/ (api endpoints - not included)
  ├─ ProductsController
  ├─ OrdersController
  └─ CartController
  
services/ (business logic - not included)
  ├─ IInventoryService - reservation, allocation, stock checks
  ├─ IOrderService - checkout, fulfillment, cancellation
  ├─ IPricingService - price calculation, tier discounts, coupons
  └─ IPaymentService - gateway integration, transaction recording
  
repositories/ (optional - ef core dbsets can be used directly)
  └─ consider repository pattern for complex queries
```

### testing

recommend integration tests with testcontainers:

```bash
dotnet add package Testcontainers.PostgreSql
```

example test setup:
```csharp
var container = new PostgreSqlBuilder().Build();
await container.StartAsync();

var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseNpgsql(container.GetConnectionString())
    .Options;

using var context = new ApplicationDbContext(options);
await context.Database.MigrateAsync();
// run tests
```

## troubleshooting

### migration fails with naming convention errors
ensure `EFCore.NamingConventions` package is installed and `UseSnakeCaseNamingConvention()` is called in ApplicationDbContext.

### unique constraint violations on price table
the filter index `(variant_id, effective_to) where effective_to is null` requires postgres 9.5+. ensure your database supports partial unique indexes.

### soft delete not working
verify global query filter is applied in `OnModelCreating`. use `IgnoreQueryFilters()` in linq queries when you need to include deleted records.

## migration commands reference

```bash
# create new migration
dotnet ef migrations add <MigrationName>

# apply pending migrations
dotnet ef database update

# rollback to specific migration
dotnet ef database update <MigrationName>

# remove last migration (if not applied)
dotnet ef migrations remove

# generate sql script
dotnet ef migrations script

# generate idempotent script (safe to run multiple times)
dotnet ef migrations script --idempotent
```

## additional resources

- [ef core documentation](https://docs.microsoft.com/en-us/ef/core/)
- [postgres jsonb type](https://www.postgresql.org/docs/current/datatype-json.html)
- [ef core global query filters](https://docs.microsoft.com/en-us/ef/core/querying/filters)

## license

proprietary - sunfead ecommerce platform
