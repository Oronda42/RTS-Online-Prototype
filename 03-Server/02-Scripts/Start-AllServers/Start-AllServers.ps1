# =======================================================
# 
# NAME: Start-AllServers.ps1
# AUTHOR: Martins, David
# COMMENTS: Starts all RTS Servers
# EXAMPLE : .\Start-AllServers.ps1 -Env "DEV_DAVID"
# CREATION DATE : 19/10/2019
# 
# -------------------------------------------------------
# Version 1.0 : Creation
#
# =======================================================

# -------------------------------------------------------
# Params
# -------------------------------------------------------

Param(	
	[Parameter(Mandatory=$True,Position=1)]
	[string] $Env
)

# -------------------------------------------------------
# Imports
# -------------------------------------------------------



# -------------------------------------------------------
# Functions
# -------------------------------------------------------



# -------------------------------------------------------
# Main
# -------------------------------------------------------

# Get XML configuration file name for the environnement
$XmlConfigurationFileName = "Start-AllServers-"+$Env+".xml"
# Get xml configuration
$ScriptPath = split-path -parent $MyInvocation.MyCommand.Definition
$XmlConfigurationFilePath = $scriptPath+"\"+$XmlConfigurationFileName

Write-Host "[INFO] Reading the configuration file $($XmlConfigurationFilePath)" -ForegroundColor Gray

if(!(Test-Path -path ($XmlConfigurationFilePath))){

    Write-Host "[ERROR] The configuration file $($XmlConfigurationFilePath) doesn't exists" -ForegroundColor Red
    exit;
}

# Get file content
$XmlConfigurationContent = Get-Content ($XmlConfigurationFilePath) -Encoding UTF8

# Create a new XML document and put the XML inside
$XmlConfiguration = New-Object System.XML.XMLDocument
$XmlConfiguration.LoadXml($XmlConfigurationContent)

# For each server in the configuration file
foreach ($Server in $XmlConfiguration.SelectNodes('//Configuration/Servers[@Environnement="'+$Env+'"]/Server')){
    Write-Host ""
    
    # Check if server is configured to start automatically
    if($Server.AutoStart -eq "1"){   
     
        Write-Host "[INFO] Starting server $($Server.Name)" -ForegroundColor Gray
        $Arguments = """$($Server.ServerConfigurationFile)"" $Env $($Server.InitializationPluginName) ""$($Server.Name)"" ""$($Server.ServerCustomConfigurationFile)"""
        Write-Host $Arguments
        $process = start-process -File $($Server.DarkRiftExecutionPath) -ArgumentList $Arguments -PassThru     
        Write-Host "[INFO] Server $($Server.Name) started successfully with PID $($process.Id)" -ForegroundColor Green

    }else{

        Write-Host "[INFO] Server $($Server.Name) is not configured to start automatically" -ForegroundColor DarkYellow

    }

}
