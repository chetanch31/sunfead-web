namespace SunfeadApi.Data.Entities;

/// <summary>
/// many-to-many junction for user roles
/// composite primary key: user_id + role_id
/// </summary>
public class UserRole
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    
    // navigation properties
    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
}
