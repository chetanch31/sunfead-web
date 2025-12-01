namespace SunfeadApi.DTOs;

public record CategoryDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    Guid? ParentId,
    bool IsActive
);

public record CategoryTreeDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    bool IsActive,
    IEnumerable<CategoryTreeDto> Children
);

public record CreateCategoryDto(
    string Name,
    string Slug,
    string? Description,
    Guid? ParentId
);

public record UpdateCategoryDto(
    string Name,
    string? Description,
    Guid? ParentId,
    bool IsActive
);
