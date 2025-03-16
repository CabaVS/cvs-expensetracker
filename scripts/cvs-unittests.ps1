$ErrorActionPreference = "Stop"

$unitTestsPath = ".\tests\CabaVS.ExpenseTracker.UnitTests\CabaVS.ExpenseTracker.UnitTests.csproj";

$toolName = "dotnet-reportgenerator-globaltool"

$toolInstalled = dotnet tool list -g | Select-String -Pattern $toolName
if (-not $toolInstalled) {
    Write-Host "'$toolName' is not installed. Installing..."
    dotnet tool install -g $toolName
} else {
    Write-Host "'$toolName' is already installed. Checking for updates..."
    dotnet tool update -g $toolName
}

$reportPath = Join-Path -Path $env:TEMP -ChildPath "cvs-expensetracker-code-coverage-report"
if (-not (Test-Path -Path $reportPath)) {
    Write-Host "Creating a temporary folder with path '$reportPath'..."
    New-Item -Path $reportPath -ItemType Directory
}
else {
    Write-Host "A temporary folder with path '$reportPath' already exists. Removing its content..."
    Remove-Item -Path "$reportPath\*" -Recurse -Force
}

Write-Host "Launching the Unit Tests..."
dotnet test $unitTestsPath --verbosity minimal --collect:"XPlat Code Coverage" --results-directory $reportPath

Write-Host "Generating a code coverage report..."
reportgenerator -reports:"$reportPath\**\coverage.cobertura.xml" -targetdir:"$reportPath\generated-report" -reporttypes:Html

Write-Host "Launching the report..."
Start-Process "$reportPath\generated-report\index.html"