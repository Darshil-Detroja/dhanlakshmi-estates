using RealEstateManagementSystem.Data;
using RealEstateManagementSystem.Models;

namespace RealEstateManagementSystem.Data
{
    /// <summary>
    /// Seeds the database with initial sample data for demonstration
    /// </summary>
    public static class DbSeeder
    {
        public static void SeedData(ApplicationDbContext context)
        {
            // Check if data already exists
            if (context.Users.Any() || context.Admins.Any() || context.Properties.Any())
            {
                return; // Database has been seeded
            }

            // Seed Admin
            var admin = new Admin
            {
                Name = "Admin",
                Email = "admin@realestate.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                PhoneNumber = "+1-555-0100",
                IsSuperAdmin = true,
                IsActive = true,
                CreatedDate = DateTime.Now
            };
            context.Admins.Add(admin);

            // Seed Users
            var users = new List<User>
            {
                new User
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
                    PhoneNumber = "+1-555-0101",
                    Address = "123 Main St",
                    City = "New York",
                    State = "NY",
                    ZipCode = "10001",
                    IsActive = true,
                    CreatedDate = DateTime.Now.AddDays(-30)
                },
                new User
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
                    PhoneNumber = "+1-555-0102",
                    Address = "456 Oak Ave",
                    City = "Los Angeles",
                    State = "CA",
                    ZipCode = "90001",
                    IsActive = true,
                    CreatedDate = DateTime.Now.AddDays(-25)
                },
                new User
                {
                    FirstName = "Michael",
                    LastName = "Johnson",
                    Email = "michael.j@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
                    PhoneNumber = "+1-555-0103",
                    Address = "789 Pine Rd",
                    City = "Chicago",
                    State = "IL",
                    ZipCode = "60601",
                    IsActive = true,
                    CreatedDate = DateTime.Now.AddDays(-20)
                }
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            // Seed Properties
            var properties = new List<Property>
            {
                new Property
                {
                    Title = "Luxury Modern Villa with Pool",
                    Description = "Stunning 5-bedroom villa featuring contemporary design, spacious living areas, gourmet kitchen, and a beautiful backyard with pool. Perfect for families seeking luxury living.",
                    PropertyType = "House",
                    Price = 1250000,
                    Address = "100 Beverly Hills Blvd",
                    City = "Los Angeles",
                    State = "CA",
                    ZipCode = "90210",
                    Bedrooms = 5,
                    Bathrooms = 4,
                    Area = 4500,
                    YearBuilt = 2020,
                    GarageSpaces = 3,
                    Status = "Available",
                    IsFeatured = true,
                    CreatedDate = DateTime.Now.AddDays(-15),
                    ViewCount = 245
                },
                new Property
                {
                    Title = "Downtown Luxury Apartment",
                    Description = "Elegant 2-bedroom apartment in the heart of the city. Modern finishes, floor-to-ceiling windows with stunning views, and access to premium amenities including gym and rooftop terrace.",
                    PropertyType = "Apartment",
                    Price = 575000,
                    Address = "250 Park Avenue",
                    City = "New York",
                    State = "NY",
                    ZipCode = "10017",
                    Bedrooms = 2,
                    Bathrooms = 2,
                    Area = 1200,
                    YearBuilt = 2019,
                    GarageSpaces = 1,
                    Status = "Available",
                    IsFeatured = true,
                    CreatedDate = DateTime.Now.AddDays(-12),
                    ViewCount = 189
                },
                new Property
                {
                    Title = "Charming Suburban Family Home",
                    Description = "Beautiful 4-bedroom home in a quiet suburban neighborhood. Large backyard, updated kitchen, hardwood floors throughout. Close to top-rated schools and parks.",
                    PropertyType = "House",
                    Price = 485000,
                    Address = "567 Maple Street",
                    City = "Austin",
                    State = "TX",
                    ZipCode = "78701",
                    Bedrooms = 4,
                    Bathrooms = 3,
                    Area = 2800,
                    YearBuilt = 2015,
                    GarageSpaces = 2,
                    Status = "Available",
                    IsFeatured = false,
                    CreatedDate = DateTime.Now.AddDays(-10),
                    ViewCount = 156
                },
                new Property
                {
                    Title = "Waterfront Condo with Marina Views",
                    Description = "Spectacular 3-bedroom waterfront condo with breathtaking marina views. High-end finishes, spacious balcony, and access to private beach and boat slip.",
                    PropertyType = "Condo",
                    Price = 825000,
                    Address = "88 Harbor Way",
                    City = "Miami",
                    State = "FL",
                    ZipCode = "33101",
                    Bedrooms = 3,
                    Bathrooms = 2,
                    Area = 1850,
                    YearBuilt = 2018,
                    GarageSpaces = 2,
                    Status = "Available",
                    IsFeatured = true,
                    CreatedDate = DateTime.Now.AddDays(-8),
                    ViewCount = 312
                },
                new Property
                {
                    Title = "Cozy Starter Home",
                    Description = "Perfect starter home with 3 bedrooms and 2 bathrooms. Recently renovated with new appliances, fresh paint, and updated fixtures. Move-in ready!",
                    PropertyType = "House",
                    Price = 325000,
                    Address = "234 Elm Street",
                    City = "Phoenix",
                    State = "AZ",
                    ZipCode = "85001",
                    Bedrooms = 3,
                    Bathrooms = 2,
                    Area = 1600,
                    YearBuilt = 2010,
                    GarageSpaces = 1,
                    Status = "Available",
                    IsFeatured = false,
                    CreatedDate = DateTime.Now.AddDays(-6),
                    ViewCount = 98
                },
                new Property
                {
                    Title = "Prime Commercial Office Space",
                    Description = "Premium commercial office space in downtown business district. Modern infrastructure, ample parking, and excellent visibility. Ideal for corporate headquarters.",
                    PropertyType = "Commercial",
                    Price = 2100000,
                    Address = "1000 Business Center Dr",
                    City = "Seattle",
                    State = "WA",
                    ZipCode = "98101",
                    Bedrooms = 0,
                    Bathrooms = 6,
                    Area = 8500,
                    YearBuilt = 2021,
                    GarageSpaces = 25,
                    Status = "Available",
                    IsFeatured = false,
                    CreatedDate = DateTime.Now.AddDays(-5),
                    ViewCount = 167
                },
                new Property
                {
                    Title = "Luxury Penthouse Suite",
                    Description = "Exclusive penthouse with panoramic city views. 4 bedrooms, 4.5 bathrooms, private elevator access, chef's kitchen, and expansive terrace perfect for entertaining.",
                    PropertyType = "Apartment",
                    Price = 3500000,
                    Address = "777 Skyline Tower",
                    City = "San Francisco",
                    State = "CA",
                    ZipCode = "94102",
                    Bedrooms = 4,
                    Bathrooms = 4,
                    Area = 3500,
                    YearBuilt = 2022,
                    GarageSpaces = 3,
                    Status = "Available",
                    IsFeatured = true,
                    CreatedDate = DateTime.Now.AddDays(-4),
                    ViewCount = 428
                },
                new Property
                {
                    Title = "Historic Victorian Mansion",
                    Description = "Beautifully restored Victorian mansion with original architectural details. 6 bedrooms, 5 bathrooms, library, wine cellar, and manicured gardens on 2 acres.",
                    PropertyType = "House",
                    Price = 1875000,
                    Address = "345 Heritage Lane",
                    City = "Boston",
                    State = "MA",
                    ZipCode = "02101",
                    Bedrooms = 6,
                    Bathrooms = 5,
                    Area = 5200,
                    YearBuilt = 1895,
                    GarageSpaces = 2,
                    Status = "Pending",
                    IsFeatured = true,
                    CreatedDate = DateTime.Now.AddDays(-3),
                    ViewCount = 276
                }
            };
            context.Properties.AddRange(properties);
            context.SaveChanges();

