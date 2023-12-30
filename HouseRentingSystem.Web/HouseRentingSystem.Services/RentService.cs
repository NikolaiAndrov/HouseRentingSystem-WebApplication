namespace HouseRentingSystem.Services
{
	using HouseRentingSystem.Data;
	using HouseRentingSystem.Services.Interfaces;
	using HouseRentingSystem.Web.ViewModels.Rent;
	using Microsoft.EntityFrameworkCore;

	public class RentService : IRentService
	{
		private readonly HouseRentingDbContext dbContext;

        public RentService(HouseRentingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<RentViewModel>> GetAllRentedHousesAsync()
		{
			ICollection<RentViewModel> allRentedHouses = await this.dbContext.Houses
				.Where(h => h.IsActive && h.RenterId.HasValue)
				.Select(h => new RentViewModel
				{
					HouseTitle = h.Title,
					HouseImageUrl = h.ImageUrl,
					AgentFullName = h.Agent.User.FirstName + " " + h.Agent.User.LastName,
					AgentEmail = h.Agent.User.Email,
					RenterFullName = h.Renter!.FirstName + " " + h.Renter.LastName,
					RenterEmail = h.Renter.Email
				})
				.ToArrayAsync();

			return allRentedHouses;
		}
	}
}
