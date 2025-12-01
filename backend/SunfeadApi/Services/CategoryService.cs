using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Repositories.Interfaces;
using SunfeadApi.DTOs;
using SunfeadApi.Services.Interfaces;

namespace SunfeadApi.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        
        return categories.Select(c => new CategoryDto(
            c.Id,
            c.Name,
            c.Slug,
            c.Description,
            c.ParentId,
            c.IsActive
        ));
    }

    public async Task<IEnumerable<CategoryTreeDto>> GetCategoryTreeAsync()
    {
        var allCategories = await _unitOfWork.Categories.GetAllAsync();
        var rootCategories = allCategories.Where(c => c.ParentId == null);

        return rootCategories.Select(c => BuildCategoryTree(c, allCategories));
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null) return null;

        return new CategoryDto(
            category.Id,
            category.Name,
            category.Slug,
            category.Description,
            category.ParentId,
            category.IsActive
        );
    }

    public async Task<CategoryDto?> GetCategoryBySlugAsync(string slug)
    {
        var category = await _unitOfWork.Categories.GetBySlugAsync(slug);
        if (category == null) return null;

        return new CategoryDto(
            category.Id,
            category.Name,
            category.Slug,
            category.Description,
            category.ParentId,
            category.IsActive
        );
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Slug = dto.Slug,
            Description = dto.Description,
            ParentId = dto.ParentId,
            IsActive = true
        };

        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return new CategoryDto(
            category.Id,
            category.Name,
            category.Slug,
            category.Description,
            category.ParentId,
            category.IsActive
        );
    }

    public async Task<CategoryDto?> UpdateCategoryAsync(Guid id, UpdateCategoryDto dto)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null) return null;

        category.Name = dto.Name;
        category.Description = dto.Description;
        category.ParentId = dto.ParentId;
        category.IsActive = dto.IsActive;

        _unitOfWork.Categories.Update(category);
        await _unitOfWork.SaveChangesAsync();

        return new CategoryDto(
            category.Id,
            category.Name,
            category.Slug,
            category.Description,
            category.ParentId,
            category.IsActive
        );
    }

    public async Task<bool> DeleteCategoryAsync(Guid id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null) return false;

        category.IsActive = false;
        _unitOfWork.Categories.Update(category);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    private CategoryTreeDto BuildCategoryTree(Category category, IEnumerable<Category> allCategories)
    {
        var children = allCategories
            .Where(c => c.ParentId == category.Id)
            .Select(c => BuildCategoryTree(c, allCategories));

        return new CategoryTreeDto(
            category.Id,
            category.Name,
            category.Slug,
            category.Description,
            category.IsActive,
            children
        );
    }
}
