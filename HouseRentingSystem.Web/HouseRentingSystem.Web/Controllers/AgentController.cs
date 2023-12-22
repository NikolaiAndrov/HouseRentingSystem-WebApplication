namespace HouseRentingSystem.Web.Controllers
{
    using HouseRentingSystem.Services.Interfaces;
	using HouseRentingSystem.Web.ViewModels.Agent;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
	using static Common.NotificationConstantMessages;

    [Authorize]
	public class AgentController : Controller
	{
		private readonly IAgentService agentService;

        public AgentController(IAgentService agentService)
        {
            this.agentService = agentService;
        }

        [HttpGet]
		public async Task <IActionResult> Become()
		{
			string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
			bool isAgentExisting = await agentService.IsAgentExistingAsync(userId);

			if (isAgentExisting)
			{
				TempData[ErrorMessage] = "You are already an agent!";
				return RedirectToAction("Index", "Home");
			}

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Become(BecomeAgentFormModel model)
		{
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAgentExisting = await agentService.IsAgentExistingAsync(userId);

            if (isAgentExisting)
            {
                TempData[ErrorMessage] = "You are already an agent!";
                return RedirectToAction("Index", "Home");
            }

			bool isPhoneTaken = await agentService.IsAgentPhoneExistingAsync(model.PhoneNumber);

			if (isPhoneTaken)
			{
				ModelState.AddModelError(nameof(model.PhoneNumber), "The phone number provided is already taken!");
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			bool hasRents = await agentService.HasRentsByUserIdAsync(userId);

			if (hasRents)
			{
				TempData[ErrorMessage] = "To became an agent You must not have any active rents!";
                return RedirectToAction("Mine", "House");
            }

			try
			{
				await agentService.CreateAgentAsync(userId, model);
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = "Unexpected error occured, please try again later or contact administrator!";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("All", "House");
        }

    }
}
