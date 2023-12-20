namespace HouseRentingSystem.Services
{
	using HouseRentingSystem.Data;
	using HouseRentingSystem.Data.Models;
	using HouseRentingSystem.Services.Data.Models;
	using HouseRentingSystem.Services.Interfaces;
	using HouseRentingSystem.Web.ViewModels.Home;
	using HouseRentingSystem.Web.ViewModels.House;
	using HouseRentingSystem.Web.Views.House.Enums;
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
				CreatedOn = DateTime.UtcNow,
				CategoryId = houseModel.CategoryId,
				AgentId = agentId
			};

			await this.dbContext.Houses.AddAsync(house);
			await this.dbContext.SaveChangesAsync();
		}

		public async Task<AllHousesFilteredAndPagedServiceModel> AllAsync(AllHousesQueryModel allHousesQueryModel)
		{
			IQueryable<House> housesQuery = this.dbContext.Houses
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(allHousesQueryModel.Category))
			{
				housesQuery = housesQuery
					.Where(h => h.Category.Name == allHousesQueryModel.Category);
			}

			if (!string.IsNullOrWhiteSpace(allHousesQueryModel.SearchString))
			{
				string wildCard = $"%{allHousesQueryModel.SearchString.ToLower()}%";

				housesQuery = housesQuery
					.Where(h => EF.Functions.Like(h.Title, wildCard) ||
								EF.Functions.Like(h.Address, wildCard) ||
								EF.Functions.Like(h.Description, wildCard));

			}

			housesQuery = allHousesQueryModel.HouseSorting switch
			{
				HouseSorting.Newest => housesQuery.OrderByDescending(h => h.CreatedOn),
				HouseSorting.Oldest => housesQuery.OrderBy(h => h.CreatedOn),
				HouseSorting.PriceAscending => housesQuery.OrderBy(h => h.PricePerMonth),
				HouseSorting.PriceDescending => housesQuery.OrderByDescending(h => h.PricePerMonth),
				_ => housesQuery
					.OrderBy(h => h.RenterId == null)
					.ThenByDescending(h => h.CreatedOn)
			};

			ICollection<HouseAllViewModel> allHouses = await housesQuery
				.Where(h => h.IsActive)
				.Skip((allHousesQueryModel.CurrentPage - 1) * allHousesQueryModel.HousesPerPage)
				.Take(allHousesQueryModel.HousesPerPage)
				.Select(h => new HouseAllViewModel
				{
					Id = h.Id.ToString(),
					Title = h.Title,
					Address = h.Address,
					ImageUrl = h.ImageUrl,
					PricePerMonth = h.PricePerMonth,
					IsRented = h.RenterId.HasValue
				})
				.ToArrayAsync();

			int totalHouses = housesQuery.Count();

			AllHousesFilteredAndPagedServiceModel model = new AllHousesFilteredAndPagedServiceModel
			{
				AllHousesCount = totalHouses,
				Houses = allHouses
			};

			return model;
		}

		public async Task<ICollection<IndexViewModel>> LastThreeHousesAsync()
		{
			ICollection<IndexViewModel> indexHouses = await dbContext.Houses
				.Where(h => h.IsActive)
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
