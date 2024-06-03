param (
    [ValidateSet("ADD", "RUN")]
    [string]$OperationType,
    [string]$MigrationName,
    [string]$Version = "8.0.6"
)

$isEfToolInstalled = Get-Command dotnet-ef -ErrorAction SilentlyContinue
if (-not $isEfToolInstalled) {
    Write-Host ">>> Installing EF Core CLI tools globally..."
    dotnet tool install --global dotnet-ef --version $Version
}
else {
    $efVersionOutput = (dotnet ef --version).Trim()
    $installedVersion = ($efVersionOutput -split "`n")[-1]
    Write-Host ">>> EF Core CLI tools are already installed (Version: $installedVersion)."
    
    if ([version]$installedVersion -lt [version]$Version) {
        Write-Host ">>> Updating EF Core CLI tools to version $Version..."
        dotnet tool update --global dotnet-ef --version $Version
    }
}

$designPackage = "Microsoft.EntityFrameworkCore.Design";
$pathToApiProject = "src\CabaVS.ExpenseTracker.API\CabaVS.ExpenseTracker.API.csproj";
$pathToInfrastructureProject = "src\External\CabaVS.ExpenseTracker.Infrastructure\CabaVS.ExpenseTracker.Infrastructure.csproj";
$pathToMigrationsFolder = "Persistence\Migrations";

Write-Host ">>> Installing NuGet package(s)..."
dotnet add $pathToApiProject package $designPackage --version $Version

if ($OperationType -eq "ADD") {
    Write-Host ">>> Running migrations ADD operation..."
    dotnet ef migrations add $MigrationName --project $pathToInfrastructureProject --output-dir $pathToMigrationsFolder --startup-project $pathToApiProject
}
else {
    Write-Host ">>> Running migrations RUN operation..."
    dotnet ef database update --project $pathToInfrastructureProject --startup-project $pathToApiProject
}

Write-Host ">>> Removing NuGet package(s)..."
dotnet remove $pathToApiProject package $designPackage