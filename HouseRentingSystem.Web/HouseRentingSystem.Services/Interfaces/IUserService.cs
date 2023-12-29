namespace HouseRentingSystem.Services.Interfaces
{
	using HouseRentingSystem.Web.ViewModels.User;

	public interface IUserService
	{
		Task<string> GetUserFullNameByEmailAsync(string email);

		Task<ICollection<UserViewModel>> GetAllUsersAsync();
	}
}
