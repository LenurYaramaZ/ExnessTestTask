$SolutionDir = "$PSScriptRoot\RestServiceTests.sln"
$PackagesDir = "$PSScriptRoot\packages"
$NugetDir = "$PSScriptRoot\Nuget"
$AutoTestAppOutDir = "$PSScriptRoot\AutoTestApp\bin\Debug"
$TestResults = "$PSScriptRoot\TestResults"
$WebRequest = "http://localhost:8080/api/VendorInformation/"
$ServiceName = "VendorService"

import-module WebAdministration

Function Find-MsBuild([int] $MaxVersion = 2017){
    $agentPath = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\msbuild.exe"
    $devPath = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\msbuild.exe"
    $proPath = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\msbuild.exe"
    $communityPath = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe"
	$communityPath64 = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\amd64\msbuild.exe"
    $fallback2015Path = "${Env:ProgramFiles(x86)}\MSBuild\14.0\Bin\MSBuild.exe"
    $fallback2013Path = "${Env:ProgramFiles(x86)}\MSBuild\12.0\Bin\MSBuild.exe"
    $fallbackPath = "C:\Windows\Microsoft.NET\Framework\v4.0.30319"
		
    If ((2017 -le $MaxVersion) -And (Test-Path $agentPath)) { return $agentPath } 
    If ((2017 -le $MaxVersion) -And (Test-Path $devPath)) { return $devPath } 
    If ((2017 -le $MaxVersion) -And (Test-Path $proPath)) { return $proPath } 
    If ((2017 -le $MaxVersion) -And (Test-Path $communityPath)) { return $communityPath }
	If ((2017 -le $MaxVersion) -And (Test-Path $communityPath64)) { return $communityPath64 }	
    If ((2015 -le $MaxVersion) -And (Test-Path $fallback2015Path)) { return $fallback2015Path } 
    If ((2013 -le $MaxVersion) -And (Test-Path $fallback2013Path)) { return $fallback2013Path } 
    If (Test-Path $fallbackPath) { return $fallbackPath } 
        
    throw "Yikes - Unable to find msbuild"
}

Function BuildSolution {
    param
    (
        [parameter(Mandatory=$true)]
        [String] $path,

        [parameter(Mandatory=$false)]
        [bool] $nuget = $true,
        
        [parameter(Mandatory=$false)]
        [bool] $clean = $true
    )
    process
    {
	    $MsBuildExe = Find-MsBuild

        if ($nuget)
		{
            Write-Host "Restoring NuGet packages" -foregroundcolor green
            nuget restore "$($path)"
        }

        if ($clean)
		{
            Write-Host "Cleaning $($path)" -foregroundcolor green
            & "$($MsBuildExe)" "$($path)" /t:Clean /m
        }
		
        Write-Host "Building $($path)" -foregroundcolor green
        & "$($MsBuildExe)" "$($path)" /t:Build /m
    }
}

Function AddTo-SystemPath {            
	Param(
	[array]$PathToAdd
	)
	
	$VerifiedPathsToAdd = $Null

	Foreach($Path in $PathToAdd)
	{            
		if($env:Path -like "*$Path*")
		{
			Write-Host "Currnet item in path is: $Path" -foregroundcolor green
			Write-Host "$Path already exists in Path statement" -foregroundcolor green
		}
		else
		{
			$VerifiedPathsToAdd += ";$Path"
			Write-Host "`$VerifiedPathsToAdd updated to contain: $Path" -foregroundcolor green
		}
		
		if($VerifiedPathsToAdd -ne $null)
		{
			Write-Host "`$VerifiedPathsToAdd contains: $verifiedPathsToAdd" -foregroundcolor green
			Write-Host "Adding $Path to Path statement now..." -foregroundcolor green
			[Environment]::SetEnvironmentVariable("Path",$env:Path + $VerifiedPathsToAdd,"Process")
		}
	}
}

