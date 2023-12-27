namespace HouseRentingSystem.Services.Interfaces
{
    using HouseRentingSystem.Services.Data.Models.House;
	using HouseRentingSystem.Services.Data.Models.Statistics;
	using HouseRentingSystem.Web.ViewModels.Home;
    using HouseRentingSystem.Web.ViewModels.House;
    using System.Diagnostics.SymbolStore;

    public interface IHouseService
	{
		Task<ICollection<IndexViewModel>> LastThreeHousesAsync();

		Task<string> AddHouseAndGetHouseIdAsync(HouseFormModel houseModel, string userId);

		Task<AllHousesFilteredAndPagedServiceModel> AllAsync(AllHousesQueryModel allHousesQueryModel);

		Task<ICollection<HouseAllViewModel>> GetAllHousesByUserOrAgentIdAsync(string userId, bool isAdimn);

		Task<HouseDetailViewModel> GetHouseDetailAsync(string houseId);

		Task<HouseFormModel> GetHouseForEditAsync(string houseId, string userId, bool isAdmin);

		Task EditHouseAsync(HouseFormModel house, string userId, string houseId, bool isAdmin);

		Task<bool> IsHouseExistingByIdAsync(string houseId);

		Task<HouseDeleteViewModel> GetHouseForDeleteByIdAsync(string houseId, string userId, bool isAdmin);

		Task DeleteHouseById(string houseId, string userId, bool isAdmin);

		Task<bool> IsHouseRented(string houseId);

		Task RentHouseAsync(string houseId, string userId);

		Task LeaveHouseAsync(string houseId);

		Task<bool> IsHouseRentedByCurrentUserAsync(string houseId, string userId);

		Task<StatisticsServiceModel> GetStatisticsAsync();
	}
}
