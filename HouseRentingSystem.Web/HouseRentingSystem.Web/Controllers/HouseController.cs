namespace HouseRentingSystem.Web.Controllers
{
	using HouseRentingSystem.Services.Interfaces;
	using HouseRentingSystem.Web.ViewModels.House;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using System.Security.Claims;
	using static Common.NotificationConstantMessages;

	[Authorize]
	public class HouseController : Controller
	{
		private readonly IHouseService houseService;
		private readonly IAgentService agentService;
		private readonly ICategoryService categoryService;

		public HouseController(IHouseService houseService, IAgentService agentService, ICategoryService categoryService) 
		{
			this.houseService = houseService;
			this.agentService = agentService;
			this.categoryService = categoryService;
		}


		[AllowAnonymous]
		public async Task<IActionResult> All()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			HouseFormModel model = new HouseFormModel();
			bool isAgent = false;

			try
			{
				string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
				isAgent = await this.agentService.IsAgentExistingAsync(userId);

				model.Categories = await this.categoryService.GetAllCategoriesAsync();
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = "Unexpected error occured, please try later or contact administrator!";
				return RedirectToAction("Index", "Home");
			}

			if (!isAgent)
			{
				TempData[ErrorMessage] = "To add a house you must be an Agent!";
				return RedirectToAction("Become", "Agent");
			}

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(HouseFormModel model)
		{
			bool isCategoryExisting = false;
			bool isAgent = false;
			string userId;

			try
			{
				userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

				isCategoryExisting = await this.categoryService.IsCategoryExistingByIdAsync(model.CategoryId);
				isAgent = await this.agentService.IsAgentExistingAsync(userId);
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = "Unexpected error occured, please try later or contact administrator!";
				return RedirectToAction("Index", "Home");
			}

			if (!isAgent)
			{
				TempData[ErrorMessage] = "To add a house you must be an Agent!";
				return RedirectToAction("Become", "Agent");
			}

			if (!isCategoryExisting)
			{
				ModelState.AddModelError(nameof(model.CategoryId), "You should select an existing category!");
			}

			if (!ModelState.IsValid)
			{
				try
				{
					model.Categories = await this.categoryService.GetAllCategoriesAsync();
				}
				catch (Exception)
				{
					TempData[ErrorMessage] = "Unexpected error occured, please try later or contact administrator!";
					return RedirectToAction("Index", "Home");
				}

				return View(model);
			}

			try
			{
				await this.houseService.AddHouseAsync(model, userId);
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = "Unexpected error occured while trying to add the house, please try later or contact administrator!";
				return RedirectToAction("Index", "Home");
			}

			return RedirectToAction("All", "House");
		}
	}
}
