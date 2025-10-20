# DhanLakshmi Estates - Real Estate Management System

A comprehensive full-stack real estate management web application built with ASP.NET Core MVC, featuring property listings, user management, and an admin panel.

## 🌟 Features

### User Features
- **User Registration & Authentication**: Secure login/registration with BCrypt password hashing
- **Property Browsing**: Search and filter properties by type, location, and price
- **Property Details**: View detailed information with image galleries
- **User Profile Management**: Update personal information with admin approval workflow
- **Forgot Password**: Password reset functionality with admin assistance

### Admin Features
- **Admin Dashboard**: Comprehensive overview with statistics
- **Property Management**: Add, edit, delete, and manage property listings
- **User Management**: View and manage user accounts
- **Profile Update Approvals**: Review and approve/reject user profile changes
- **Property Image Management**: Upload and manage multiple images per property

### Visual Features
- **Animated Backgrounds**: Beautiful dark blue gradient animations on home, login, and header
- **Responsive Design**: Mobile-friendly interface using Bootstrap 5
- **Modern UI**: Clean, professional design with smooth animations
- **Logo Support**: Brand logo integration throughout the application

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 8.0 MVC
- **Language**: C#
- **Database**: SQL Server LocalDB
- **ORM**: Entity Framework Core (Code First)
- **Frontend**: HTML5, CSS3, Bootstrap 5, jQuery
- **Authentication**: ASP.NET Core Identity Cookies with BCrypt.Net-Next
- **Icons**: Bootstrap Icons

## 📋 Prerequisites

- Visual Studio 2022 (with ASP.NET and web development workload)
- .NET 8.0 SDK
- SQL Server LocalDB

## 🚀 Getting Started

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/RealEstateManagementSystem.git
   cd RealEstateManagementSystem
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access the application**
   - Open your browser and navigate to: `http://localhost:5207`

### Using Visual Studio 2022

1. Open `RealEstateManagementSystem.sln`
2. Press `F5` or click the green play button to run
3. The application will open in your default browser

## 🔐 Default Login Credentials

### Admin Account
- **Email**: admin@realestate.com
- **Password**: Admin@123

### User Account
- **Email**: john.doe@example.com
- **Password**: User@123

## 📁 Project Structure

```
RealEstateManagementSystem/
├── Controllers/          # MVC Controllers
├── Data/                # Database Context
├── Models/              # Entity Models & ViewModels
├── Views/               # Razor Views
│   ├── Account/        # Login, Register, Profile
│   ├── Home/           # Home page
│   ├── Property/       # Property listings
│   ├── AdminDashboard/ # Admin dashboard
│   ├── AdminProperty/  # Property management
│   └── AdminUser/      # User management
├── wwwroot/            # Static files
│   ├── css/           # Stylesheets
│   ├── js/            # JavaScript files
│   └── images/        # Images and logo
└── Migrations/         # EF Core migrations
```

## 🎨 Key Features Explanation

### Animated Backgrounds
The application features stunning dark blue gradient animations on:
- **Home Page**: Smooth gradient wave with floating particles
- **Login Page**: Matching animated background for cohesive design
- **Header/Navbar**: Animated gradient with shimmer effect

### Profile Update Workflow
Users can request profile updates which require admin approval:
1. User submits profile changes
2. Changes stored as pending JSON
3. Admin reviews side-by-side comparison
4. Admin approves or rejects changes
5. User profile updated upon approval

### Property Management
Admins can manage properties with:
- Multiple image uploads per property
- Primary image selection
- Featured property marking
- Detailed property information (bedrooms, bathrooms, area, etc.)
- Status management (Available, Sold, Pending)

## 🗄️ Database Schema

Main entities:
- **Admin**: Administrator accounts
- **User**: Customer accounts
- **Property**: Property listings
- **PropertyImage**: Property images
- **Inquiry**: Customer inquiries
- **Appointment**: Property viewing appointments

## 🔧 Configuration

### Connection String
Update `appsettings.json` if needed:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RealEstateDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

## 📝 Development Notes

- Sample data is automatically seeded on first run
- Hot reload enabled with `dotnet watch run`
- Logo file should be placed in `wwwroot/images/logo.png`
- Brand colors: Green (#0f7c3e, #1a8f4a) and Gold (#c79a2e, #d4af37)

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is open source and available under the [MIT License](LICENSE).

## 👨‍💻 Author

Created as a comprehensive real estate management solution.

## 🙏 Acknowledgments

- Bootstrap for responsive design framework
- Entity Framework Core for ORM
- Bootstrap Icons for iconography
- ASP.NET Core team for the excellent framework

---

**Note**: This is a demonstration project. For production use, implement proper security measures, email services, and additional features as needed.
