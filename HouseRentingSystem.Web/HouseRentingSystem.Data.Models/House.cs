namespace HouseRentingSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static Common.EntityValidationConstants.HouseValidation;

    public class House
    {
        public House()
        {
            this.Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(ImageUrlMaxlLength)]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public decimal PricePerMonth { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; } = null!;

        [Required]
        public Guid AgentId { get; set; }

        [ForeignKey(nameof(AgentId))]
        public virtual Agent Agent { get; set; } = null!;

        public Guid? RenterId { get; set; }

        public virtual ApplicationUser? Renter { get; set; }
    }
}
