using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Repositories.Interfaces;

// user & auth repositories
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> PhoneExistsAsync(string phone, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetUsersWithRolesAsync(CancellationToken cancellationToken = default);
}

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetRolesForUserAsync(Guid userId, CancellationToken cancellationToken = default);
}

public interface IUserRoleRepository : IRepository<UserRole>
{
    Task<bool> UserHasRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserRole>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task RemoveUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);
}

public interface IAddressRepository : IRepository<Address>
{
    Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Address?> GetDefaultAddressAsync(Guid userId, CancellationToken cancellationToken = default);
}
