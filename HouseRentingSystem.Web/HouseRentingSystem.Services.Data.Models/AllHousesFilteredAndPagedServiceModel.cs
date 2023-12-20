namespace HouseRentingSystem.Services.Data.Models
{
    using HouseRentingSystem.Web.ViewModels.House;

	public class AllHousesFilteredAndPagedServiceModel
	{
        public AllHousesFilteredAndPagedServiceModel()
        {
            this.Houses = new HashSet<HouseAllViewModel>();
        }

        public int AllHousesCount { get; set; }

		public ICollection<HouseAllViewModel> Houses { get; set; }
	}
}
