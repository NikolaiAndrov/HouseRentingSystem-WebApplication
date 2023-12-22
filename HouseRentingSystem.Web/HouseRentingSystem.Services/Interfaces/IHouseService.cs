﻿namespace HouseRentingSystem.Services.Interfaces
{
	using HouseRentingSystem.Services.Data.Models;
	using HouseRentingSystem.Web.ViewModels.Home;
	using HouseRentingSystem.Web.ViewModels.House;

	public interface IHouseService
	{
		Task<ICollection<IndexViewModel>> LastThreeHousesAsync();

		Task<string> AddHouseAndGetHouseIdAsync(HouseFormModel houseModel, string userId);

		Task<AllHousesFilteredAndPagedServiceModel> AllAsync(AllHousesQueryModel allHousesQueryModel);

		Task<ICollection<HouseAllViewModel>> GetAllHousesByUserOrAgentIdAsync(string userId);

		Task<HouseDetailViewModel> GetHouseDetailAsync(string houseId);

		Task<HouseFormModel> GetHouseForEditAsync(string houseId, string userId);

		Task EditHouseAsync(HouseFormModel house, string userId, string houseId);

		Task<bool> IsHouseExistingByIdAsync(string houseId);

		Task<HouseDeleteViewModel> GetHouseForDeleteByIdAsync(string houseId, string userId);

		Task DeleteHouseById(string houseId, string userId);
	}
}
