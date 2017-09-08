param($installPath, $toolsPath, $package, $project)

Write-Host "Staging Umbraco Data - For Item Setup" 
Write-Host "Install Path :", "$installPath"
Write-Host "Tools Path   :", "$toolsPath"
Write-Host "" 

if ($project) {

	$projectPath = (Get-Item $project.Properties.Item("FullPath").Value).FullName
	Write-Host "projectPath :" "${projectPath}"
	
	$dataPath = Join-Path $projectPath "App_Data\Aubergine\"
	Write-Host "dataPath    :" "${dataPath}"

	$dataSource = JointPath $installPath "Data\"
	Write-Host "dataSource  :" "${dataSource}"

	New-Item -ItemType Directory -Force -Path $dataPath
	robocopy $dataSource $dataPath /is /it/ e
}