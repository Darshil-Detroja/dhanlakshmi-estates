using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateManagementSystem.Models
{
    /// <summary>
    /// Represents an inquiry/contact message from a user
    /// </summary>
    public class Inquiry
    {
        [Key]
        public int InquiryId { get; set; }

        [ForeignKey("User")]
        [Display(Name = "User")]
        public int? UserId { get; set; }

        [ForeignKey("Property")]
        [Display(Name = "Property")]
        public int? PropertyId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(20)]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Message is required")]
        [StringLength(2000)]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; } = string.Empty;

        [Display(Name = "Inquiry Date")]
        public DateTime InquiryDate { get; set; } = DateTime.Now;

        [Display(Name = "Is Read")]
        public bool IsRead { get; set; } = false;

        [Display(Name = "Is Resolved")]
        public bool IsResolved { get; set; } = false;

        [StringLength(1000)]
        [Display(Name = "Admin Response")]
        [DataType(DataType.MultilineText)]
        public string? AdminResponse { get; set; }

        [Display(Name = "Response Date")]
        public DateTime? ResponseDate { get; set; }

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual Property? Property { get; set; }
    }
}
