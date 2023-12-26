namespace HouseRentingSystem.Web.Controllers
{
    using HouseRentingSystem.Data.Models;
    using HouseRentingSystem.Web.ViewModels.User;
	using Microsoft.AspNetCore.Authentication;
	using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using static Common.NotificationConstantMessages;

    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserStore<ApplicationUser> userStore;

        public UserController(SignInManager<ApplicationUser> signInManager,
               UserManager<ApplicationUser> userManager,
               IUserStore<ApplicationUser> userStore)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userStore = userStore;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser applicationUser = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            await this.userManager.SetEmailAsync(applicationUser, model.Email);
            await this.userManager.SetUserNameAsync(applicationUser, model.Email);

            IdentityResult identityResult = await this.userManager.CreateAsync(applicationUser, model.Password);

            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            await this.signInManager.SignInAsync(applicationUser, false);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			LoginFormModel model = new LoginFormModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var signInResult = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (!signInResult.Succeeded)
            {
                TempData[ErrorMessage] = "An error occured while logging you, please try again later or contact administrator!";
                return View(model);
            }

            return Redirect(model.ReturnUrl ?? "/Home/Index");
        }
    }
}
