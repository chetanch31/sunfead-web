namespace SunfeadApi.DTOs;

public record PagedResult<T>(
    IEnumerable<T> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages
);

public record ApiResponse<T>(
    bool Success,
    T? Data,
    string? Message = null,
    IEnumerable<string>? Errors = null
);

public record ApiResponse(
    bool Success,
    string? Message = null,
    IEnumerable<string>? Errors = null
);
