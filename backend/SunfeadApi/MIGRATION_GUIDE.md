# migration and setup quick start

## prerequisites

- .net 8.0 sdk installed
- postgresql 13+ running locally or accessible remotely
- ef core tools installed globally

## step-by-step setup

### step 1: install ef core tools (if not already installed)

```bash
dotnet tool install --global dotnet-ef --version 8.*
```

verify installation:
```bash
dotnet ef --version
```

### step 2: configure database connection

**option a: using user-secrets (recommended for development)**

```bash
cd SunfeadApi
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=sunfead_db;Username=postgres;Password=your_actual_password"
```

**option b: using appsettings.Development.json (not recommended - avoid committing passwords)**

create or edit `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=sunfead_db;Username=postgres;Password=your_password"
  }
}
```

then add to `.gitignore`:
```
appsettings.Development.json
```

### step 3: update program.cs

replace the existing `Program.cs` content with the code from `PROGRAM_EXAMPLE.cs` or manually add:

```csharp
using Microsoft.EntityFrameworkCore;
using SunfeadApi.Data;

var builder = WebApplication.CreateBuilder(args);

// add dbcontext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ... rest of your code

// seed database (development only)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
    await SeedData.SeedAsync(context);
}
```

### step 4: restore nuget packages

```bash
dotnet restore
```

this will install:
- Npgsql.EntityFrameworkCore.PostgreSQL
- EFCore.NamingConventions
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.Tools

### step 5: create initial migration

```bash
dotnet ef migrations add InitialCreate --output-dir Data/Migrations
```

this generates:
- `Data/Migrations/[timestamp]_InitialCreate.cs` - migration file
- `Data/Migrations/ApplicationDbContextModelSnapshot.cs` - model snapshot

### step 6: review generated migration (optional)

open the migration file and verify tables are created with snake_case naming:
- `users`, `products`, `product_variants`, `inventory_batches`, etc.

### step 7: apply migration to database

```bash
dotnet ef database update
```

this will:
1. create database if it doesn't exist
2. apply all pending migrations
3. create all 32 tables with indexes and constraints

### step 8: verify database

connect to postgres and verify schema:

```bash
psql -h localhost -U postgres -d sunfead_db
```

inside psql:
```sql
-- list all tables
\dt

-- view table structure
\d users
\d products
\d inventory_batches

-- check seeded data
select * from roles;
select * from categories;
select * from products;
```

### step 9: run application

```bash
dotnet run
```

the application will:
1. apply any pending migrations
2. seed initial data (if development environment)
3. start api server

access swagger ui at: `https://localhost:5001/swagger`

## common issues and fixes

### issue: "password authentication failed"

**fix**: verify postgres connection string credentials

```bash
# test connection manually
psql -h localhost -U postgres -d sunfead_db
```

### issue: "database sunfead_db does not exist"

**fix**: create database manually or let ef create it

```bash
psql -h localhost -U postgres
create database sunfead_db;
\q
```

then run `dotnet ef database update` again

### issue: "UseSnakeCaseNamingConvention not found"

**fix**: ensure EFCore.NamingConventions package is installed

```bash
dotnet add package EFCore.NamingConventions --version 8.0.3
dotnet restore
```

### issue: migration file shows pascal case table names

**fix**: delete migration and recreate after installing EFCore.NamingConventions

```bash
dotnet ef migrations remove
dotnet restore
dotnet ef migrations add InitialCreate --output-dir Data/Migrations
```

### issue: seed data not populating

**fix**: check if Program.cs has seeding code and environment is Development

```bash
# run with development environment
dotnet run --environment Development
```

### issue: duplicate key violations during seed

**fix**: seed data is idempotent but if tables have existing data:

```sql
-- clear all tables (careful - deletes data)
truncate table users, roles, user_roles, products, product_variants, 
  prices, inventory_batches, categories, brands, tax_rates cascade;
```

then run application again to reseed

## migration commands reference

### create new migration
```bash
dotnet ef migrations add <MigrationName> --output-dir Data/Migrations
```

### apply all pending migrations
```bash
dotnet ef database update
```

### rollback to specific migration
```bash
dotnet ef database update <MigrationName>
```

### rollback all migrations
```bash
dotnet ef database update 0
```

### remove last migration (if not applied)
```bash
dotnet ef migrations remove
```

### generate sql script for review
```bash
dotnet ef migrations script --output migration.sql
```

### generate idempotent script (safe to run multiple times)
```bash
dotnet ef migrations script --idempotent --output migration_idempotent.sql
```

### list all migrations
```bash
dotnet ef migrations list
```

## production deployment

for production, **do not use auto-migration or seeding** in application startup.

instead:

1. **generate idempotent sql script**:
```bash
dotnet ef migrations script --idempotent --output deploy.sql
```

2. **review script manually**

3. **apply via dba or deployment pipeline**:
```bash
psql -h production-host -U dbuser -d sunfead_db -f deploy.sql
```

4. **remove seeding code from production**:
```csharp
// in Program.cs - only seed in development
if (app.Environment.IsDevelopment())
{
    // seeding code here
}
```

5. **use environment variables for connection string**:
```bash
export ConnectionStrings__DefaultConnection="Host=prod-db;Database=sunfead_db;..."
```

## database backup and restore

### backup
```bash
pg_dump -h localhost -U postgres -d sunfead_db -F c -f sunfead_backup.dump
```

### restore
```bash
pg_restore -h localhost -U postgres -d sunfead_db -c sunfead_backup.dump
```

## performance monitoring

### enable ef core sql logging (development only)

in `appsettings.Development.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  }
}
```

### analyze slow queries

```sql
-- enable query logging in postgres
alter database sunfead_db set log_min_duration_statement = 100;

-- view slow queries
select query, mean_exec_time, calls 
from pg_stat_statements 
order by mean_exec_time desc 
limit 10;
```

## next steps after setup

1. verify all 32 tables created with snake_case naming
2. check seed data populated (3 roles, 5 categories, 12 products, etc.)
3. test admin login with credentials from seed data
4. implement service layer for business logic
5. add api controllers for product catalog, cart, orders
6. implement authentication and authorization
7. add payment gateway integration
8. implement inventory reservation and order fulfillment workflows

---

**you're now ready to start building the business logic layer on top of this normalized database schema!**
