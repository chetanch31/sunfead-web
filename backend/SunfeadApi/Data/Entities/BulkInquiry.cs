using SunfeadApi.Data.Common;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// bulk order inquiry for corporate/wholesale customers
/// </summary>
public class BulkInquiry : BaseEntity
{
    public Guid Id { get; set; }
    
    // nullable for anonymous inquiries
    public Guid? UserId { get; set; }
    
    public string ContactName { get; set; } = null!;
    public string? CompanyName { get; set; }
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Gstin { get; set; }
    
    public DateTime? RequiredDate { get; set; }
    public string? Pincode { get; set; }
    
    // url to uploaded csv file with product list
    public string? CsvFileUrl { get; set; }
    
    public int? EstimatedQty { get; set; }
    public string? PackagingPreference { get; set; }
    public decimal? BudgetPerUnit { get; set; }
    public string? Message { get; set; }
    
    public BulkInquiryStatus Status { get; set; } = BulkInquiryStatus.New;
    
    // assigned sales rep
    public Guid? AssignedTo { get; set; }
    
    // navigation properties
    public User? User { get; set; }
}