            // Seed Property Images
            var propertyImages = new List<PropertyImage>();
            
            // Images for Luxury Modern Villa
            propertyImages.AddRange(new[]
            {
                new PropertyImage { PropertyId = 1, ImageUrl = "/images/properties/villa1-1.jpg", Caption = "Front View", IsPrimary = true, DisplayOrder = 1 },
                new PropertyImage { PropertyId = 1, ImageUrl = "/images/properties/villa1-2.jpg", Caption = "Living Room", IsPrimary = false, DisplayOrder = 2 },
                new PropertyImage { PropertyId = 1, ImageUrl = "/images/properties/villa1-3.jpg", Caption = "Pool Area", IsPrimary = false, DisplayOrder = 3 }
            });

            // Images for Downtown Luxury Apartment
            propertyImages.AddRange(new[]
            {
                new PropertyImage { PropertyId = 2, ImageUrl = "/images/properties/apt1-1.jpg", Caption = "City View", IsPrimary = true, DisplayOrder = 1 },
                new PropertyImage { PropertyId = 2, ImageUrl = "/images/properties/apt1-2.jpg", Caption = "Kitchen", IsPrimary = false, DisplayOrder = 2 },
                new PropertyImage { PropertyId = 2, ImageUrl = "/images/properties/apt1-3.jpg", Caption = "Master Bedroom", IsPrimary = false, DisplayOrder = 3 }
            });

