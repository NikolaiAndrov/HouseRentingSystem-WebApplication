namespace HouseRentingSystem.Services.Interfaces
{
    using HouseRentingSystem.Web.ViewModels.Agent;

    public interface IAgentService
    {
        Task<bool>IsAgentExistingAsync(string userId);

        Task<bool> IsAgentPhoneExistingAsync(string phoneNumber);

        Task<bool> HasRentsByUserIdAsync(string userId);

        Task CreateAgentAsync(string userId, BecomeAgentFormModel model);
    }
}
