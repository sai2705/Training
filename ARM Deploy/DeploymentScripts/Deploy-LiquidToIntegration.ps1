param (

[string]$IntegrationAccountName,

[string] $ResourceGroupName,

[string] $ResourceLocation = "Central US",

[string] $Path,

[string] $Output
)
$json = $Output | ConvertFrom-Json

Write-Output "***json : $json"

Write-Output "***ResourceGroupName : $ResourceGroupName" 

Write-Output "***Path : $Path"

$IntegrationAccountName = "$json.integrationAccountName.value"



Get-ChildItem "$Path\*" -Recurse -Include *.xslt, *.liquid |
Foreach-Object {
$Content = Get-Content $_.FullName | Out-String
$ResourceName = [System.IO.Path]::GetFileNameWithoutExtension($_.FullName) 

 if($_.Name -like '*.liquid')
{
$PropertiesObject = @{
mapType = "liquid"
content = "$Content"
contentType = "text/plain"
}
}

 elseif($_.Name -like '*.xslt')
{
$PropertiesObject = @{
mapType = "xslt"
content = "$Content"
contentType = "application/xml"
}
}
 New-AzureRmResource -Location $ResourceLocation -PropertyObject $PropertiesObject -ResourceGroupName $ResourceGroupName -ResourceType Microsoft.Logic/integrationAccounts/maps -ResourceName "$IntegrationAccountName/$ResourceName" -ApiVersion 2019-05-01 -Force
}