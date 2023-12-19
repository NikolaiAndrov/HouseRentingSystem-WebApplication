namespace HouseRentingSystem.Web.Controllers
{
	using HouseRentingSystem.Web.ViewModels.House;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class HouseController : Controller
	{
		[AllowAnonymous]
		public async Task<IActionResult> All()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			HouseFormModel model = new HouseFormModel();
			return View(model);
		}
	}
}
