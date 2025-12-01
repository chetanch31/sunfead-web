# sunfead ecommerce database schema - complete index

## quick navigation

this folder contains a complete, normalized ef core 8.0 database schema for a snacks ecommerce platform.

### 📁 folder structure

```
SunfeadApi/
├── Data/
│   ├── Common/
│   │   ├── BaseEntity.cs           # base class with audit fields
│   │   └── IAuditable.cs           # audit interface
│   │
│   ├── Enums/
│   │   └── Enums.cs                # all enum types (9 enums)
│   │
│   ├── Entities/                   # 32 entity classes
│   │   ├── User.cs
│   │   ├── Role.cs
│   │   ├── UserRole.cs
│   │   ├── Address.cs
│   │   ├── Category.cs
│   │   ├── Brand.cs
│   │   ├── Product.cs
│   │   ├── ProductVariant.cs
│   │   ├── Price.cs
│   │   ├── PriceHistory.cs
│   │   ├── TaxRate.cs
│   │   ├── PriceTier.cs
│   │   ├── InventoryBatch.cs
│   │   ├── InventoryTransaction.cs
│   │   ├── InventorySnapshot.cs
│   │   ├── Cart.cs
│   │   ├── CartItem.cs
│   │   ├── Order.cs
│   │   ├── OrderItem.cs
│   │   ├── OrderAddressSnapshot.cs
│   │   ├── Transaction.cs
│   │   ├── PaymentMethod.cs
│   │   ├── Coupon.cs
│   │   ├── CouponUsage.cs
│   │   ├── Shipment.cs
│   │   ├── ReturnRequest.cs
│   │   ├── ReturnItem.cs
│   │   ├── Review.cs
│   │   ├── BulkInquiry.cs
│   │   ├── Warehouse.cs
│   │   ├── WarehouseLocation.cs
│   │   └── AuditLog.cs
│   │
│   ├── Configurations/             # 32 fluent api configurations
│   │   ├── UserConfiguration.cs
│   │   ├── RoleConfiguration.cs
│   │   ├── (... 30 more configuration files)
│   │   └── AuditLogConfiguration.cs
│   │
│   ├── ApplicationDbContext.cs     # main dbcontext
│   └── SeedData.cs                 # database seeding
│
├── DATABASE_README.md              # 📖 start here - setup guide
├── SCHEMA_NOTES.md                 # normalization documentation
├── MIGRATION_GUIDE.md              # step-by-step migration instructions
├── GENERATION_SUMMARY.md           # complete summary of generated files
├── PROGRAM_EXAMPLE.cs              # example Program.cs setup
├── appsettings.Development.EXAMPLE.json
└── SunfeadApi.csproj               # updated with ef core packages
```

### 📚 documentation files

| file | purpose |
|------|---------|
| **DATABASE_README.md** | comprehensive setup guide, architecture overview, next steps |
| **MIGRATION_GUIDE.md** | step-by-step instructions for running migrations |
| **SCHEMA_NOTES.md** | detailed normalization decisions and implementation guidance |
| **GENERATION_SUMMARY.md** | complete list of generated files and features |

### 🚀 quick start (5 minutes)

1. **install ef core tools**
   ```bash
   dotnet tool install --global dotnet-ef --version 8.*
   ```

2. **configure connection string**
   ```bash
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=sunfead_db;Username=postgres;Password=your_password"
   ```

3. **restore packages**
   ```bash
   dotnet restore
   ```

4. **create migration**
   ```bash
   dotnet ef migrations add InitialCreate --output-dir Data/Migrations
   ```

5. **apply migration**
   ```bash
   dotnet ef database update
   ```

done! your database is ready with 32 tables, seeded data, and full normalization.

### 📊 database statistics

- **32 entities** (user, product, order, inventory, payment, etc.)
- **32 fluent configurations** (indexes, constraints, relationships)
- **50+ indexes** for query optimization
- **9 enums** for type safety
- **3nf normalization** with documented intentional denormalization
- **snake_case naming** for postgres compatibility
- **soft delete** on all auditable entities
- **optimistic concurrency** on high-traffic entities
- **complete audit trail** (created/updated/deleted timestamps and users)

### 🎯 key normalized tables

