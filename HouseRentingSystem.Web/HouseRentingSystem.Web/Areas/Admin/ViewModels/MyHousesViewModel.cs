namespace HouseRentingSystem.Web.Areas.Admin.ViewModels
{
	using HouseRentingSystem.Web.ViewModels.House;

	public class MyHousesViewModel
	{
        public MyHousesViewModel()
        {
            this.AddedHouses = new HashSet<HouseAllViewModel>();
			this.RentedHouses = new HashSet<HouseAllViewModel>();
        }

        public ICollection<HouseAllViewModel> AddedHouses { get; set; }

		public ICollection<HouseAllViewModel> RentedHouses { get;  set; }
	}
}
