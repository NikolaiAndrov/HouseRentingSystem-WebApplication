namespace HouseRentingSystem.Services.Interfaces
{
	using HouseRentingSystem.Web.ViewModels.Home;
	using HouseRentingSystem.Web.ViewModels.House;

	public interface IHouseService
	{
		Task<ICollection<IndexViewModel>> LastThreeHousesAsync();

		Task AddHouseAsync(HouseFormModel houseModel, string userId);
	}
}
