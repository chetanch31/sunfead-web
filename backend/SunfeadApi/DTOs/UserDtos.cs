namespace SunfeadApi.DTOs;

public record UserDto(
    Guid Id,
    string Username,
    string Email,
    string? Phone,
    DateTime CreatedAt,
    bool IsAdmin = false
);

public record RegisterUserDto(
    string Username,
    string Email,
    string Password,
    string? Phone
);

public record LoginDto(
    string EmailOrUsername,
    string Password
);

public record AddressDto(
    Guid Id,
    string? Label,
    string Name,
    string Phone,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string Pincode,
    string Country,
    bool IsDefault
);

public record CreateAddressDto(
    string? Label,
    string Name,
    string Phone,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string Pincode,
    string Country,
    bool IsDefault
);
