namespace HouseRentingSystem.Web.Areas.Admin.Controllers
{
	using HouseRentingSystem.Services.Interfaces;
	using HouseRentingSystem.Web.ViewModels.Rent;
	using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using static Common.GeneralConstants;

	public class RentController : BaseAdminController
	{
		private readonly IRentService rentService;
		private readonly IMemoryCache memoryCache;

        public RentController(IRentService rentService, IMemoryCache memoryCache)
        {
            this.rentService = rentService;
			this.memoryCache = memoryCache;
        }

		[Route("/Rent/All")]
		[ResponseCache(Duration = 120)]
        public async Task <IActionResult> All()
		{
			ICollection<RentViewModel> allRents = this.memoryCache
				.Get<ICollection<RentViewModel>>(RentsCacheKey);

			if (allRents == null)
			{
				allRents = await this.rentService.GetAllRentedHousesAsync();

				MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromMinutes(RentsCacheDurationMinutes));

				this.memoryCache.Set(RentsCacheKey, allRents, cacheOptions);
			}

			return View(allRents);
		}
	}
}
