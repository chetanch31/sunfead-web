using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// tax rate master (gst/vat)
/// normalization: centralized tax configuration with effective date range support
/// allows historical tax rate changes and hsn/sac code tracking
/// </summary>
public class TaxRate : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!; // e.g. "GST 5%", "GST 12%"
    public decimal RatePercent { get; set; }
    public string? HsnSacCode { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    
    // navigation properties
    public ICollection<Price> Prices { get; set; } = new List<Price>();
    public ICollection<PriceHistory> PriceHistories { get; set; } = new List<PriceHistory>();
}
