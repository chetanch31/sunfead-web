using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// brand entity for product manufacturer/brand
/// </summary>
public class Brand : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    
    // navigation properties
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
