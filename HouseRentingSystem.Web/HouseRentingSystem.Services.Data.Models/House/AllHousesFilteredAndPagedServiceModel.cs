namespace HouseRentingSystem.Services.Data.Models.House
{
    using HouseRentingSystem.Web.ViewModels.House;

    public class AllHousesFilteredAndPagedServiceModel
    {
        public AllHousesFilteredAndPagedServiceModel()
        {
            Houses = new HashSet<HouseAllViewModel>();
        }

        public int AllHousesCount { get; set; }

        public ICollection<HouseAllViewModel> Houses { get; set; }
    }
}
