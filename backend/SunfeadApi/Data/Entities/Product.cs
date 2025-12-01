using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// product master entity - no pricing or inventory here
/// normalization: pricing lives in price/price_history, inventory in inventory_batch
/// this table contains only product identity and description
/// </summary>
public class Product : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? ShortDescription { get; set; }
    
    // html allowed but sanitize in application layer
    public string? FullDescription { get; set; }
    
    public Guid CategoryId { get; set; }
    public Guid? BrandId { get; set; }
    
    // seo fields
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // navigation properties
    public Category Category { get; set; } = null!;
    public Brand? Brand { get; set; }
    public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
