# Database Setup Instructions

## Quick Setup (Recommended)

The application is configured to automatically create the database when you first run it. However, if you encounter database issues, follow these steps:

### Step 1: Ensure SQL Server LocalDB is Installed

Check if SQL Server LocalDB is available:
```powershell
sqllocaldb info
```

If not installed, download from: https://aka.ms/ssmsfullsetup

### Step 2: Create Database with Migrations

```powershell
# Navigate to project directory
cd RealEstateManagementSystem

# Create migration
dotnet ef migrations add InitialCreate

# Apply migration to database
dotnet ef database update
```

### Step 3: Run the Application

```powershell
dotnet run
```

The application will:
1. Connect to SQL Server LocalDB
2. Create the database if it doesn't exist
3. Apply migrations
4. Seed sample data automatically

---

## Alternative: Manual Database Creation

If Entity Framework migrations don't work, you can create the database manually:

### SQL Script

```sql
CREATE DATABASE RealEstateDB;
GO

USE RealEstateDB;
GO

-- Users Table
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    PhoneNumber NVARCHAR(20),
    Address NVARCHAR(200),
    City NVARCHAR(100),
    State NVARCHAR(100),
    ZipCode NVARCHAR(20),
    ProfileImage NVARCHAR(MAX),
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    LastLogin DATETIME2,
    IsActive BIT NOT NULL DEFAULT 1
);

-- Admins Table
CREATE TABLE Admins (
    AdminId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    PhoneNumber NVARCHAR(20),
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    LastLogin DATETIME2,
    IsSuperAdmin BIT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1
);

-- Properties Table
CREATE TABLE Properties (
    PropertyId INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(2000) NOT NULL,
    PropertyType NVARCHAR(50) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Address NVARCHAR(200) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    State NVARCHAR(100) NOT NULL,
    ZipCode NVARCHAR(20) NOT NULL,
    Bedrooms INT NOT NULL,
    Bathrooms INT NOT NULL,
    Area DECIMAL(18,2) NOT NULL,
    YearBuilt INT,
    GarageSpaces INT,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Available',
    IsFeatured BIT NOT NULL DEFAULT 0,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME2,
    ViewCount INT NOT NULL DEFAULT 0
);

-- PropertyImages Table
CREATE TABLE PropertyImages (
    ImageId INT PRIMARY KEY IDENTITY(1,1),
    PropertyId INT NOT NULL,
    ImageUrl NVARCHAR(500) NOT NULL,
    Caption NVARCHAR(200),
    IsPrimary BIT NOT NULL DEFAULT 0,
    DisplayOrder INT NOT NULL DEFAULT 0,
    UploadedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (PropertyId) REFERENCES Properties(PropertyId) ON DELETE CASCADE
);

-- Inquiries Table
CREATE TABLE Inquiries (
    InquiryId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT,
    PropertyId INT,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20),
    Subject NVARCHAR(200) NOT NULL,
    Message NVARCHAR(2000) NOT NULL,
    InquiryDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    IsRead BIT NOT NULL DEFAULT 0,
    IsResolved BIT NOT NULL DEFAULT 0,
    AdminResponse NVARCHAR(1000),
    ResponseDate DATETIME2,
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE SET NULL,
    FOREIGN KEY (PropertyId) REFERENCES Properties(PropertyId) ON DELETE SET NULL
);

-- Insert Sample Admin
INSERT INTO Admins (Name, Email, PasswordHash, PhoneNumber, IsSuperAdmin, CreatedDate)
VALUES ('System Administrator', 'admin@realestate.com', '$2a$11$YourHashedPasswordHere', '+1-555-0100', 1, GETDATE());

-- Note: Replace the PasswordHash with actual BCrypt hash for 'Admin@123'
```

---

## Connection String Configuration

The default connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RealEstateDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### For SQL Server (not LocalDB):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=RealEstateDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True"
}
```

### For Azure SQL:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=tcp:yourserver.database.windows.net,1433;Database=RealEstateDB;User ID=yourusername;Password=yourpassword;Encrypt=True;Connection Timeout=30;"
}
```

---

## Verify Database Creation

After running the application, verify the database was created:

```powershell
sqlcmd -S "(localdb)\mssqllocaldb" -Q "SELECT name FROM sys.databases WHERE name = 'RealEstateDB'"
```

Or use SQL Server Management Studio (SSMS) to connect to `(localdb)\mssqllocaldb`

---

## Troubleshooting

### Error: "Invalid object name 'Users'"

**Solution**: The database wasn't created. Run migrations:
```powershell
dotnet ef database update
```

### Error: "A network-related or instance-specific error"

**Solution**: SQL Server LocalDB not running. Start it:
```powershell
sqllocaldb start mssqllocaldb
```

### Error: "Login failed for user"

**Solution**: Check connection string credentials or use Trusted_Connection=True for Windows Authentication

### Migrations Not Working

**Solution**: Ensure EF tools are installed:
```powershell
dotnet tool install --global dotnet-ef
```

Then retry:
```powershell
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## Reset Database

To start fresh:

```powershell
# Delete database
dotnet ef database drop

# Recreate
dotnet ef database update
```

Or manually in SQL:
```sql
USE master;
GO
DROP DATABASE IF EXISTS RealEstateDB;
GO
```

Then run the application again to recreate with seed data.

---

## Seed Data

The application automatically seeds:
- 1 Admin account
- 3 User accounts  
- 8 Properties
- Multiple property images
- 4 Sample inquiries

This happens automatically on first run via `DbSeeder.cs` in `Program.cs`.

---

## Production Deployment

For production:

1. Use a proper SQL Server instance (not LocalDB)
2. Update connection string in `appsettings.Production.json`
3. Run migrations on production database
4. Ensure proper security (firewall, SSL, etc.)
5. Consider using Azure SQL or AWS RDS

---

## Notes

- LocalDB is for development only
- Sample data includes hashed passwords (BCrypt)
- Images folder needs write permissions
- Database is created automatically if using EF migrations
- For production, use environment-specific connection strings

---

**Database Status**: Configured and ready to use with automatic creation and seeding.
