namespace HouseRentingSystem.Services.Interfaces
{
	using HouseRentingSystem.Web.ViewModels.Home;

	public interface IHouseService
	{
		Task<ICollection<IndexViewModel>> LastThreeHousesAsync();
	}
}
