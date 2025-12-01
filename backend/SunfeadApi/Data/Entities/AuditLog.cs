using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// audit log for entity changes
/// stores generic change tracking for compliance and debugging
/// </summary>
public class AuditLog : BaseEntity
{
    public Guid Id { get; set; }
    public string EntityName { get; set; } = null!;
    public string EntityId { get; set; } = null!;
    public string Action { get; set; } = null!; // "create", "update", "delete"
    
    // nullable for system-initiated changes
    public Guid? PerformedBy { get; set; }
    
    // json representation of changes (before/after values)
    public string? ChangesJson { get; set; }
}
