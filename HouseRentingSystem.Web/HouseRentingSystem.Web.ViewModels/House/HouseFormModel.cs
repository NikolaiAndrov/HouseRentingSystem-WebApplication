namespace HouseRentingSystem.Web.ViewModels.House
{
	using HouseRentingSystem.Web.ViewModels.Category;
	using System.ComponentModel.DataAnnotations;
	using static Common.EntityValidationConstants.HouseValidation;

	public class HouseFormModel
	{
        public HouseFormModel()
        {
            this.Categories = new HashSet<CategoryFormModel>();
        }

		[Required(AllowEmptyStrings = false)]
		[StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

		[Required(AllowEmptyStrings = false)]
		[StringLength(AddressMaxLength, MinimumLength = AddressMinLength)]
		public string Address { get; set; } = null!;

		[Required(AllowEmptyStrings = false)]
		[StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
		public string Description { get; set; } = null!;

		[Required(AllowEmptyStrings = false)]
		[MaxLength(ImageUrlMaxlLength)]
		[Display(Name = "Image Link")]
		public string ImageUrl { get; set; } = null!;

		[Range(typeof(decimal), PricePerMonthMinValue, PricePerMonthMaxValue)]
		[Display(Name = "Price Per Month")]
		public decimal PricePerMonth { get; set; }

		[Required]
		public int CategoryId { get; set; }

		public ICollection<CategoryFormModel> Categories { get; set; }
	}
}
