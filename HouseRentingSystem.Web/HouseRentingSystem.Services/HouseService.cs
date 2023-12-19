namespace HouseRentingSystem.Services
{
	using HouseRentingSystem.Data;
	using HouseRentingSystem.Data.Models;
	using HouseRentingSystem.Services.Interfaces;
	using HouseRentingSystem.Web.ViewModels.Home;
	using HouseRentingSystem.Web.ViewModels.House;
	using Microsoft.EntityFrameworkCore;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public class HouseService : IHouseService
	{
		private readonly HouseRentingDbContext dbContext;
		private readonly IAgentService agentService;

        public HouseService(HouseRentingDbContext dbContext, IAgentService agentService)
        {
            this.dbContext = dbContext;
			this.agentService = agentService;
        }

		public async Task AddHouseAsync(HouseFormModel houseModel, string userId)
		{
			Guid agentId = await this.agentService.GetAgentIdAsync(userId);

			House house = new House
			{
				Title = houseModel.Title,
				Address = houseModel.Address,
				Description = houseModel.Description,
				ImageUrl = houseModel.ImageUrl,
				PricePerMonth = houseModel.PricePerMonth,
				CategoryId = houseModel.CategoryId,
				AgentId = agentId
			};

			await this.dbContext.Houses.AddAsync(house);
			await this.dbContext.SaveChangesAsync();
		}

		public async Task<ICollection<IndexViewModel>> LastThreeHousesAsync()
		{
			ICollection<IndexViewModel> indexHouses = await dbContext.Houses
				.OrderByDescending(h => h.CreatedOn)
				.Take(3)
				.Select(h => new IndexViewModel
				{
					Id = h.Id.ToString(),
					Title = h.Title,
					ImageUrl = h.ImageUrl
				})
				.ToArrayAsync();

			return indexHouses;
		}
	}
}
