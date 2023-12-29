namespace HouseRentingSystem.Web.Areas.Admin.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using static Common.GeneralConstants;

	[Area(AdminAreaName)]
	[Authorize(Roles = AdminRoleName)]
	public class BaseAdminController : Controller
	{
	}
}
