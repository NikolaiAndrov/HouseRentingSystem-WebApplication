namespace HouseRentingSystem.Web.Controllers
{
	using HouseRentingSystem.Services.Interfaces;
	using HouseRentingSystem.Web.ViewModels;
	using HouseRentingSystem.Web.ViewModels.Home;
	using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;

    public class HomeController : Controller
    {
        private readonly IHouseService houseService;

        public HomeController(IHouseService houseService)
        {
            this.houseService = houseService;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<IndexViewModel> indexHouses = await houseService.LastThreeHousesAsync();
            return View(indexHouses);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 400 || statusCode == 404)
            {
                return View("Error404");
            }

            if (statusCode == 401)
            {
				return View("Error401");
			}

            return View();
        }
    }
}
