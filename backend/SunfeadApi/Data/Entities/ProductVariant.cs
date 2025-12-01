using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// product variant entity - sku-level item
/// normalization: pricing separated into price table, inventory into inventory_batch
/// optimistic concurrency for concurrent updates
/// </summary>
public class ProductVariant : BaseEntity
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Sku { get; set; } = null!;
    public string VariantName { get; set; } = null!; // e.g. "200g", "500g"
    public int WeightInGrams { get; set; }
    public string? Barcode { get; set; }
    public bool IsActive { get; set; } = true;
    
    // optimistic concurrency token
    public byte[] RowVersion { get; set; } = null!;
    
    // navigation properties
    public Product Product { get; set; } = null!;
    public ICollection<Price> Prices { get; set; } = new List<Price>();
    public ICollection<PriceHistory> PriceHistories { get; set; } = new List<PriceHistory>();
    public ICollection<InventoryBatch> InventoryBatches { get; set; } = new List<InventoryBatch>();
    public ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<PriceTier> PriceTiers { get; set; } = new List<PriceTier>();
}
