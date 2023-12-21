namespace HouseRentingSystem.Web.Controllers
{
	using HouseRentingSystem.Services.Data.Models;
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
		[HttpGet]
		public async Task<IActionResult> All([FromQuery]AllHousesQueryModel queryModel)
		{
			AllHousesFilteredAndPagedServiceModel serviceModel;

			try
			{
				serviceModel = await this.houseService.AllAsync(queryModel);
				queryModel.Houses = serviceModel.Houses;
				queryModel.TotalHouses = serviceModel.AllHousesCount;
				queryModel.Categories = await this.categoryService.AllCategoriesNameAsync();
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = "Unexpected error occured, please try later or contact administrator!";
				return RedirectToAction("Index", "Home");
			}

			return View(queryModel);
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

		[HttpGet]
		public async Task<IActionResult> Mine()
		{
			ICollection<HouseAllViewModel> myHouses;

			try
			{
				string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
				myHouses = await this.houseService.GetAllHousesByUserOrAgentIdAsync(userId);
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = "Unexpected error occured while retreiving your list of houses, please try later or contact administrator!";
				return RedirectToAction("Index", "Home");
			}

			return View(myHouses);
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Details(string id)
		{
			HouseDetailViewModel house;

			try
			{
				house = await this.houseService.GetHouseDetailAsync(id);
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = "House not found!";
				return RedirectToAction("Index", "Home");
			}

			return View(house);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string Id)
		{
			HouseFormModel house;
			bool isAgentExisting = false;
			bool isHouseExisting = false;

			try
			{
				string userId =	this.User.FindFirstValue(ClaimTypes.NameIdentifier);
				isAgentExisting = await this.agentService.IsAgentExistingAsync(userId);
				isHouseExisting = await this.houseService.IsHouseExistingByIdAsync(Id);
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = "Unexpected error occured while retreiving your list of houses, please try later or contact administrator!";
				return RedirectToAction("Index", "Home");
			}

			if (!isAgentExisting)
			{
				TempData[ErrorMessage] = "To edit a house you must be an agent and house must be yours!";
				return RedirectToAction("Become", "Agent");
			}

			if (!isHouseExisting)
			{
				TempData[ErrorMessage] = "It seems the house You looking for is no longer available!";
				return RedirectToAction("Index", "Home");
			}

			try
			{
				string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
				house = await this.houseService.GetHouseForEditAsync(Id, userId);
				house.Categories = await this.categoryService.GetAllCategoriesAsync();
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = "You do not have the permission to edit this house. To edit a house you must be the owner (Agent) of the house!";
				return RedirectToAction("Index", "Home");
			}

			return View(house);
		}

		//[HttpPost]
		//public async Task<IActionResult> Edit(string Id, HouseFormModel model)
		//{

		//}
	}
}
