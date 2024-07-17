param (
    [Parameter(Mandatory=$true)]
    [string]$upn,
    
    [Parameter(Mandatory=$true)]
    [string]$rgName
)

$roleName = "Key Vault Bicep deployment operator"
$subscriptionId = (Get-AzContext).Subscription.Id 

Write-Host "Verifying that custom role is exist under selected subscription..."
$roleDefinition = Get-AzRoleDefinition -Name $roleName

Write-Host $roleDefinition

if ($roleDefinition) {
    Write-Host "Custom role already created."
    
    Write-Host "Verifying if role is already assigned to a user..."
    $roleAssignment = Get-AzRoleAssignment -RoleDefinitionName $roleName -SignInName $upn -Scope "/subscriptions/$subscriptionId"
    
    if ($roleAssignment) {
        Write-Host "Role is already assigned to a user."
        exit
    }
}
else {
    Write-Host "Creating a custom role..."
    
    $content = Get-Content -Path ".\.azure\kv-bicep-deployment-operator-role.json" -Raw
    $updatedContent = $content -replace "00000000-0000-0000-0000-000000000000", $subscriptionId
    
    $tempFile = New-TemporaryFile
    
    Set-Content -Path $tempFile.FullName -Value $updatedContent
    
    New-AzRoleDefinition -InputFile $tempFile.FullName
    
    Remove-Item -Path $tempFile.FullName
}

Write-Host "Assigning a role to a user..."
New-AzRoleAssignment -ResourceGroupName $rgName -RoleDefinitionName $roleName -SignInName $upn