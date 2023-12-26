namespace HouseRentingSystem.Services.Interfaces
{
	public interface IUserService
	{
		Task<string> GetUserFullNameByEmailAsync(string email);
	}
}
