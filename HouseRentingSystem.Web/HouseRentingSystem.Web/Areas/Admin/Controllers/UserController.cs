namespace HouseRentingSystem.Web.Areas.Admin.Controllers
{
	using HouseRentingSystem.Services.Interfaces;
	using HouseRentingSystem.Web.ViewModels.User;
	using Microsoft.AspNetCore.Mvc;

	public class UserController : BaseAdminController
	{
		private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

		[Route("User/All")]
        public async Task<IActionResult> All()
		{
			ICollection<UserViewModel> allUsers = await this.userService.GetAllUsersAsync();

			return View(allUsers);
		}
	}
}
