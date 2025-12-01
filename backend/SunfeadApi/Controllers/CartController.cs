using Microsoft.AspNetCore.Mvc;
using SunfeadApi.DTOs;
using SunfeadApi.Services.Interfaces;

namespace SunfeadApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    // Note: In production, userId should come from authenticated user context
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<ApiResponse<CartDto>>> GetCart(Guid userId)
    {
        try
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null)
                return NotFound(new ApiResponse<CartDto>(false, null, "Cart not found"));

            return Ok(new ApiResponse<CartDto>(true, cart));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<CartDto>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpPost("{userId:guid}/items")]
    public async Task<ActionResult<ApiResponse<CartDto>>> AddToCart(Guid userId, [FromBody] AddToCartDto dto)
    {
        try
        {
            var cart = await _cartService.AddToCartAsync(userId, dto);
            return Ok(new ApiResponse<CartDto>(true, cart, "Item added to cart"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<CartDto>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpPut("{userId:guid}/items/{cartItemId:guid}")]
    public async Task<ActionResult<ApiResponse<CartDto>>> UpdateCartItem(Guid userId, Guid cartItemId, [FromBody] UpdateCartItemDto dto)
    {
        try
        {
            var cart = await _cartService.UpdateCartItemAsync(userId, cartItemId, dto);
            if (cart == null)
                return NotFound(new ApiResponse<CartDto>(false, null, "Cart or item not found"));

            return Ok(new ApiResponse<CartDto>(true, cart, "Cart item updated"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<CartDto>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpDelete("{userId:guid}/items/{cartItemId:guid}")]
    public async Task<ActionResult<ApiResponse>> RemoveCartItem(Guid userId, Guid cartItemId)
    {
        try
        {
            var result = await _cartService.RemoveCartItemAsync(userId, cartItemId);
            if (!result)
                return NotFound(new ApiResponse(false, "Cart or item not found"));

            return Ok(new ApiResponse(true, "Item removed from cart"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse(false, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpDelete("{userId:guid}")]
    public async Task<ActionResult<ApiResponse>> ClearCart(Guid userId)
    {
        try
        {
            var result = await _cartService.ClearCartAsync(userId);
            if (!result)
                return NotFound(new ApiResponse(false, "Cart not found"));

            return Ok(new ApiResponse(true, "Cart cleared"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse(false, "An error occurred", new[] { ex.Message }));
        }
    }
}
