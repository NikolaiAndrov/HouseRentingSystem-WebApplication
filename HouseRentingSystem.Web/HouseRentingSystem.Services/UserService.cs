namespace HouseRentingSystem.Services
{
	using HouseRentingSystem.Data;
	using HouseRentingSystem.Data.Models;
	using HouseRentingSystem.Services.Interfaces;
	using Microsoft.EntityFrameworkCore;

	public class UserService : IUserService
	{
		private readonly HouseRentingDbContext dbContext;

        public UserService(HouseRentingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<string> GetUserFullNameByEmailAsync(string email)
		{
			ApplicationUser? user = await this.dbContext.Users
				.FirstOrDefaultAsync(u => u.Email == email);

			if (user == null)
			{
				return string.Empty;
			}

			return $"{user.FirstName} {user.LastName}";
		}
	}
}
