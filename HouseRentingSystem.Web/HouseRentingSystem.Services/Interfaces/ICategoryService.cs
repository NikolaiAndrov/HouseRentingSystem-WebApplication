namespace HouseRentingSystem.Services.Interfaces
{
	using HouseRentingSystem.Web.ViewModels.Category;

	public interface ICategoryService
	{
		Task<ICollection<CategoryFormModel>> GetAllCategoriesAsync();
	}
}
