namespace HouseRentingSystem.Web.Infrastructure.Extensions
{
	using HouseRentingSystem.Data.Models;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.Extensions.DependencyInjection;
	using static Common.GeneralConstants;

	public static class WebApplicationBuilderExtensions
	{
		public static IApplicationBuilder SeedAdministrator(this IApplicationBuilder app, string email)
		{
			using IServiceScope scopedService = app.ApplicationServices.CreateScope();

			IServiceProvider serviceProvider = scopedService.ServiceProvider;

			UserManager<ApplicationUser> userManagaer = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			RoleManager<IdentityRole<Guid>> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

			Task.Run(async () =>
			{
				if (await roleManager.RoleExistsAsync(AdminRoleName))
				{
					return;
				}

				IdentityRole<Guid> role = new IdentityRole<Guid>(AdminRoleName);

				await roleManager.CreateAsync(role);

				ApplicationUser adminUser = await userManagaer.FindByEmailAsync(email);

				await userManagaer.AddToRoleAsync(adminUser, AdminRoleName);
			})
			.GetAwaiter()
			.GetResult();

			return app;
		}
	}
}
