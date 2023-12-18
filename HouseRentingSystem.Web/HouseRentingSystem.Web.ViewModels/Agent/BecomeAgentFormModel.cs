namespace HouseRentingSystem.Web.ViewModels.Agent
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.AgentValidation;

    public class BecomeAgentFormModel
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(PhoneMaxLength, MinimumLength = PhoneMinLength)]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = null!;
    }
}
