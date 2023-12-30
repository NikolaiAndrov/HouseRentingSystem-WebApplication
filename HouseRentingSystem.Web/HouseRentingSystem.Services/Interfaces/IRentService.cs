namespace HouseRentingSystem.Services.Interfaces
{
	using HouseRentingSystem.Web.ViewModels.Rent;

	public interface IRentService
	{
		Task<ICollection<RentViewModel>> GetAllRentedHousesAsync();
	}
}
