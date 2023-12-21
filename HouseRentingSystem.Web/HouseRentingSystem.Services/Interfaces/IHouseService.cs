namespace HouseRentingSystem.Services.Interfaces
{
	using HouseRentingSystem.Services.Data.Models;
	using HouseRentingSystem.Web.ViewModels.Home;
	using HouseRentingSystem.Web.ViewModels.House;

	public interface IHouseService
	{
		Task<ICollection<IndexViewModel>> LastThreeHousesAsync();

		Task AddHouseAsync(HouseFormModel houseModel, string userId);

		Task<AllHousesFilteredAndPagedServiceModel> AllAsync(AllHousesQueryModel allHousesQueryModel);

		Task<ICollection<HouseAllViewModel>> GetAllHousesByUserOrAgentIdAsync(string userId);
	}
}