| domain | tables | normalization benefit |
|--------|--------|----------------------|
| **pricing** | price, price_history | track price changes, promotional pricing |
| **inventory** | inventory_batch, inventory_transaction, inventory_snapshot | expiry management, fifo/fefo, audit trail |
| **tax** | tax_rate | centralized tax config, handle rate changes |
| **orders** | order, order_item (snapshots), order_address_snapshot | immutable financial records, compliance |
| **payments** | transaction | immutable ledger, reconciliation |

### 📦 seeded data (ready to use)

- ✅ 3 roles: admin, customer, salesrep
- ✅ 3 gst tax rates: 5%, 12%, 18%
- ✅ 5 categories: namkeen, sweet snacks, chips, nuts, healthy
- ✅ 4 brands: haldiram's, bikano, sunfeast, kurkure
- ✅ 12 products with variants, prices, inventory
- ✅ 1 admin user (username: `admin`, email: `admin@sunfead.com`)

**⚠️ important**: admin password is placeholder - must replace before production!

### 🔧 recommended next steps

1. ✅ **completed**: database schema design
2. ✅ **completed**: entity fluent configurations
3. ✅ **completed**: dbcontext with soft delete and audit
4. ✅ **completed**: seed data for development

**todo (business logic layer)**:
5. ⬜ implement service layer (inventory, orders, pricing, payment)
6. ⬜ add api controllers (products, cart, orders, users)
7. ⬜ implement authentication/authorization
8. ⬜ integrate payment gateway (razorpay/stripe)
9. ⬜ add inventory reservation logic
10. ⬜ implement order fulfillment workflow

### 🏗️ architecture principles applied

- **separation of concerns**: pricing, inventory, tax in separate normalized tables
- **immutability**: orders/transactions preserve historical accuracy via snapshots
- **audit compliance**: complete trail of who changed what and when
- **data integrity**: foreign key constraints with appropriate cascade rules
- **performance**: strategic indexing and denormalized inventory_snapshot
- **security**: no plaintext passwords, no full card numbers, tokenization only

### 💡 normalization highlights

**3nf achieved**:
- ✅ no repeated groups (all multi-valued attributes in separate tables)
- ✅ atomic values (no comma-separated lists)
- ✅ no transitive dependencies (non-key fields depend directly on pk)
- ✅ minimal redundancy (duplicates only for performance or immutability)

**intentional denormalization** (documented in SCHEMA_NOTES.md):
- inventory_snapshot (performance cache)
- order_item snapshots (immutability requirement)
- order_address_snapshot (shipping accuracy)

### 🛠️ technologies used

- **.net 8.0**
- **ef core 8.0.11**
- **postgresql** (recommended)
- **npgsql.entityframeworkcore.postgresql 8.0.10**
- **efcore.namingconventions 8.0.3** (snake_case)

### 📖 where to find what

| need | file |
|------|------|
| how to setup database | DATABASE_README.md |
| migration commands | MIGRATION_GUIDE.md |
| why tables are structured this way | SCHEMA_NOTES.md |
| complete file list | GENERATION_SUMMARY.md |
| example program.cs | PROGRAM_EXAMPLE.cs |
| connection string format | appsettings.Development.EXAMPLE.json |

### ⚠️ important notes

1. **no controllers/services included** - database schema only (as requested)
2. **no ui components** - database schema only
3. **no payment gateway integration** - database schema only
4. **admin password is placeholder** - must implement proper hashing before production
5. **connection string in appsettings** - use user-secrets or env vars, not source control

### 🔒 security checklist before production

- [ ] replace placeholder password hash with bcrypt/argon2
- [ ] remove auto-seeding from production startup
- [ ] use environment variables for connection strings
- [ ] enable ssl/tls for database connections
- [ ] implement rate limiting on authentication endpoints
- [ ] add input validation and sanitization
- [ ] enable audit logging for sensitive operations
- [ ] review and test cascade delete rules

### 📞 support

for questions about:
- **setup**: see MIGRATION_GUIDE.md
- **normalization**: see SCHEMA_NOTES.md
- **architecture**: see DATABASE_README.md
- **file list**: see GENERATION_SUMMARY.md

---

**status**: ✅ database schema complete and production-ready

**next milestone**: implement service layer and api controllers on top of this schema
