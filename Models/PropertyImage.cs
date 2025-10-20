using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateManagementSystem.Models
{
    /// <summary>
    /// Represents an image associated with a property
    /// </summary>
    public class PropertyImage
    {
        [Key]
        public int ImageId { get; set; }

        [Required]
        [ForeignKey("Property")]
        [Display(Name = "Property")]
        public int PropertyId { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        [StringLength(500)]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "Image Caption")]
        public string? Caption { get; set; }

        [Display(Name = "Is Primary")]
        public bool IsPrimary { get; set; } = false;

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; } = 0;

        [Display(Name = "Uploaded Date")]
        public DateTime UploadedDate { get; set; } = DateTime.Now;

        // Navigation property
        public virtual Property? Property { get; set; }
    }
}
