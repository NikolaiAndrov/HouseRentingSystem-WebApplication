namespace HouseRentingSystem.WebApi.Controllers
{
	using HouseRentingSystem.Services.Data.Models.Statistics;
	using HouseRentingSystem.Services.Interfaces;
	using Microsoft.AspNetCore.Mvc;

	[Route("api/statistics")]
	[ApiController]
	public class StatisticsApiController : ControllerBase
	{
		private readonly IHouseService houseService;

        public StatisticsApiController(IHouseService houseService)
        {
            this.houseService = houseService;
        }

		[HttpGet]
		[Produces("application/json")]
		[ProducesResponseType(200, Type = typeof(StatisticsServiceModel))]
		[ProducesResponseType(400)]
		public async Task<IActionResult> GetStatistics()
		{
			try
			{
 				StatisticsServiceModel model = await this.houseService.GetStatisticsAsync();
				return this.Ok(model);
			}
			catch (Exception)
			{
				return this.BadRequest();
			}
		}
    }
}
