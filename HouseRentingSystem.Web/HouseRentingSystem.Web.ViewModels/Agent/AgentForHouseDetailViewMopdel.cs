namespace HouseRentingSystem.Web.ViewModels.Agent
{
	using System.ComponentModel.DataAnnotations;

	public class AgentForHouseDetailViewMopdel
	{
		public string Email { get; set; } = null!;

		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; } = null!;
	}
}
