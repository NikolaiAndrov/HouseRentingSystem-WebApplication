namespace HouseRentingSystem.WebApi
{
	using HouseRentingSystem.Data;
	using HouseRentingSystem.Services;
	using HouseRentingSystem.Services.Interfaces;
	using Microsoft.EntityFrameworkCore;

	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
			builder.Services.AddDbContext<HouseRentingDbContext>(opt => opt.UseSqlServer(connectionString));
			builder.Services.AddScoped<IHouseService, HouseService>();
			builder.Services.AddScoped<IAgentService, AgentService>();
			builder.Services.AddScoped<ICategoryService, CategoryService>();

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddCors(setup =>
			{
				setup.AddPolicy("HouseRentingSystem", policybuilder =>
				{
					policybuilder.WithOrigins("https://localhost:7028")
					.AllowAnyHeader()
					.AllowAnyMethod();
				});
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.UseCors("HouseRentingSystem");

			app.Run();
		}
	}
}
