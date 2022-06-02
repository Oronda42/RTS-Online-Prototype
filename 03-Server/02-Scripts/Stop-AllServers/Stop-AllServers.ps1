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
$XmlConfigurationFileName = "Stop-AllServers-"+$Env+".xml"
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
$processCommand = "test"

# For each server in the configuration file
foreach ($ServerHost in $XmlConfiguration.SelectNodes('//Configuration/Hosts[@Environnement="'+$Env+'"]/Host')){
    Write-Host ""
    
    if($ServerHost.IP -eq "127.0.0.1" -or $ServerHost.IP -eq "localhost"){

        Write-Host "[INFO] Stopping servers on $($ServerHost.Name)" -ForegroundColor Gray
        $process = Get-Process "*$($ServerHost.ExecutableName)"
        $process.CloseMainWindow()
        Write-Host "[INFO] Servers stopped successfully on $($ServerHost.Name)" -ForegroundColor Green
        
    }
    else{
        #Remote kill
        
    }

}
