namespace HouseRentingSystem.Web.Controllers
{
	using HouseRentingSystem.Services.Interfaces;
	using HouseRentingSystem.Web.Infrastructure.Extensions;
	using HouseRentingSystem.Web.ViewModels.Category;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class CategoryController : Controller
	{
		private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

		[HttpGet]
        public async Task<IActionResult> All()
		{
			ICollection<AllCategoriesViewModel> allCategories = await this.categoryService.GetAllCategoriesForListingAsync();

			return View(allCategories);
		}

		[HttpGet]
		public async Task<IActionResult> Details(int Id, string information)
		{
			CategoryDetailsViewModel category = await this.categoryService.GetCategoryDetailsAsync(Id);

			if (category == null || category.GetUrlInformation() != information)
			{
				return this.NotFound();
			}

			return View(category);
		}
	}
}
