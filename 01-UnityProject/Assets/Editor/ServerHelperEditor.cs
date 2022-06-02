using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;

public class ServerHelperEditor : Editor
{

    private static string GetConfigigurationForUser(string pUSer)
    {
        switch (pUSer)
        {
            case "DESKTOP-S5IF8LQ\\cyber":
                return "DEV_DAVID";
            case "DESKTOP-6I1PK2P\\David":
                return "DEV_DAVID_PORTABLE";
            case "DESKTOP-R97JL63\\Fax37":
                return "DEV_FABIEN";
            case "DESKTOP-TS7B98M\\Lsday":
                return "DEV_OLIVIER";
            default:
                return "";
        }
    }

    [MenuItem("RTS/00 - Server/Start")]
    public static void StartServer()
    {
        string user = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        string configurationName = GetConfigigurationForUser(user);


        Debug.Log("Connected as " + user);
        if (configurationName == "")
        {
            Debug.LogError("Error : No configuration found for user " + user + ". Please add an entry for this user to GetConfigigurationForUser function");
            return;
        }
        Debug.Log("Configuration used will be " + configurationName);

        try
        {
            string scriptFile = Application.dataPath + "/../../03-Server/02-Scripts/Start-AllServers/Start-AllServers.ps1";
            string arguments = "-Env " + configurationName;
            string command = "-command \"& '" + scriptFile + "'\" " + arguments;

            Debug.Log("Command is powershell.exe " + command);

            var processInfo = new ProcessStartInfo("powershell.exe", command);
            processInfo.CreateNoWindow = false;
            processInfo.UseShellExecute = false;

            var process = Process.Start(processInfo);

            process.WaitForExit();
            process.Close();

             Debug.Log("Darkrift Server successfully started");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log("Can't start Darkrift Server : " + e.Message);
        }
    }

    [MenuItem("RTS/00 - Server/Stop")]
    public static void StopServer()
    {
        try
        {
            string user = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string configurationName = GetConfigigurationForUser(user);


            Debug.Log("Connected as " + user);
            if (configurationName == "")
            {
                Debug.LogError("Error : No configuration found for user " + user + ". Please add an entry for this user to GetConfigigurationForUser function");
                return;
            }
            Debug.Log("Configuration used will be " + configurationName);

            string scriptFile = Application.dataPath + "/../../03-Server/02-Scripts/Stop-AllServers/Stop-AllServers.ps1";
            string arguments = "-Env " + configurationName;
            string command = "-command \"& '" + scriptFile + "'\" " + arguments;

            Debug.Log("Command is powershell.exe " + command);

            var processInfo = new ProcessStartInfo("powershell.exe", "-command \"& '" + scriptFile + "'\" " + arguments);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;

            var process = Process.Start(processInfo);

            process.WaitForExit();
            process.Close();

            Debug.Log("Darkrift Server successfully closed");
        }
        catch (Exception e)
        {
            Debug.Log("No process found" + e.Message);
        }
    }

    [MenuItem("RTS/00 - Server/Restart")]
    public static void RestartServer()
    {
        StopServer();
        StartServer();
    }
}
