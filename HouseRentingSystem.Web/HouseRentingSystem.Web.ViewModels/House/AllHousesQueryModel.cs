namespace HouseRentingSystem.Web.ViewModels.House
{
	using HouseRentingSystem.Web.Views.House.Enums;
	using System.ComponentModel.DataAnnotations;
	using static Common.GeneralConstants;

	public class AllHousesQueryModel
	{
        public AllHousesQueryModel()
        {
            this.Categories = new HashSet<string>();
			this.Houses = new HashSet<HouseAllViewModel>();
			this.CurrentPage = DefaultPage;
			this.HousesPerPage = DefaultEntitiesPerPage;
        }

        public string? Category { get; set; }

		[Display(Name = "Search by word")]
		public string? SearchString { get; set; }

		[Display(Name = "Sort Houses By")]
		public HouseSorting HouseSorting { get; set; }

		public int CurrentPage { get; set; }

		[Display(Name = "Houses Per Page")]
		public int HousesPerPage { get; set; }

		public int TotalHouses { get; set; }

		public ICollection<string> Categories { get; set; }

		public ICollection<HouseAllViewModel> Houses { get; set; }
	}
}
