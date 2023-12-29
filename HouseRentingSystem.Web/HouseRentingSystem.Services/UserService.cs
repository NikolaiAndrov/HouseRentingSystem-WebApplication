namespace HouseRentingSystem.Services
{
	using HouseRentingSystem.Data;
	using HouseRentingSystem.Data.Models;
	using HouseRentingSystem.Services.Interfaces;
	using HouseRentingSystem.Web.ViewModels.User;
	using Microsoft.EntityFrameworkCore;
	using System.Collections.Generic;

	public class UserService : IUserService
	{
		private readonly HouseRentingDbContext dbContext;

        public UserService(HouseRentingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

		public async Task<ICollection<UserViewModel>> GetAllUsersAsync()
		{
			ICollection<UserViewModel> allUsers = await this.dbContext.Users
				.Select(u => new UserViewModel
				{
					Id = u.Id.ToString(),
					Email = u.Email,
					FullName = u.FirstName + " " + u.LastName,
					PhoneNumber = string.Empty
				})
				.ToArrayAsync();

			foreach (var user in allUsers)
			{
				Agent? agent = await this.dbContext.Agents
					.FirstOrDefaultAsync(a => a.UserId.ToString() == user.Id);

				if (agent != null) 
				{
					user.PhoneNumber = agent.PhoneNumber;
				}
			}

			return allUsers;
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
