param (
    [ValidateSet("ADD", "RUN")]
    [string]$OperationType,
    [string]$MigrationName,
    [string]$Version = "9.0.3"
)

$ErrorActionPreference = "Stop"

$toolName = "dotnet-ef"

$designPackage = "Microsoft.EntityFrameworkCore.Design";
$pathToApiProject = "src\CabaVS.ExpenseTracker.API\CabaVS.ExpenseTracker.API.csproj";
$pathToInfrastructureProject = "src\External\CabaVS.ExpenseTracker.Persistence\CabaVS.ExpenseTracker.Persistence.csproj";
$pathToMigrationsFolder = "Migrations";

$toolInstalled = dotnet tool list -g | Select-String -Pattern $toolName
if (-not $toolInstalled) {
    Write-Host "'$toolName' is not installed. Installing..."
    dotnet tool install -g $toolName
} else {
    Write-Host "'$toolName' is already installed. Checking for updates..."
    dotnet tool update -g $toolName
}

Write-Host "Installing '$designPackage' into API project..."
dotnet add $pathToApiProject package $designPackage --version $Version

if ($OperationType -eq "ADD") {
    Write-Host "Adding migration..."
    dotnet ef migrations add $MigrationName --project $pathToInfrastructureProject --output-dir $pathToMigrationsFolder --startup-project $pathToApiProject
}
else {
    Write-Host "Updating database..."
    dotnet ef database update --project $pathToInfrastructureProject --startup-project $pathToApiProject
}

Write-Host "Uninstalling '$designPackage' from API project..."
dotnet remove $pathToApiProject package $designPackage