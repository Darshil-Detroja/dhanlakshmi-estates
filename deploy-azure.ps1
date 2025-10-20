# DhanLakshmi Estates - Quick Azure Deployment Script
# Run this script in PowerShell to prepare for deployment

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "  DhanLakshmi Estates - Azure Deployment Setup   " -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""

# Step 1: Check .NET SDK
Write-Host "[1/4] Checking .NET SDK..." -ForegroundColor Yellow
$dotnetVersion = dotnet --version
Write-Host "✓ .NET SDK installed: $dotnetVersion" -ForegroundColor Green

# Step 2: Build the project
Write-Host ""
Write-Host "[2/4] Building the project..." -ForegroundColor Yellow
dotnet build --configuration Release
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Build successful!" -ForegroundColor Green
} else {
    Write-Host "✗ Build failed. Please fix errors first." -ForegroundColor Red
    exit 1
}

# Step 3: Create publish folder
Write-Host ""
Write-Host "[3/4] Creating publish package..." -ForegroundColor Yellow
dotnet publish -c Release -o ./publish
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Publish package created in ./publish folder" -ForegroundColor Green
} else {
    Write-Host "✗ Publish failed" -ForegroundColor Red
    exit 1
}

# Step 4: Check for Azure CLI
Write-Host ""
Write-Host "[4/4] Checking Azure CLI..." -ForegroundColor Yellow
$azureCliInstalled = $false
try {
    az --version 2>&1 | Out-Null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ Azure CLI installed" -ForegroundColor Green
        $azureCliInstalled = $true
    }
} catch {
    Write-Host "⚠ Azure CLI not installed" -ForegroundColor Yellow
    Write-Host "  Download from: https://aka.ms/installazurecliwindows" -ForegroundColor Cyan
}

# Summary
Write-Host ""
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "  Deployment Preparation Complete!" -ForegroundColor Green
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "NEXT STEPS - Choose your deployment method:" -ForegroundColor White
Write-Host ""
Write-Host "METHOD 1: Visual Studio 2022 (RECOMMENDED)" -ForegroundColor Green
Write-Host "  1. Open RealEstateManagementSystem.sln" -ForegroundColor White
Write-Host "  2. Right-click project -> Publish" -ForegroundColor White
Write-Host "  3. Select Azure -> Azure App Service" -ForegroundColor White
Write-Host "  4. Follow the wizard" -ForegroundColor White
Write-Host ""

if ($azureCliInstalled) {
    Write-Host "METHOD 2: Azure CLI (Available)" -ForegroundColor Green
    Write-Host "  az login" -ForegroundColor Cyan
    Write-Host "  az webapp up --name dhanlakshmi-estates --runtime 'DOTNET:8.0'" -ForegroundColor Cyan
} else {
    Write-Host "METHOD 2: Install Azure CLI first" -ForegroundColor Yellow
    Write-Host "  https://aka.ms/installazurecliwindows" -ForegroundColor Cyan
}

Write-Host ""
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "For detailed guide, see: AZURE_DEPLOYMENT_GUIDE.md" -ForegroundColor Cyan
Write-Host ""
Write-Host "Your website will be at:" -ForegroundColor White
Write-Host "https://dhanlakshmi-estates.azurewebsites.net" -ForegroundColor Green
Write-Host ""
Write-Host "==================================================" -ForegroundColor Cyan
