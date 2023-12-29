namespace HouseRentingSystem.Web.Areas.Admin.Controllers
{
	using HouseRentingSystem.Services.Interfaces;
	using HouseRentingSystem.Web.Areas.Admin.ViewModels;
	using HouseRentingSystem.Web.Infrastructure.Extensions;
	using Microsoft.AspNetCore.Mvc;

	public class HouseController : BaseAdminController
	{
		private readonly IAgentService agentService;
		private readonly IHouseService houseService;

        public HouseController(IAgentService agentService, IHouseService houseService)
        {
            this.agentService = agentService;
			this.houseService = houseService;
        }
		
        public async Task <IActionResult> Mine()
		{

			MyHousesViewModel myHouses = new MyHousesViewModel();

			try
			{
				string userId = this.User.GetId();
				Guid agentId = await this.agentService.GetAgentIdAsync(userId);
				myHouses.AddedHouses = await this.houseService.GetAllHousesByAgentIdAsync(agentId);
				myHouses.RentedHouses = await this.houseService.GetAllHousesByUserIdAsync(userId);
			}
			catch (Exception)
			{
				return this.BadRequest();
			}

			return View(myHouses);
		}
	}
}
