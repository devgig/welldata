
$versionModule = Import-Module -Name ".\buildVersionModule.psm1" -AsCustomObject
$rootDropLocation = "C:\Repos"

Properties {
    $project_dir = Split-Path $psake.build_script_file
    $build_dir = "$project_dir\build"
    $release_dir = Join-Path $build_dir '\Release';
    $packages_dir = Join-Path $project_dir '\packages';
	
    if(-not $buildNumber) {
        $buildNumber = "0"
        $isTFSBuild = $false
        $isLocalBuild = $true
        $db_environment = "LOCAL"
    }
    else {
        $isTFSBuild = $true
        $isLocalBuild = $false
        $db_environment = "BUILD"
    }

    # Example usage for release builds:
    # .\psake test.ps1 -parameters @{ "version_major" = '3'; "version_minor" = '1'; 'is_release_build' = $true }
    # powershell .\psake test.ps1 -parameters "@{ 'version_major' = '3'; 'version_minor' = '1'; 'is_release_build' = `$true }"
    if(-not $version_major) { $version_major = "2" }
    if(-not $version_minor) { $version_minor = "0" }
    $version = $versionModule.BuildVersionNumber($version_major, $version_minor, $buildNumber, $isTFSBuild)
	$shortVersion = "$version_major.$version_minor"
    $db_version = $version

    $buildTimeStamp =  Get-Date -format s
    $project_name = "Well Data Viewer"
    
    $currentYear = Get-Date -format yyyy
}


Task Clean {
    Write-Host "Creating build directory: $build_dir" -ForegroundColor Green
    if (Test-Path $build_dir) {
        rd $build_dir -rec -force | out-null
    }

    # wait for directory to be finish deleting, now sure why it's not completely synchron
    if(Test-Path $build_dir) {
        Start-Sleep -m 500
    }

    mkdir $build_dir | out-null
    Get-ChildItem $build_dir -Recurse | foreach { if($_.IsReadOnly){$_.IsReadOnly= $false} }
}


Task CommonAssemblyInfoAndDbVersion {
    $versionModule.create_commonAssemblyInfo($version, $project_name, $buildTimeStamp, $buildNumber, "$project_dir\CommonAssemblyInfo.cs", $currentYear)
}



