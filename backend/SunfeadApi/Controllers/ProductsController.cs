using Microsoft.AspNetCore.Mvc;
using SunfeadApi.DTOs;
using SunfeadApi.Services.Interfaces;

namespace SunfeadApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<ProductDto>>>> GetProducts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] bool? isActive = null,
        [FromQuery] Guid? categoryId = null)
    {
        try
        {
            var result = await _productService.GetProductsAsync(page, pageSize, isActive, categoryId);
            return Ok(new ApiResponse<PagedResult<ProductDto>>(true, result));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<PagedResult<ProductDto>>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<ProductDetailDto>>> GetProduct(Guid id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new ApiResponse<ProductDetailDto>(false, null, "Product not found"));

            return Ok(new ApiResponse<ProductDetailDto>(true, product));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<ProductDetailDto>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<ApiResponse<ProductDetailDto>>> GetProductBySlug(string slug)
    {
        try
        {
            var product = await _productService.GetProductBySlugAsync(slug);
            if (product == null)
                return NotFound(new ApiResponse<ProductDetailDto>(false, null, "Product not found"));

            return Ok(new ApiResponse<ProductDetailDto>(true, product));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<ProductDetailDto>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpGet("category/{categoryId:guid}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetProductsByCategory(Guid categoryId)
    {
        try
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(new ApiResponse<IEnumerable<ProductDto>>(true, products));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<IEnumerable<ProductDto>>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ProductDetailDto>>> CreateProduct([FromBody] CreateProductDto dto)
    {
        try
        {
            var product = await _productService.CreateProductAsync(dto);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, new ApiResponse<ProductDetailDto>(true, product, "Product created successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<ProductDetailDto>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<ProductDetailDto>>> UpdateProduct(Guid id, [FromBody] UpdateProductDto dto)
    {
        try
        {
            var product = await _productService.UpdateProductAsync(id, dto);
            if (product == null)
                return NotFound(new ApiResponse<ProductDetailDto>(false, null, "Product not found"));

            return Ok(new ApiResponse<ProductDetailDto>(true, product, "Product updated successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<ProductDetailDto>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse>> DeleteProduct(Guid id)
    {
        try
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
                return NotFound(new ApiResponse(false, "Product not found"));

            return Ok(new ApiResponse(true, "Product deleted successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse(false, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpPost("{productId:guid}/variants")]
    public async Task<ActionResult<ApiResponse<ProductVariantDto>>> AddVariant(Guid productId, [FromBody] CreateProductVariantDto dto)
    {
        try
        {
            var variant = await _productService.AddVariantAsync(productId, dto);
            return Ok(new ApiResponse<ProductVariantDto>(true, variant, "Variant added successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<ProductVariantDto>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpGet("variants/{variantId:guid}/stock")]
    public async Task<ActionResult<ApiResponse<int>>> GetStock(Guid variantId)
    {
        try
        {
            var stock = await _productService.GetAvailableStockAsync(variantId);
            return Ok(new ApiResponse<int>(true, stock));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<int>(false, 0, "An error occurred", new[] { ex.Message }));
        }
    }
}
