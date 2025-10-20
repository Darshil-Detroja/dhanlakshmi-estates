# Azure Deployment Guide - DhanLakshmi Estates

## 🌐 Complete Guide to Deploy Your Website to Azure

This guide will help you deploy your ASP.NET Core MVC Real Estate Management System to Azure, making it accessible online.

---

## 📋 What You'll Need

1. **Azure Account** (Free tier available)
   - Sign up at: https://azure.microsoft.com/free/
   - You get $200 free credit for 30 days
   - Free services for 12 months

2. **Visual Studio 2022** (Already installed ✓)

3. **Your GitHub Repository** (Optional but recommended)

---

## 🚀 Method 1: Deploy Using Visual Studio 2022 (RECOMMENDED - Easiest)

### Step 1: Create Azure Account

1. Go to https://azure.microsoft.com/free/
2. Click **"Start free"**
3. Sign in with Microsoft account (or create one)
4. Complete the signup process (requires credit card for verification, but won't charge for free tier)

### Step 2: Open Your Project in Visual Studio 2022

1. Open `RealEstateManagementSystem.sln` in Visual Studio 2022
2. Make sure the project builds successfully:
   - Press `Ctrl + Shift + B` or go to **Build → Build Solution**

### Step 3: Publish to Azure

1. **Right-click** on the project name in Solution Explorer
2. Select **"Publish..."**
3. In the Publish dialog:
   - Select **"Azure"** → Click **Next**
   - Select **"Azure App Service (Windows)"** → Click **Next**
   - Click **"Sign in"** (use your Azure account)
   - Click **"Create New"** (green plus button)

4. **Create App Service** dialog:
   ```
   Name: dhanlakshmi-estates (or your preferred name)
   Subscription: Your Azure subscription
   Resource Group: Click "New" → Name: RealEstateRG
   Hosting Plan: Click "New"
     - Name: DhanLakshmiPlan
     - Location: Choose nearest (e.g., East US, West Europe)
     - Size: Free (F1) or Basic (B1)
   ```

5. Click **"Create"**

6. After creation, click **"Finish"**

7. Click **"Publish"** button

8. **Wait for deployment** (5-10 minutes)

### Step 4: Create Azure SQL Database

Since the app needs a database, you'll need to create one:

1. Go to **Azure Portal**: https://portal.azure.com
2. Click **"+ Create a resource"**
3. Search for **"SQL Database"**
4. Click **"Create"**
5. Fill in details:
   ```
   Database name: RealEstateDB
   Server: Click "Create new"
     - Server name: dhanlakshmi-sql (must be unique)
     - Server admin login: sqladmin
     - Password: [Create strong password]
     - Location: Same as your App Service
   Compute + storage: Click "Configure"
     - Select "Basic" tier ($5/month) or
     - Select "Serverless" for pay-as-you-go
   ```
6. Click **"Review + create"** → **"Create"**

### Step 5: Configure Connection String

1. In Azure Portal, go to your **App Service** (dhanlakshmi-estates)
2. Go to **Settings → Configuration**
3. Under **Connection strings**, click **"+ New connection string"**
4. Add:
   ```
   Name: DefaultConnection
   Value: [Copy from your SQL Database connection string]
   Type: SQLAzure
   ```
5. To get the connection string:
   - Go to your SQL Database in Azure Portal
   - Click **"Connection strings"** (left menu)
   - Copy the ADO.NET connection string
   - Replace `{your_password}` with your actual password
6. Click **"Save"**

### Step 6: Apply Database Migrations

**Option A: Using Visual Studio**
1. Open **Package Manager Console** (Tools → NuGet Package Manager → Package Manager Console)
2. Run:
   ```powershell
   Update-Database -Connection "your-azure-connection-string"
   ```

**Option B: Using Azure Portal**
1. In your App Service, go to **Development Tools → Console**
2. Run:
   ```bash
   dotnet ef database update
   ```

### Step 7: Access Your Website

Your website will be live at:
```
https://dhanlakshmi-estates.azurewebsites.net
```

---

## 🚀 Method 2: Deploy Using Azure CLI (Advanced)

### Step 1: Install Azure CLI

Download from: https://aka.ms/installazurecliwindows

### Step 2: Login to Azure

```bash
az login
```

### Step 3: Create Resource Group

```bash
az group create --name RealEstateRG --location eastus
```

### Step 4: Create App Service Plan

```bash
az appservice plan create --name DhanLakshmiPlan --resource-group RealEstateRG --sku B1
```

### Step 5: Create Web App

```bash
az webapp create --name dhanlakshmi-estates --resource-group RealEstateRG --plan DhanLakshmiPlan --runtime "DOTNET:8.0"
```

### Step 6: Deploy Code

```bash
cd c:\Users\HP\OneDrive\Desktop\Real\RealEstateManagementSystem
dotnet publish -c Release -o ./publish
az webapp deployment source config-zip --resource-group RealEstateRG --name dhanlakshmi-estates --src ./publish.zip
```

---

## 🚀 Method 3: Deploy Using GitHub Actions (Automated)

### Step 1: Push to GitHub

```bash
git remote add origin https://github.com/YOUR_USERNAME/RealEstateManagementSystem.git
git push -u origin main
```

### Step 2: Enable GitHub Deployment in Azure

1. In Azure Portal → Your App Service
2. Go to **Deployment Center**
3. Select **GitHub**
4. Authorize GitHub
5. Select your repository and branch (main)
6. Click **Save**

Azure will automatically deploy whenever you push to GitHub!

---

## 🔧 Post-Deployment Configuration

### 1. Upload Logo

After deployment, you need to upload your logo:
1. In Azure Portal → App Service → **Advanced Tools (Kudu)**
2. Go to **Debug console → CMD**
3. Navigate to `site/wwwroot/images/`
4. Drag and drop your logo.png file

### 2. Configure Environment

In App Service → **Configuration → Application settings**, add:
```
ASPNETCORE_ENVIRONMENT = Production
```

### 3. Enable HTTPS

In App Service → **TLS/SSL settings**:
- Enable **HTTPS Only**: ON

### 4. Custom Domain (Optional)

If you have a custom domain:
1. App Service → **Custom domains**
2. Click **"+ Add custom domain"**
3. Follow the verification steps

---

## 💰 Pricing Estimate

### Free Tier Option
- **App Service**: Free (F1) - $0/month
- **SQL Database**: Not available in free tier
- **Total**: $0/month (using in-app database, limited features)

### Basic Tier (Recommended)
- **App Service**: Basic (B1) - ~$13/month
- **SQL Database**: Basic - ~$5/month
- **Total**: ~$18/month

### Free Alternative
Use **Azure SQL Database Serverless** (pay only when used):
- App Service: Free (F1)
- SQL Database: Serverless - ~$0.50-$5/month (based on usage)

---

## 🎯 Quick Start Checklist

- [ ] Create Azure account
- [ ] Open project in Visual Studio 2022
- [ ] Right-click project → Publish
- [ ] Create new Azure App Service
- [ ] Create Azure SQL Database
- [ ] Configure connection string
- [ ] Run database migrations
- [ ] Test website online
- [ ] Upload logo and assets
- [ ] Share your live website URL!

---

## 🌐 Your Website URLs

After deployment, your website will be available at:

**Default Azure URL:**
```
https://dhanlakshmi-estates.azurewebsites.net
```

**Custom Domain** (if configured):
```
https://www.dhanlakshmistates.com
```

---

## 🔍 Troubleshooting

### Issue: Website shows "Service Unavailable"
**Solution**: Check Application Insights logs in Azure Portal

### Issue: Database connection fails
**Solution**: 
1. Verify connection string in Configuration
2. Check SQL Database firewall rules (allow Azure services)
3. Verify database migrations were applied

### Issue: Images not loading
**Solution**: Upload images to wwwroot/images folder using Kudu

### Issue: Login not working
**Solution**: Clear cookies, ensure database has seeded admin user

---

## 📞 Support Resources

- **Azure Documentation**: https://docs.microsoft.com/azure/app-service/
- **Azure Support**: https://azure.microsoft.com/support/
- **Azure Pricing Calculator**: https://azure.microsoft.com/pricing/calculator/

---

## 🎉 Success!

Once deployed, share your website:
```
🌐 Live Website: https://dhanlakshmi-estates.azurewebsites.net

🔐 Login Credentials:
Admin: admin@realestate.com / Admin@123
User: john.doe@example.com / User@123
```

---

**Created for DhanLakshmi Estates Real Estate Management System**
