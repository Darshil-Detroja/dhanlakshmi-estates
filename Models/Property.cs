using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateManagementSystem.Models
{
    /// <summary>
    /// Represents a real estate property in the system
    /// </summary>
    public class Property
    {
        [Key]
        public int PropertyId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Property type is required")]
        [StringLength(50)]
        [Display(Name = "Property Type")]
        public string PropertyType { get; set; } = string.Empty; // House, Apartment, Condo, Land, Commercial

        [Required(ErrorMessage = "Price is required")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required")]
        [StringLength(100)]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Zip code is required")]
        [StringLength(20)]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bedrooms is required")]
        [Range(0, 100, ErrorMessage = "Bedrooms must be between 0 and 100")]
        public int Bedrooms { get; set; }

        [Required(ErrorMessage = "Bathrooms is required")]
        [Range(0, 100, ErrorMessage = "Bathrooms must be between 0 and 100")]
        public int Bathrooms { get; set; }

        [Required(ErrorMessage = "Area is required")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Area must be a positive number")]
        [Display(Name = "Area (sq ft)")]
        public decimal Area { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Year built must be a positive number")]
        [Display(Name = "Year Built")]
        public int? YearBuilt { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Garage spaces must be a positive number")]
        [Display(Name = "Garage Spaces")]
        public int? GarageSpaces { get; set; }

        [StringLength(50)]
        [Display(Name = "Listing Status")]
        public string Status { get; set; } = "Available"; // Available, Sold, Pending, Rented

        [Display(Name = "Is Featured")]
        public bool IsFeatured { get; set; } = false;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "View Count")]
        public int ViewCount { get; set; } = 0;

        // Full address for display
        [NotMapped]
        public string FullAddress => $"{Address}, {City}, {State} {ZipCode}";

        // Navigation property for property images
        public virtual ICollection<PropertyImage>? PropertyImages { get; set; }

        // Navigation property for inquiries about this property
        public virtual ICollection<Inquiry>? Inquiries { get; set; }
    }
}
