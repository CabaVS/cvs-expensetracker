param (
    [string]$loc = "WestEurope"
)

$ErrorActionPreference = "Stop"

$environment = Read-Host "Enter a name for Environment"

$solutionName = "cvs-et"

$rgName = "$solutionName-$environment"
$kvName = "$solutionName-kv-$environment"

Write-Host "Creating a RG and Azure Key Vault..."
New-AzResourceGroup -Name $rgName -Location $loc
New-AzKeyVault -VaultName $kvName -ResourceGroupName $rgName -Location $loc -EnabledForTemplateDeployment

Write-Host "Allowing access to Azure Key Vault for current user..."
$currentSubscriptionId = (Get-AzContext).Subscription.Id
$currentUserId = (Get-AzADUser).Id
New-AzRoleAssignment -ObjectId $currentUserId -RoleDefinitionName "Key Vault Administrator" -Scope "/subscriptions/$currentSubscriptionId/resourceGroups/$rgName/providers/Microsoft.KeyVault/vaults/$kvName"

Write-Host "Waiting for 10 seconds..."
Start-Sleep -Seconds 10

Write-Host "Generating DB user and DB password..."
$sqluser = "cvs-dbuser-" + (-join ((65..90) + (97..122) | Get-Random -Count 10 | ForEach-Object { [char]$_ }))
$sqlpassword = -join ([guid]::NewGuid().ToString().ToCharArray() | Sort-Object { Get-Random })

Write-Host "Inserting generated secrets into Azure Key Vault..."
$sqluserSecure = ConvertTo-SecureString $sqluser -AsPlainText -Force
$sqlpasswordSecure = ConvertTo-SecureString $sqlpassword -AsPlainText -Force
Set-AzKeyVaultSecret -VaultName $kvName -Name "sqluser" -SecretValue $sqluserSecure
Set-AzKeyVaultSecret -VaultName $kvName -Name "sqlpassword" -SecretValue $sqlpasswordSecure

Write-Host "Generated sqluser: $sqluser"
Write-Host "Generated sqlpassword: $sqlpassword"