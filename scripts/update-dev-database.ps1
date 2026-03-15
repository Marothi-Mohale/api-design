param()

$ErrorActionPreference = "Stop"
$env:DOTNET_CLI_HOME = "C:\Users\ASUS\Documents\API DESIGN\.dotnet"
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"
$env:DOTNET_NOLOGO = "1"

Write-Host "Restoring local tools..."
dotnet tool restore

Write-Host "Applying EF Core migrations..."
dotnet tool run dotnet-ef database update `
  --project .\TutoringPlatform.Infrastructure\TutoringPlatform.Infrastructure.csproj `
  --startup-project .\TutoringPlatform.Api\TutoringPlatform.Api.csproj
