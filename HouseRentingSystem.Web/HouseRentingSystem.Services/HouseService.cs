namespace HouseRentingSystem.Services
{
	using HouseRentingSystem.Data;
	using HouseRentingSystem.Data.Models;
	using HouseRentingSystem.Services.Data.Models;
	using HouseRentingSystem.Services.Interfaces;
	using HouseRentingSystem.Web.ViewModels.Agent;
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

		public async Task<string> AddHouseAndGetHouseIdAsync(HouseFormModel houseModel, string userId)
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

			return house.Id.ToString();
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

		public async Task DeleteHouseById(string houseId, string userId)
		{
			Guid agentId = await this.agentService.GetAgentIdAsync(userId);

			House houseToDelete = await this.dbContext.Houses
				.FirstAsync(h => h.IsActive && h.Id.ToString() == houseId && h.AgentId == agentId);

			houseToDelete.IsActive = false;

			await this.dbContext.SaveChangesAsync();
		}

		public async Task EditHouseAsync(HouseFormModel house, string userId, string houseId)
		{
			Guid agentId = await this.agentService.GetAgentIdAsync(userId);

			House houseToEdit = await this.dbContext.Houses
				.FirstAsync(h => h.IsActive && h.Id.ToString() == houseId && h.AgentId == agentId);

			houseToEdit.Title = house.Title;
			houseToEdit.Address = house.Address;
			houseToEdit.Description = house.Description;
			houseToEdit.ImageUrl = house.ImageUrl;
			houseToEdit.PricePerMonth = house.PricePerMonth;
			houseToEdit.CategoryId = house.CategoryId;

			await this.dbContext.SaveChangesAsync();
		}

		public async Task<ICollection<HouseAllViewModel>> GetAllHousesByUserOrAgentIdAsync(string userId)
		{
			ICollection<HouseAllViewModel> myHouses;
			bool isAgent = await this.agentService.IsAgentExistingAsync(userId);

            if (isAgent)
            {
                Guid agentId = await this.agentService.GetAgentIdAsync(userId);

				myHouses = await this.dbContext.Houses
					.Where(h => h.IsActive && h.AgentId == agentId)
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
            }
			else
			{
				myHouses = await this.dbContext.Houses
					.Where(h => h.IsActive && h.RenterId.HasValue && h.RenterId.ToString() == userId)
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
			}

			return myHouses;
        }

		public async Task<HouseDetailViewModel> GetHouseDetailAsync(string houseId)
		{
			HouseDetailViewModel house = await this.dbContext.Houses
				.Where(h => h.IsActive && h.Id.ToString() == houseId)
				.Select (h => new HouseDetailViewModel
				{
					Id = houseId,
					Title = h.Title,
					Address = h.Address,
					ImageUrl = h.ImageUrl,
					PricePerMonth = h.PricePerMonth,
					IsRented = h.RenterId.HasValue,
					Description = h.Description,
					Category = h.Category.Name,
					Agent = new AgentForHouseDetailViewMopdel
					{
						Email = h.Agent.User.Email,
						PhoneNumber = h.Agent.PhoneNumber
					}
				})
				.FirstAsync();

			return house;
		}

		public async Task<HouseDeleteViewModel> GetHouseForDeleteByIdAsync(string houseId, string userId)
		{
			Guid agentId = await this.agentService.GetAgentIdAsync(userId);

			HouseDeleteViewModel houseToDelete = await this.dbContext.Houses
				.Where(h => h.IsActive && h.Id.ToString() == houseId && h.AgentId == agentId)
				.Select(h => new HouseDeleteViewModel
				{
					Title = h.Title,
					Address = h.Address,
					ImageUrl = h.ImageUrl,
				})
				.FirstAsync();

			return houseToDelete;
		}

		public async Task<HouseFormModel> GetHouseForEditAsync(string houseId, string userId)
		{
			Guid agentId = await this.agentService.GetAgentIdAsync(userId);

			HouseFormModel houseFormModel = await this.dbContext.Houses
				.Where(h => h.IsActive && h.Id.ToString() == houseId && h.AgentId == agentId)
				.Select(h => new HouseFormModel
				{
					Title = h.Title,
					Address = h.Address,
					Description = h.Description,
					ImageUrl= h.ImageUrl,
					PricePerMonth = h.PricePerMonth,
					CategoryId = h.CategoryId
				})
				.FirstAsync();
			
			return houseFormModel;
		}

		public async Task<bool> IsHouseExistingByIdAsync(string houseId)
			=> await this.dbContext.Houses.AnyAsync(h => h.IsActive && h.Id.ToString() == houseId);

		public async Task<bool> IsHouseRented(string houseId)
		{
			House house = await this.dbContext.Houses
				.FirstAsync(h => h.IsActive && h.Id.ToString() == houseId);

			return house.RenterId.HasValue;
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

		public async Task RentHouseAsync(string houseId, string userId)
		{
			House house = await this.dbContext.Houses
				.FirstAsync(h => h.IsActive && h.Id.ToString() == houseId);

			house.RenterId = Guid.Parse(userId);

			await this.dbContext.SaveChangesAsync();
		}
	}
}
