using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Repositories.Interfaces;
using SunfeadApi.DTOs;
using SunfeadApi.Services.Interfaces;

namespace SunfeadApi.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null) return null;

        var isAdmin = await _unitOfWork.UserRoles.AnyAsync(ur => 
            ur.UserId == user.Id && ur.Role.NormalizedName == "ADMIN");

        return new UserDto(
            user.Id,
            user.Username,
            user.Email,
            user.Phone,
            user.CreatedAt,
            isAdmin
        );
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(email);
        if (user == null) return null;

        var isAdmin = await _unitOfWork.UserRoles.AnyAsync(ur => 
            ur.UserId == user.Id && ur.Role.NormalizedName == "ADMIN");

        return new UserDto(
            user.Id,
            user.Username,
            user.Email,
            user.Phone,
            user.CreatedAt,
            isAdmin
        );
    }

    public async Task<UserDto> CreateUserAsync(RegisterUserDto dto)
    {
        // Check if email exists
        var existingUser = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new InvalidOperationException("Email already exists");

        // Check if username exists
        var usernameExists = await _unitOfWork.Users.AnyAsync(u => u.Username == dto.Username);
        if (usernameExists)
            throw new InvalidOperationException("Username already exists");

        // Hash password with BCrypt
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var salt = BCrypt.Net.BCrypt.GenerateSalt();

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = passwordHash,
            Salt = salt,
            Phone = dto.Phone,
            IsEmailConfirmed = false,
            PreferredLanguage = "en",
            RowVersion = new byte[8]
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return new UserDto(
            user.Id,
            user.Username,
            user.Email,
            user.Phone,
            user.CreatedAt,
            false
        );
    }

    public async Task<UserDto?> AuthenticateUserAsync(LoginDto dto)
    {
        // Try to find user by email first, then by username
        var user = await _unitOfWork.Users.GetByEmailAsync(dto.EmailOrUsername);
        
        if (user == null)
        {
            // If not found by email, try username
            user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Username == dto.EmailOrUsername);
        }
        
        if (user == null) return null;

        // Verify password
        var isValidPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!isValidPassword) return null;

        var isAdmin = await _unitOfWork.UserRoles.AnyAsync(ur => 
            ur.UserId == user.Id && ur.Role.NormalizedName == "ADMIN");

        return new UserDto(
            user.Id,
            user.Username,
            user.Email,
            user.Phone,
            user.CreatedAt,
            isAdmin
        );
    }

    public async Task<IEnumerable<AddressDto>> GetUserAddressesAsync(Guid userId)
    {
        var addresses = await _unitOfWork.Addresses.GetByUserIdAsync(userId);
        
        return addresses.Select(a => new AddressDto(
            a.Id,
            null, // label not in entity
            a.Name,
            a.Phone,
            a.AddressLine1,
            a.AddressLine2,
            a.City,
            a.State,
            a.Pincode,
            a.Country,
            a.IsDefault
        ));
    }

    public async Task<AddressDto> AddAddressAsync(Guid userId, CreateAddressDto dto)
    {
        var address = new Address
        {
            UserId = userId,
            Name = dto.Name,
            Phone = dto.Phone,
            AddressLine1 = dto.AddressLine1,
            AddressLine2 = dto.AddressLine2,
            City = dto.City,
            State = dto.State,
            Pincode = dto.Pincode,
            Country = dto.Country,
            IsDefault = dto.IsDefault
        };

        // If this is set as default, unset other defaults
        if (dto.IsDefault)
        {
            var existingAddresses = await _unitOfWork.Addresses.GetByUserIdAsync(userId);
            foreach (var existing in existingAddresses.Where(a => a.IsDefault))
            {
                existing.IsDefault = false;
                _unitOfWork.Addresses.Update(existing);
            }
        }

        await _unitOfWork.Addresses.AddAsync(address);
        await _unitOfWork.SaveChangesAsync();

        return new AddressDto(
            address.Id,
            null, // label not in entity
            address.Name,
            address.Phone,
            address.AddressLine1,
            address.AddressLine2,
            address.City,
            address.State,
            address.Pincode,
            address.Country,
            address.IsDefault
        );
    }

    public async Task<bool> DeleteAddressAsync(Guid userId, Guid addressId)
    {
        var address = await _unitOfWork.Addresses.GetByIdAsync(addressId);
        if (address == null || address.UserId != userId) return false;

        _unitOfWork.Addresses.Remove(address);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
