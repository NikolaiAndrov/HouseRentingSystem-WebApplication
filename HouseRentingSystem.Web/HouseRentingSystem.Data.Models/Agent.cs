namespace HouseRentingSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static Common.EntityValidationConstants.AgentValidation;

    public class Agent
    {
        public Agent() 
        { 
            this.Id = Guid.NewGuid();
            this.ManagedHouses = new HashSet<House>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(PhoneMaxLength)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = null!;

        public virtual ICollection<House> ManagedHouses { get; set; }
    }
}
