namespace HouseRentingSystem.Web.Areas.Admin.Controllers
{
	using HouseRentingSystem.Services.Interfaces;
	using HouseRentingSystem.Web.ViewModels.Rent;
	using Microsoft.AspNetCore.Mvc;

	public class RentController : BaseAdminController
	{
		private readonly IRentService rentService;

        public RentController(IRentService rentService)
        {
            this.rentService = rentService;
        }

		[Route("/Rent/All")]
        public async Task <IActionResult> All()
		{
			ICollection<RentViewModel> allRents = await this.rentService.GetAllRentedHousesAsync();

			return View(allRents);
		}
	}
}
