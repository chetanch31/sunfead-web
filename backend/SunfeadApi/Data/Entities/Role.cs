namespace SunfeadApi.Data.Entities;

/// <summary>
/// role for authorization
/// </summary>
public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string NormalizedName { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // navigation properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
