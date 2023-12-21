namespace HouseRentingSystem.Web.ViewModels.House
{
	using HouseRentingSystem.Web.ViewModels.Agent;

	public class HouseDetailViewModel : HouseAllViewModel
	{
		public string Description { get; set; } = null!;

		public string Category { get; set; } = null!;

		public AgentForHouseDetailViewMopdel Agent { get; set; } = null!;
	}
}
