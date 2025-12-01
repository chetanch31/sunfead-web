using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Repositories.Interfaces;
using SunfeadApi.DTOs;
using SunfeadApi.Services.Interfaces;

namespace SunfeadApi.Services;

public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;

    public CartService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CartDto?> GetCartByUserIdAsync(Guid userId)
    {
        var cart = await _unitOfWork.Carts.GetActiveCartByUserIdAsync(userId);
        if (cart == null) return null;

        var items = await _unitOfWork.CartItems.GetByCartIdAsync(cart.Id);
        
        var itemDtos = new List<CartItemDto>();
        decimal subTotal = 0;

        foreach (var item in items)
        {
            var variant = await _unitOfWork.ProductVariants.GetByIdAsync(item.VariantId);
            var product = variant != null ? await _unitOfWork.Products.GetByIdAsync(variant.ProductId) : null;
            var price = variant != null ? await _unitOfWork.Prices.GetActivePrice(variant.Id) : null;

            var unitPrice = price?.SellingPrice ?? 0;
            var totalPrice = unitPrice * item.Qty;
            subTotal += totalPrice;

            itemDtos.Add(new CartItemDto(
                item.Id,
                item.VariantId,
                product?.Name ?? "Unknown",
                variant?.Sku ?? "Unknown",
                item.Qty,
                unitPrice,
                totalPrice
            ));
        }

        return new CartDto(
            cart.Id,
            cart.UserId ?? Guid.Empty,
            itemDtos,
            subTotal,
            items.Sum(i => i.Qty),
            cart.CreatedAt
        );
    }

    public async Task<CartDto> AddToCartAsync(Guid userId, AddToCartDto dto)
    {
        // Get or create cart
        var cart = await _unitOfWork.Carts.GetActiveCartByUserIdAsync(userId);
        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                IsActive = true
            };
            await _unitOfWork.Carts.AddAsync(cart);
            await _unitOfWork.SaveChangesAsync();
        }

        // Check if item already exists
        var existingItem = await _unitOfWork.CartItems.GetByCartAndVariantAsync(cart.Id, dto.VariantId);
        
        if (existingItem != null)
        {
            existingItem.Qty += dto.Quantity;
            _unitOfWork.CartItems.Update(existingItem);
        }
        else
        {
            // Get current price for snapshot
            var currentPrice = await _unitOfWork.Prices.GetActivePrice(dto.VariantId);
            if (currentPrice == null)
                throw new InvalidOperationException("No active price found for variant");

            var cartItem = new CartItem
            {
                CartId = cart.Id,
                VariantId = dto.VariantId,
                Qty = dto.Quantity,
                UnitPriceSnapshotId = currentPrice.Id,
                AddedAt = DateTime.UtcNow
            };
            await _unitOfWork.CartItems.AddAsync(cartItem);
        }

        await _unitOfWork.SaveChangesAsync();

        return (await GetCartByUserIdAsync(userId))!;
    }

    public async Task<CartDto?> UpdateCartItemAsync(Guid userId, Guid cartItemId, UpdateCartItemDto dto)
    {
        var cart = await _unitOfWork.Carts.GetActiveCartByUserIdAsync(userId);
        if (cart == null) return null;

        var cartItem = await _unitOfWork.CartItems.GetByIdAsync(cartItemId);
        if (cartItem == null || cartItem.CartId != cart.Id) return null;

        if (dto.Quantity <= 0)
        {
            _unitOfWork.CartItems.Remove(cartItem);
        }
        else
        {
            cartItem.Qty = dto.Quantity;
            _unitOfWork.CartItems.Update(cartItem);
        }

        await _unitOfWork.SaveChangesAsync();

        return await GetCartByUserIdAsync(userId);
    }

    public async Task<bool> RemoveCartItemAsync(Guid userId, Guid cartItemId)
    {
        var cart = await _unitOfWork.Carts.GetActiveCartByUserIdAsync(userId);
        if (cart == null) return false;

        var cartItem = await _unitOfWork.CartItems.GetByIdAsync(cartItemId);
        if (cartItem == null || cartItem.CartId != cart.Id) return false;

        _unitOfWork.CartItems.Remove(cartItem);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ClearCartAsync(Guid userId)
    {
        var cart = await _unitOfWork.Carts.GetActiveCartByUserIdAsync(userId);
        if (cart == null) return false;

        var items = await _unitOfWork.CartItems.GetByCartIdAsync(cart.Id);
        _unitOfWork.CartItems.RemoveRange(items);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
