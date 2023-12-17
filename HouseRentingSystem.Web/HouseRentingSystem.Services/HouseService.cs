namespace HouseRentingSystem.Services
{
	using HouseRentingSystem.Data;
	using HouseRentingSystem.Services.Interfaces;
	using HouseRentingSystem.Web.ViewModels.Home;
	using Microsoft.EntityFrameworkCore;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public class HouseService : IHouseService
	{
		private readonly HouseRentingDbContext dbContext;

        public HouseService(HouseRentingDbContext dbContext)
        {
            this.dbContext = dbContext;
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
