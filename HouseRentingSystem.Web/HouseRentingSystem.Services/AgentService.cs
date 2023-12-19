namespace HouseRentingSystem.Services
{
    using HouseRentingSystem.Data;
    using HouseRentingSystem.Data.Models;
    using HouseRentingSystem.Services.Interfaces;
    using HouseRentingSystem.Web.ViewModels.Agent;
    using Microsoft.EntityFrameworkCore;

    public class AgentService : IAgentService
    {
        private readonly HouseRentingDbContext dbContext;

        public AgentService(HouseRentingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> IsAgentExistingAsync(string userId)
            => await this.dbContext.Agents.AnyAsync(a => a.UserId.ToString() == userId);

        public async Task<bool> IsAgentPhoneExistingAsync(string phoneNumber)
            => await this.dbContext.Agents.AnyAsync(a => a.PhoneNumber == phoneNumber);

        public async Task<bool> HasRentsByUserIdAsync(string userId)
            => await this.dbContext.Houses.AnyAsync(h => h.RenterId.ToString() == userId);

        public async Task CreateAgentAsync(string userId, BecomeAgentFormModel model)
        {
            Agent agent = new Agent
            {
                UserId = Guid.Parse(userId),
                PhoneNumber = model.PhoneNumber
            };

            await dbContext.Agents.AddAsync(agent);
            await dbContext.SaveChangesAsync();
        }

		public async Task<Guid> GetAgentIdAsync(string userId)
		{
			Guid agentId = await this.dbContext.Agents
                .Where(a => a.UserId.ToString() == userId)
                .Select(a => a.Id)
                .FirstAsync();

            return agentId;
		}
	}
}