            // Images for other properties (simplified)
            for (int i = 3; i <= 8; i++)
            {
                propertyImages.Add(new PropertyImage 
                { 
                    PropertyId = i, 
                    ImageUrl = $"/images/properties/property{i}-1.jpg", 
                    Caption = "Main View", 
                    IsPrimary = true, 
                    DisplayOrder = 1 
                });
            }

            context.PropertyImages.AddRange(propertyImages);
            context.SaveChanges();

            // Seed Inquiries
            var inquiries = new List<Inquiry>
            {
                new Inquiry
                {
                    UserId = 1,
                    PropertyId = 1,
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    Phone = "+1-555-0101",
                    Subject = "Inquiry about Luxury Modern Villa",
                    Message = "I'm very interested in scheduling a viewing of this property. Are weekends available?",
                    InquiryDate = DateTime.Now.AddDays(-3),
                    IsRead = true,
                    IsResolved = false
                },
                new Inquiry
                {
                    UserId = 2,
                    PropertyId = 2,
                    Name = "Jane Smith",
                    Email = "jane.smith@example.com",
                    Phone = "+1-555-0102",
                    Subject = "Downtown Apartment Availability",
                    Message = "Is this apartment still available? I'd like to know more about the HOA fees and amenities.",
                    InquiryDate = DateTime.Now.AddDays(-2),
                    IsRead = true,
                    IsResolved = true,
                    AdminResponse = "Yes, the apartment is still available. HOA fees are $450/month and include gym, pool, and concierge services. Would you like to schedule a viewing?",
                    ResponseDate = DateTime.Now.AddDays(-1)
                },
                new Inquiry
                {
                    UserId = 3,
                    PropertyId = 4,
                    Name = "Michael Johnson",
                    Email = "michael.j@example.com",
                    Phone = "+1-555-0103",
                    Subject = "Waterfront Condo Question",
                    Message = "Does the condo come with a boat slip? Also, what are the property taxes?",
                    InquiryDate = DateTime.Now.AddDays(-1),
                    IsRead = false,
                    IsResolved = false
                },
                new Inquiry
                {
                    UserId = null,
                    PropertyId = null,
                    Name = "Sarah Williams",
                    Email = "sarah.w@example.com",
                    Phone = "+1-555-0104",
                    Subject = "General Inquiry",
                    Message = "I'm looking for a 3-bedroom house in the $400-500k range. Do you have any recommendations?",
                    InquiryDate = DateTime.Now,
                    IsRead = false,
                    IsResolved = false
                }
            };
            context.Inquiries.AddRange(inquiries);
            context.SaveChanges();
        }
    }
}