Task CompileRelease -Depends Clean, CommonAssemblyInfoAndDbVersion {
    Write-Host "Building Well Data Viewer" -ForegroundColor Green

    Set-ItemProperty "$project_dir\src\WellData\index.html" -name IsReadOnly -value $false
    Set-ItemProperty ".\src\WellData\WellData.nuspec" -Name IsReadOnly -Value $false
    Set-ItemProperty "$packages_dir\Squirrel.Windows.1.9.1\tools\StubExecutable.exe" -Name IsReadOnly -Value $false
    Set-ItemProperty "$packages_dir\Squirrel.Windows.1.9.1\tools\Squirrel.exe" -Name IsReadOnly -Value $false
    Set-ItemProperty "$packages_dir\Squirrel.Windows.1.9.1\tools\Setup.exe" -Name IsReadOnly -Value $false

    $versionModule.create_WellDataAssemblyInfo($version, "WellData", $buildTimeStamp, $buildNumber, "$project_dir\src\WellData\Properties\AssemblyInfo.cs", $currentYear)
    $svaPublishDir = $rootDropLocation + "\deploy\"
    # $svaStgPublishDir = $rootDropLocation + "\WellData\WellDataStage\"
    # if($is_release_build) {
    #     $svaTstPublishDir = $rootDropLocation + "\WellData\WellDataTest\"
    #     $svaStgPublishDir = $rootDropLocation + "\WellData\WellData\"
    # }

    $versions = $version.Split(".")
    $nugetVersion = ($versions[0..2]) -join(".")
    $nuget = "$packages_dir\NuGet.CommandLine.5.2.0\tools\nuget.exe"
    $squirrel = "$packages_dir\Squirrel.Windows.1.9.1\tools\squirrel.exe"
    buildReleaseProject "WellData.Core"
    buildReleaseProject "WellData.Bootstrap"
    buildReleaseProject "WellData.Core.Data"
    buildReleaseProject "WellData.Core.Services"
    buildReleaseProject "WellData.Ui"
    buildReleaseProject "WellData"

    $srcContents = New-Object xml
    $srcContents.Load(".\src\WellData\WellData.nuspec")
    $srcContents.package.metadata.version = $nugetVersion
    $srcContents.Save(".\src\WellData\WellData.nuspec")
    (Get-Content "$release_dir\WellData\index.html").replace('{VERSION}', $nugetVersion) | Set-Content "$release_dir\WellData\index.html"
    (Get-Content "$release_dir\WellData\WellData.exe.config").replace('http://localhost/deploy', 'https://fervent-mcclintock-317d78.netlify.com/') | Set-Content "$release_dir\WellData\WellData.exe.config"

    mkdir .\Releases | out-null
    Remove-Item .\Releases\*
    Copy-Item $svaPublishDir\* .\Releases
    Copy-Item "$release_dir\WellData\index.html" .\Releases
    Copy-Item "$release_dir\WellData\whitelogo.png" .\Releases

    & "$nuget" pack .\src\WellData\WellData.nuspec
    Write-Host "Running squirrel releasify" -ForegroundColor Green
#    & "$squirrel" --releasify ".\WellData.$nugetVersion.nupkg" --framework-version=net45 --no-msi """/a /f .\tools\DigitalCertificate\certificat.pfx /p AI007 /fd sha256 /tr http://timestamp.digicert.com /td sha256"""
    & "$squirrel" --releasify ".\WellData.$nugetVersion.nupkg" --framework-version=net45 --no-msi 
    Write-Host "Waiting for squirrel releasify to complete" -ForegroundColor Green
    Wait-Process -Name "squirrel"
    Write-Host "squirrel releasify completed" -ForegroundColor Green

    Write-Host "Copying files to drop location" -ForegroundColor Green
    Copy-Item .\Releases\* $svaPublishDir
    Write-Host "Publishing files to Production channel" -ForegroundColor Green
    Remove-Item .\Releases -Recurse -Force | out-null
    Remove-Item .\build -Recurse -Force | out-null
    Write-Host "Folder cleanup complete" -ForegroundColor Green
    # if($is_release_build) {
    #     Write-Host "Publishing files to Test channel" -ForegroundColor Green
    #     Copy-Item .\Releases\* \\localhost\wwwroot\WellData\WellDataTest
    #     Write-Host "Test channel publish complete" -ForegroundColor Green
    # } else {
    #     Write-Host "Publishing files to Alpha channel" -ForegroundColor Green
    #     Copy-Item .\Releases\* \\localhost\wwwroot\WellData\WellDataAlpha
    #     Write-Host "Alpha channel publish complete" -ForegroundColor Green
    # }

    # (Get-Content "$release_dir\WellData\WellData.exe.config").replace('http://localhost/test/', 'http://localhost/prod/') | Set-Content "$release_dir\WellData\WellData.exe.config"
    # #mkdir .\Releases | out-null
    # Remove-Item .\Releases\*
    # Copy-Item $svaStgPublishDir\* .\Releases
    # Copy-Item "$release_dir\WellData\index.html" .\Releases
    # Copy-Item "$release_dir\WellData\whitelogo.png" .\Releases

    # & "$nuget" pack .\src\WellData\WellData.nuspec
    # Write-Host "Running squirrel releasify" -ForegroundColor Green
    # & "$squirrel" --releasify ".\WellData.$nugetVersion.nupkg" --framework-version=net472 --no-msi  """/a /f .\tools\DigitalCertificate\certificat.pfx /p AI007 /fd sha256 /tr http://timestamp.digicert.com /td sha256"""
    # Write-Host "Waiting for squirrel releasify to complete" -ForegroundColor Green
    # Wait-Process -Name "squirrel"
    # Write-Host "squirrel releasify completed" -ForegroundColor Green

    # Write-Host "Copying files to Stage drop location" -ForegroundColor Green
    # Copy-Item .\Releases\* $svaStgPublishDir
    # Write-Host "Publishing files to Staging channel" -ForegroundColor Green
    # Write-Host "Staging channel publish complete" -ForegroundColor Green


}

function buildReleaseProject($projectToBuild) {
	Write-Host "Building $projectToBuild" -ForegroundColor Green

    $fixed_release_dir = "$release_dir\$projectToBuild"
    $fixed_release_dir = $fixed_release_dir.Replace("\", "\\");
    $fixed_release_dir = "$fixed_release_dir\\"

    $scrubbedProjectToBuild = $projectToBuild.Replace(".", "_")
    

	Exec { msbuild "WellData.sln" /t:$scrubbedProjectToBuild /p:Configuration=Release /p:Platform="Any CPU" /p:OutDir=$fixed_release_dir /v:quiet }
}

