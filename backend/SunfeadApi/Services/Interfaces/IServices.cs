using SunfeadApi.DTOs;

namespace SunfeadApi.Services.Interfaces;

public interface IProductService
{
    Task<PagedResult<ProductDto>> GetProductsAsync(int page, int pageSize, bool? isActive = null, Guid? categoryId = null);
    Task<ProductDetailDto?> GetProductByIdAsync(Guid id);
    Task<ProductDetailDto?> GetProductBySlugAsync(string slug);
    Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(Guid categoryId);
    Task<ProductDetailDto> CreateProductAsync(CreateProductDto dto);
    Task<ProductDetailDto?> UpdateProductAsync(Guid id, UpdateProductDto dto);
    Task<bool> DeleteProductAsync(Guid id);
    Task<ProductVariantDto> AddVariantAsync(Guid productId, CreateProductVariantDto dto);
    Task<int> GetAvailableStockAsync(Guid variantId);
}

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    Task<IEnumerable<CategoryTreeDto>> GetCategoryTreeAsync();
    Task<CategoryDto?> GetCategoryByIdAsync(Guid id);
    Task<CategoryDto?> GetCategoryBySlugAsync(string slug);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
    Task<CategoryDto?> UpdateCategoryAsync(Guid id, UpdateCategoryDto dto);
    Task<bool> DeleteCategoryAsync(Guid id);
}

public interface ICartService
{
    Task<CartDto?> GetCartByUserIdAsync(Guid userId);
    Task<CartDto> AddToCartAsync(Guid userId, AddToCartDto dto);
    Task<CartDto?> UpdateCartItemAsync(Guid userId, Guid cartItemId, UpdateCartItemDto dto);
    Task<bool> RemoveCartItemAsync(Guid userId, Guid cartItemId);
    Task<bool> ClearCartAsync(Guid userId);
}

public interface IOrderService
{
    Task<PagedResult<OrderSummaryDto>> GetUserOrdersAsync(Guid userId, int page, int pageSize);
    Task<OrderDto?> GetOrderByIdAsync(Guid orderId, Guid userId);
    Task<OrderDto> CreateOrderAsync(Guid userId, CreateOrderDto dto);
    Task<bool> CancelOrderAsync(Guid orderId, Guid userId);
}

public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(Guid id);
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<UserDto> CreateUserAsync(RegisterUserDto dto);
    Task<UserDto?> AuthenticateUserAsync(LoginDto dto);
    Task<IEnumerable<AddressDto>> GetUserAddressesAsync(Guid userId);
    Task<AddressDto> AddAddressAsync(Guid userId, CreateAddressDto dto);
    Task<bool> DeleteAddressAsync(Guid userId, Guid addressId);
}
