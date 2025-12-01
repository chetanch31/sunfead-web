using Microsoft.AspNetCore.Mvc;
using SunfeadApi.DTOs;
using SunfeadApi.Services.Interfaces;

namespace SunfeadApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // Note: In production, userId should come from authenticated user context
    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<ApiResponse<PagedResult<OrderSummaryDto>>>> GetUserOrders(
        Guid userId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        try
        {
            var orders = await _orderService.GetUserOrdersAsync(userId, page, pageSize);
            return Ok(new ApiResponse<PagedResult<OrderSummaryDto>>(true, orders));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<PagedResult<OrderSummaryDto>>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpGet("{orderId:guid}")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> GetOrder(Guid orderId, [FromQuery] Guid userId)
    {
        try
        {
            var order = await _orderService.GetOrderByIdAsync(orderId, userId);
            if (order == null)
                return NotFound(new ApiResponse<OrderDto>(false, null, "Order not found"));

            return Ok(new ApiResponse<OrderDto>(true, order));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<OrderDto>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpPost("user/{userId:guid}")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> CreateOrder(Guid userId, [FromBody] CreateOrderDto dto)
    {
        try
        {
            var order = await _orderService.CreateOrderAsync(userId, dto);
            return CreatedAtAction(nameof(GetOrder), new { orderId = order.Id, userId }, new ApiResponse<OrderDto>(true, order, "Order created successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<OrderDto>(false, null, ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<OrderDto>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpPost("{orderId:guid}/cancel")]
    public async Task<ActionResult<ApiResponse>> CancelOrder(Guid orderId, [FromQuery] Guid userId)
    {
        try
        {
            var result = await _orderService.CancelOrderAsync(orderId, userId);
            if (!result)
                return BadRequest(new ApiResponse(false, "Order cannot be cancelled"));

            return Ok(new ApiResponse(true, "Order cancelled successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse(false, "An error occurred", new[] { ex.Message }));
        }
    }
}
