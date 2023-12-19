namespace HouseRentingSystem.Services
{
	using HouseRentingSystem.Data;
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

		public async Task<bool> IsCategoryExistingByIdAsync(int id)
			=> await this.dbContext.Categories.AnyAsync(c => c.Id == id);
	}
}
