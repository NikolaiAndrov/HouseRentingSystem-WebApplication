namespace HouseRentingSystem.Services
{
	using HouseRentingSystem.Data;
	using HouseRentingSystem.Data.Models;
	using HouseRentingSystem.Services.Interfaces;
	using HouseRentingSystem.Web.ViewModels.Category;
	using Microsoft.EntityFrameworkCore;

	public class CategoryService : ICategoryService
	{
		private readonly HouseRentingDbContext dbContext;
        public CategoryService(HouseRentingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

		public async Task<ICollection<string>> AllCategoriesNameAsync()
		{
			ICollection<string> categories = await this.dbContext.Categories
				.Select(c => c.Name)
				.ToArrayAsync();

			return categories;
		}

		public async Task<ICollection<CategoryFormModel>> GetAllCategoriesAsync()
		{
			ICollection<CategoryFormModel> categories = await this.dbContext.Categories
				.Select(c => new CategoryFormModel
				{
					Id = c.Id,
					Name = c.Name,
				})
				.ToArrayAsync();

			return categories;
		}

		public async Task<ICollection<AllCategoriesViewModel>> GetAllCategoriesForListingAsync()
		{
			ICollection<AllCategoriesViewModel> allCategories = await this.dbContext.Categories
				.Select(c => new AllCategoriesViewModel
				{
					Id = c.Id,
					Name = c.Name,
				})
				.ToArrayAsync();

			return allCategories;
		}

		public async Task<CategoryDetailsViewModel> GetCategoryDetailsAsync(int id)
		{
			Category? category = await this.dbContext.Categories
				.FirstOrDefaultAsync(c => c.Id == id);

			if (category == null)
			{
				return null!;
			}

			CategoryDetailsViewModel detailsViewModel = new CategoryDetailsViewModel
			{
				Id = id,
				Name = category.Name,
			};

			return detailsViewModel;
		}

		public async Task<bool> IsCategoryExistingByIdAsync(int id)
			=> await this.dbContext.Categories.AnyAsync(c => c.Id == id);
	}
}
