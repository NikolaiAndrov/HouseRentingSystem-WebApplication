namespace HouseRentingSystem.Data.Models
{
    using Microsoft.AspNetCore.Identity;
	using System.ComponentModel.DataAnnotations;
	using static Common.EntityValidationConstants.UserValidation;

    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser() 
        {
            this.Id = Guid.NewGuid();
            this.RentedHouses = new HashSet<House>();
        }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required(AllowEmptyStrings = false)]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        public virtual ICollection<House> RentedHouses { get; set; }
    }
}
