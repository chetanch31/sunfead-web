using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Enums;
using SunfeadApi.Data.Repositories.Interfaces;
using SunfeadApi.DTOs;
using SunfeadApi.Services.Interfaces;

namespace SunfeadApi.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<OrderSummaryDto>> GetUserOrdersAsync(Guid userId, int page, int pageSize)
    {
        var (orders, total) = await _unitOfWork.Orders.GetPagedAsync(
            page,
            pageSize,
            o => o.UserId == userId
        );

        var orderSummaries = new List<OrderSummaryDto>();
        foreach (var order in orders)
        {
            var itemCount = await _unitOfWork.OrderItems.CountAsync(oi => oi.OrderId == order.Id);
            orderSummaries.Add(new OrderSummaryDto(
                order.Id,
                order.OrderNumber,
                order.Status,
                order.TotalAmount,
                order.CreatedAt,
                itemCount
            ));
        }

        return new PagedResult<OrderSummaryDto>(
            orderSummaries,
            page,
            pageSize,
            total,
            (int)Math.Ceiling(total / (double)pageSize)
        );
    }

    public async Task<OrderDto?> GetOrderByIdAsync(Guid orderId, Guid userId)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order == null || !order.UserId.HasValue || order.UserId.Value != userId) return null;

        var orderItems = await _unitOfWork.OrderItems.GetByOrderIdAsync(orderId);
        
        var itemDtos = new List<OrderItemDto>();
        foreach (var item in orderItems)
        {
            var variant = await _unitOfWork.ProductVariants.GetByIdAsync(item.VariantId);
            var product = variant != null ? await _unitOfWork.Products.GetByIdAsync(variant.ProductId) : null;

            itemDtos.Add(new OrderItemDto(
                item.Id,
                item.VariantId,
                item.ProductNameSnapshot,
                item.SkuSnapshot,
                item.Qty,
                item.UnitPriceSnapshot,
                item.TaxAmount,
                item.TotalAmount
            ));
        }

        return new OrderDto(
            order.Id,
            order.OrderNumber,
            order.UserId!.Value,
            order.Status,
            order.SubtotalAmount,
            order.TaxAmount,
            order.ShippingAmount,
            order.DiscountAmount,
            order.TotalAmount,
            order.CreatedAt,
            itemDtos
        );
    }

    public async Task<OrderDto> CreateOrderAsync(Guid userId, CreateOrderDto dto)
    {
        // Get cart
        var cart = await _unitOfWork.Carts.GetActiveCartByUserIdAsync(userId);
        if (cart == null)
            throw new InvalidOperationException("Cart not found");

        var cartItems = await _unitOfWork.CartItems.GetByCartIdAsync(cart.Id);
        if (!cartItems.Any())
            throw new InvalidOperationException("Cart is empty");

        // Get addresses
        var shippingAddress = await _unitOfWork.Addresses.GetByIdAsync(dto.ShippingAddressId);
        var billingAddress = await _unitOfWork.Addresses.GetByIdAsync(dto.BillingAddressId);

        if (shippingAddress == null || billingAddress == null)
            throw new InvalidOperationException("Invalid address");

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            // Calculate totals
            decimal subTotal = 0;
            decimal taxAmount = 0;
            var orderItems = new List<OrderItem>();

            foreach (var cartItem in cartItems)
            {
                var variant = await _unitOfWork.ProductVariants.GetByIdAsync(cartItem.VariantId);
                var product = variant != null ? await _unitOfWork.Products.GetByIdAsync(variant.ProductId) : null;
                var price = await _unitOfWork.Prices.GetActivePrice(cartItem.VariantId);

                var unitPrice = price?.SellingPrice ?? 0;
                var itemTotal = unitPrice * cartItem.Qty;
                var itemTax = itemTotal * 0.1m; // 10% tax for simplicity

                subTotal += itemTotal;
                taxAmount += itemTax;

                // Get a tax rate (simplified - just get first active one)
                var taxRates = await _unitOfWork.TaxRates.GetActiveTaxRatesAsync();
                var taxRate = taxRates.FirstOrDefault();

                orderItems.Add(new OrderItem
                {
                    VariantId = cartItem.VariantId,
                    SkuSnapshot = variant?.Sku ?? "UNKNOWN",
                    ProductNameSnapshot = product?.Name ?? "Unknown Product",
                    VariantNameSnapshot = variant?.Sku ?? "Unknown",
                    Qty = cartItem.Qty,
                    UnitPriceSnapshot = unitPrice,
                    MrpSnapshot = price?.Mrp ?? unitPrice,
                    TaxRateSnapshotId = taxRate?.Id ?? Guid.Empty,
                    TaxAmount = itemTax,
                    DiscountAmount = 0,
                    TotalAmount = itemTotal + itemTax
                });
            }

            // Apply coupon discount if provided
            decimal discountAmount = 0;
            if (dto.CouponId.HasValue)
            {
                var coupon = await _unitOfWork.Coupons.GetByIdAsync(dto.CouponId.Value);
                if (coupon != null && await _unitOfWork.Coupons.IsCouponValidAsync(coupon.Code))
                {
                    if (coupon.Type == CouponType.Percentage)
                    {
                        discountAmount = subTotal * (coupon.Value / 100);
                    }
                    else
                    {
                        discountAmount = coupon.Value;
                    }
                }
            }

            var shippingCost = 10.00m; // Fixed shipping for simplicity
            var grandTotal = subTotal + taxAmount + shippingCost - discountAmount;

            // Create order
            var order = new Order
            {
                OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}",
                UserId = userId,
                Status = OrderStatus.Pending,
                SubtotalAmount = subTotal,
                TaxAmount = taxAmount,
                ShippingAmount = shippingCost,
                DiscountAmount = discountAmount,
                TotalAmount = grandTotal,
                PaymentStatus = PaymentStatus.Pending,
                PlacedAt = DateTime.UtcNow,
                RowVersion = new byte[8]
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            // Add order items
            foreach (var item in orderItems)
            {
                item.OrderId = order.Id;
                await _unitOfWork.OrderItems.AddAsync(item);
            }

            // Create address snapshots (simplified - just store formatted text)
            var shippingSnapshot = new OrderAddressSnapshot
            {
                OrderId = order.Id,
                AddressText = $"{shippingAddress.AddressLine1}, {shippingAddress.AddressLine2}, {shippingAddress.City}, {shippingAddress.State} {shippingAddress.Pincode}, {shippingAddress.Country}",
                Name = shippingAddress.Name,
                Phone = shippingAddress.Phone,
                Gstin = shippingAddress.Gstin
            };

            await _unitOfWork.OrderAddressSnapshots.AddAsync(shippingSnapshot);

            // Record coupon usage if applicable
            if (dto.CouponId.HasValue)
            {
                var couponUsage = new CouponUsage
                {
                    CouponId = dto.CouponId.Value,
                    OrderId = order.Id,
                    UserId = userId,
                    DiscountAmount = discountAmount
                };
                await _unitOfWork.CouponUsages.AddAsync(couponUsage);
            }

            // Clear cart
            cart.IsActive = false;
            _unitOfWork.Carts.Update(cart);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return (await GetOrderByIdAsync(order.Id, userId))!;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> CancelOrderAsync(Guid orderId, Guid userId)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order == null || !order.UserId.HasValue || order.UserId.Value != userId) return false;

        if (order.Status != OrderStatus.Pending)
            return false;

        order.Status = OrderStatus.Cancelled;
        _unitOfWork.Orders.Update(order);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
