namespace HouseRentingSystem.Services.Interfaces
{
	using HouseRentingSystem.Web.ViewModels.Category;

	public interface ICategoryService
	{
		Task<ICollection<CategoryFormModel>> GetAllCategoriesAsync();

		Task<bool> IsCategoryExistingByIdAsync(int id);

		Task<ICollection<string>> AllCategoriesNameAsync();

		Task<ICollection<AllCategoriesViewModel>> GetAllCategoriesForListingAsync();

		Task<CategoryDetailsViewModel> GetCategoryDetailsAsync(int id);
	}
}
