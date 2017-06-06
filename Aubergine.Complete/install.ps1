param($installPath, $toolsPath, $package, $project)

Write-Host "Install Path:" "$installPath"
Write-Host "Tools Path  :" "$toolsPath"

Write-Host " "

if ($project) {
 	# Create paths and list them
	$projectPath = (Get-Item $project.Properties.Item("FullPath").Value).FullName
	Write-Host "projectPath:" "${projectPath}"

	$dataPath = Join-Path $projectPath "App_Data\Aubergine\"
	Write-Host "dataPath:" "${dataPath}"

	$dataSource = Join-Path $installPath "Data\"		
	Write-Host "dataSource:" "${dataPath}"


    New-Item -ItemType Directory -Force -Path $dataPath
    robocopy $dataSource $dataPath /is /it /e

}