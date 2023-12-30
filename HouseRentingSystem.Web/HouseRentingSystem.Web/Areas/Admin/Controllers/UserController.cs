namespace HouseRentingSystem.Web.Areas.Admin.Controllers
{
	using HouseRentingSystem.Services.Interfaces;
	using HouseRentingSystem.Web.ViewModels.User;
	using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
	using static Common.GeneralConstants;

    public class UserController : BaseAdminController
	{
		private readonly IUserService userService;
		private readonly IMemoryCache memoryCache;

        public UserController(IUserService userService, IMemoryCache memoryCache)
        {
            this.userService = userService;
			this.memoryCache = memoryCache;
        }

		[Route("User/All")]
		[ResponseCache(Duration = 30)]
        public async Task<IActionResult> All()
		{
			ICollection<UserViewModel> allUsers = this.memoryCache.Get<ICollection<UserViewModel>>(UsersCacheKey);

			if (allUsers == null)
			{
				allUsers = await this.userService.GetAllUsersAsync();

				MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromMinutes(UsersCacheDurationMinutes));

				this.memoryCache.Set(UsersCacheKey, allUsers, cacheOptions);
			}

			return View(allUsers);
		}
	}
}
