namespace HouseRentingSystem.Web
{
	using Microsoft.EntityFrameworkCore;
	using HouseRentingSystem.Data;
	using HouseRentingSystem.Data.Models;
	using HouseRentingSystem.Services;
	using HouseRentingSystem.Services.Interfaces;
	using HouseRentingSystem.Web.Infrastructure.ModelBinders;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Identity;
	using static Common.GeneralConstants;
	using HouseRentingSystem.Web.Infrastructure.Extensions;

	public class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

			builder.Services.AddDbContext<HouseRentingDbContext>(options =>
				options.UseSqlServer(connectionString));

			builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
			{
				options.SignIn.RequireConfirmedAccount = builder
					.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");

				options.Password.RequireUppercase = builder
					.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");

				options.Password.RequireLowercase = builder
					.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");

				options.Password.RequireNonAlphanumeric = builder
					.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");

				options.Password.RequireDigit = builder
					.Configuration.GetValue<bool>("Identity:Password:RequireDigit");

				options.Password.RequiredLength = builder
					.Configuration.GetValue<int>("Identity:Password:RequiredLength");
			})
			.AddRoles<IdentityRole<Guid>>()
			.AddEntityFrameworkStores<HouseRentingDbContext>();

			builder.Services
				.AddControllersWithViews()
				.AddMvcOptions(options =>
				{
					options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
					options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
				});

			builder.Services.AddScoped<IHouseService, HouseService>();
			builder.Services.AddScoped<IAgentService, AgentService>();
			builder.Services.AddScoped<ICategoryService, CategoryService>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IRentService, RentService>();

			builder.Services.ConfigureApplicationCookie(cfg =>
			{
				cfg.LoginPath = "/User/Login";
			});

			WebApplication app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseMigrationsEndPoint();
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error/500");
				app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.SeedAdministrator(DevelopmentAdminEmail);

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(name: "areas", pattern: "/{area:exists}/{controller=Home}/{action=Index}/{id?}");

				endpoints.MapControllerRoute(name: "ProtectingUrlRoute",
					pattern: "/{controller}/{action}/{id}/{information}",
					defaults: new { Controller = "Category", Action = "Details" });

				endpoints.MapDefaultControllerRoute();
				endpoints.MapRazorPages();
			});

			app.Run();
		}
	}
}