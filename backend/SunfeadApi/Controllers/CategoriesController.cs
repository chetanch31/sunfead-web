using Microsoft.AspNetCore.Mvc;
using SunfeadApi.DTOs;
using SunfeadApi.Services.Interfaces;

namespace SunfeadApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDto>>>> GetCategories()
    {
        try
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(new ApiResponse<IEnumerable<CategoryDto>>(true, categories));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<IEnumerable<CategoryDto>>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpGet("tree")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoryTreeDto>>>> GetCategoryTree()
    {
        try
        {
            var tree = await _categoryService.GetCategoryTreeAsync();
            return Ok(new ApiResponse<IEnumerable<CategoryTreeDto>>(true, tree));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<IEnumerable<CategoryTreeDto>>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> GetCategory(Guid id)
    {
        try
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound(new ApiResponse<CategoryDto>(false, null, "Category not found"));

            return Ok(new ApiResponse<CategoryDto>(true, category));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<CategoryDto>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> GetCategoryBySlug(string slug)
    {
        try
        {
            var category = await _categoryService.GetCategoryBySlugAsync(slug);
            if (category == null)
                return NotFound(new ApiResponse<CategoryDto>(false, null, "Category not found"));

            return Ok(new ApiResponse<CategoryDto>(true, category));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<CategoryDto>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> CreateCategory([FromBody] CreateCategoryDto dto)
    {
        try
        {
            var category = await _categoryService.CreateCategoryAsync(dto);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, new ApiResponse<CategoryDto>(true, category, "Category created successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<CategoryDto>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> UpdateCategory(Guid id, [FromBody] UpdateCategoryDto dto)
    {
        try
        {
            var category = await _categoryService.UpdateCategoryAsync(id, dto);
            if (category == null)
                return NotFound(new ApiResponse<CategoryDto>(false, null, "Category not found"));

            return Ok(new ApiResponse<CategoryDto>(true, category, "Category updated successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<CategoryDto>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse>> DeleteCategory(Guid id)
    {
        try
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result)
                return NotFound(new ApiResponse(false, "Category not found"));

            return Ok(new ApiResponse(true, "Category deleted successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse(false, "An error occurred", new[] { ex.Message }));
        }
    }
}