Function AddTo-IISSite {            

 Param(
    [parameter(Mandatory=$true)]
    [String] $siteName,

	[parameter(Mandatory=$true)]
    [String] $sitePhysicalPath,
	
	[parameter(Mandatory=$false)]
    [String] $siteDomainName = "localhost",
	
    [parameter(Mandatory=$false)]
    [String] $port = "8080",
    
    [parameter(Mandatory=$false)]
    [String] $iisAppPoolDotNetVersion = "v4.0"
  )
 Process
 {
	#navigate to the app pools root
	cd IIS:\AppPools\
	
	#check if the app pool exists
	if (!(Test-Path $siteName -pathType container))
	{
		#create the app pool
		$appPool = New-Item $siteName
		$appPool | Set-ItemProperty -Name "managedRuntimeVersion" -Value $iisAppPoolDotNetVersion
		Write-Host "Site was successfully added to IIS pool" -foregroundcolor green
	}
	else
	{
		Write-Host "Site $siteName has already been added to IIS Pool" -foregroundcolor green
	}
	
	#navigate to the sites root
	cd IIS:\Sites\
	
	#check if the site exists
	if (Test-Path $siteName -pathType container)
	{
		Write-Host "Site $siteName has already been added to IIS Sites" -foregroundcolor green
	}
	else
	{
		#create the site
		$iisApp = New-Item $siteName -bindings @{protocol="http";bindingInformation=":" + $port + ":" + $siteDomainName} -physicalPath $sitePhysicalPath
		$iisApp | Set-ItemProperty -Name "applicationPool" -Value $siteName
		Write-Host "Site was successfully added to IIS Sitest" -foregroundcolor green
	}
 }
}

Function CheckServiceStatus {

	$response = try { Invoke-WebRequest $WebRequest }
				catch { $_.Exception.Response.StatusCode.Value__ }

	If ($response -eq '404') {
	  Write-Host "Site is OK!" -foregroundcolor green
	}
	Else {
	  Write-Host "The Site may be down, please check! $WebRequest"  -foregroundcolor red
	  Break
	}
}

Function StartStopSite {
    param
    (
	    [parameter(Mandatory=$true)]
        [String] $siteName,
	
        [parameter(Mandatory=$true)]
        [String] $action
    )
    process
    {
		if (Test-Path $siteName)
		{
			try
			{
				if($action -eq "Start")
				{
					Start-WebAppPool -Name $siteName
					Start-WebSite -Name $siteName
				}
				else
				{
					Stop-WebAppPool -Name $siteName
					Stop-WebSite -Name $siteName
				}
				Write-Host "Site was successfully $action" -foregroundcolor green
			}
			catch
			{
				Write-Host "Can't $action site" -foregroundcolor red
				Break
			}
		}
		else
		{
			Write-Host "Site $siteName is not exist yet" -foregroundcolor green
		}
    }
}
 
#Add items to the path
AddTo-SystemPath $NugetDir, "$PackagesDir\ReportUnit.1.2.1\tools", "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\amd64"

# Stop IIS before building project
StartStopSite $ServiceName "Stop"

#Restoring NuGet packages; Build solution
BuildSolution $SolutionDir -clean $false

#Add site to iis pool and sites
AddTo-IISSite $ServiceName -sitePhysicalPath "$PSScriptRoot\VendorService" 

#Start IIS
StartStopSite $ServiceName "Start"

# Check ServiceStatus
CheckServiceStatus

# Install NUnit Test Runner
$nuget = "$NugetDir\nuget.exe"
& $nuget install NUnit.Runners -ExcludeVersion -o $PackagesDir

# Set nunit path test runner
$nunit = "$PackagesDir\NUnit.ConsoleRunner\tools\nunit3-console.exe"

#Find tests in AutoTestAppOutDir
$tests = (Get-ChildItem $AutoTestAppOutDir -Recurse -Include *AutoTestApp.dll)

# Run NUnit3 tests
& $nunit $tests --noheader --framework=net-4.5 --labels=all --work=$TestResults 

# Generate HtmlReport
Invoke-Expression "& ReportUnit $TestResults $TestResults"

# Open HtmlReport
Invoke-Item $TestResults\TestResult.html

Read-Host -Prompt "Press Enter to exit"