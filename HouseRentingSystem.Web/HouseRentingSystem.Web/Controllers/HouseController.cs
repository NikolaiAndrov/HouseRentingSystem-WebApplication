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
		public async Task<IActionResult> All([FromQuery] AllHousesQueryModel queryModel)
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
				this.GeneralError();
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
				this.GeneralError();
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

			string houseId;

			try
			{
				houseId = await this.houseService.AddHouseAndGetHouseIdAsync(model, userId);
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = "Unexpected error occured while trying to add the house, please try later or contact administrator!";
				return RedirectToAction("Index", "Home");
			}

			TempData[SuccessMessage] = "You have added a new house successfuly";
			return RedirectToAction("Details", "House", new {id = houseId});
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
				string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
				isAgentExisting = await this.agentService.IsAgentExistingAsync(userId);
				isHouseExisting = await this.houseService.IsHouseExistingByIdAsync(Id);
			}
			catch (Exception)
			{
				this.GeneralError();
			}

			if (!isAgentExisting)
			{
				TempData[ErrorMessage] = "To edit a house you must be an agent and house must be yours!";
				return RedirectToAction("Become", "Agent");
			}

			if (!isHouseExisting)
			{
				TempData[ErrorMessage] = "It seems the house You are looking for is no longer available!";
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
				return RedirectToAction("All", "House");
			}

			return View(house);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(string Id, HouseFormModel model)
		{
			bool isAgentExisting = false;
			bool isHouseExisting = false;
			bool isCategoryExisting = false;

			try
			{
				string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
				isAgentExisting = await this.agentService.IsAgentExistingAsync(userId);
				isHouseExisting = await this.houseService.IsHouseExistingByIdAsync(Id);
				isCategoryExisting = await this.categoryService.IsCategoryExistingByIdAsync(model.CategoryId);
			}
			catch (Exception)
			{
				this.GeneralError();
			}

			if (!isAgentExisting)
			{
				TempData[ErrorMessage] = "To edit a house you must be an agent and house must be yours!";
				return RedirectToAction("Become", "Agent");
			}

			if (!isHouseExisting)
			{
				TempData[ErrorMessage] = "It seems that the house You are looking for is no longer available!";
				return RedirectToAction("Index", "Home");
			}

			if (!isCategoryExisting)
			{
				this.ModelState.AddModelError(nameof(model.CategoryId), "The selected category does not exist!");
			}

			if (!this.ModelState.IsValid)
			{
				model.Categories = await this.categoryService.GetAllCategoriesAsync();
				return View(model);
			}

			try
			{
				string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
				await this.houseService.EditHouseAsync(model, userId, Id);
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = "You do not have the permission to edit this house. To edit a house you must be the owner (Agent) of the house!";
				return RedirectToAction("Mine", "House");
			}

			TempData[SuccessMessage] = "You have edited the house successfuly!";
			return RedirectToAction("Details", "House", new { Id });
		}

		[HttpGet]
		public async Task<IActionResult> Delete(string id)
		{
			bool isAgentExisting = false;
			bool isHouseExisting = false;
			string userId;

			try
			{
				userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
				isAgentExisting = await this.agentService.IsAgentExistingAsync(userId);
				isHouseExisting = await this.houseService.IsHouseExistingByIdAsync(id);
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = "Unexpected error occured while trying to execute your request, please try later or contact administrator!";
				return RedirectToAction("Index", "Home");
			}

			if (!isAgentExisting)
			{
				TempData[ErrorMessage] = "To delete a house you must be an agent and house must be yours!";
				return RedirectToAction("Become", "Agent");
			}

			if (!isHouseExisting)
			{
				TempData[ErrorMessage] = "It seems that the house You looking for is no longer available!";
				return RedirectToAction("Index", "Home");
			}

			HouseDeleteViewModel houseToDelete;

			try
			{
				houseToDelete = await this.houseService.GetHouseForDeleteByIdAsync(id, userId);
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = "You do not have the permission to delete this house. To delete a house you must be the owner (Agent) of the house!";
				return RedirectToAction("Mine", "House");
			}

			TempData[WarningMessage] = "You are about to delete this house!";
			return View(houseToDelete);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(string id, HouseDeleteViewModel model)
		{
			bool isAgentExisting = false;
			bool isHouseExisting = false;
			string userId;

			try
			{
				userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
				isAgentExisting = await this.agentService.IsAgentExistingAsync(userId);
				isHouseExisting = await this.houseService.IsHouseExistingByIdAsync(id);
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = "Unexpected error occured while trying to execute your request, please try later or contact administrator!";
				return RedirectToAction("Index", "Home");
			}

			if (!isAgentExisting)
			{
				TempData[ErrorMessage] = "To delete a house you must be an agent and house must be yours!";
				return RedirectToAction("Become", "Agent");
			}

			if (!isHouseExisting)
			{
				TempData[ErrorMessage] = "It seems that the house You looking for is no longer available!";
				return RedirectToAction("Index", "Home");
			}

			try
			{
				await this.houseService.DeleteHouseById(id, userId);
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = "You do not have the permission to delete this house. To delete a house you must be the owner (Agent) of the house!";
				return RedirectToAction("Mine", "House");
			}

			TempData[SuccessMessage] = "You have successfully deleted the house!";
			return RedirectToAction("Mine", "House");
		}

		[HttpPost]
		public async Task<IActionResult> Rent(string id)
		{
			bool isHouseExisting;
			bool isHouseRented;
			bool isAgent;
			string userId;

			try
			{
				isHouseExisting = await this.houseService.IsHouseExistingByIdAsync(id);
				isHouseRented = await this.houseService.IsHouseRented(id);
				userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
				isAgent = await this.agentService.IsAgentExistingAsync(userId);
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = "Unexpected error occured while trying to execute your request, please try later or contact administrator!";
				return RedirectToAction("Index", "Home");
			}

			if (!isHouseExisting)
			{
				TempData[ErrorMessage] = "It seems that the house You are looking for is no longer available!";
				return RedirectToAction("Index", "Home");
			}

			if (isHouseRented)
			{
				TempData[ErrorMessage] = "This house is already rented!";
				return RedirectToAction("All", "House");
			}

			if (isAgent)
			{
				TempData[ErrorMessage] = "Agents cannot rent houses!";
				return RedirectToAction("Index", "Home");
			}

			try
			{
				await this.houseService.RentHouseAsync(id, userId);
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = "Unexpected error occured while trying to execute your request, please try later or contact administrator!";
				return RedirectToAction("Index", "Home");
			}

			TempData[SuccessMessage] = "House rented successfully!";
			return RedirectToAction("Mine", "House");
		}

		private IActionResult GeneralError()
		{
			TempData[ErrorMessage] = "Unexpected error occured while trying to execute your request, please try later or contact administrator!";
			return RedirectToAction("Index", "Home");
		}
	}
}
