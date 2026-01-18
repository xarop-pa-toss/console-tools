using System.Diagnostics;
using System.Management;
using Spectre.Console;

namespace ConsoleTools.Utils;

// MADE WITH CLAUDE
public class NOTUSED_ShellDetector
{
    public static string GetParentProcessName()
    {
        try
        {
            var currentProcess = Process.GetCurrentProcess();
            var query = $"SELECT ParentProcessId FROM Win32_Process WHERE ProcessId = {currentProcess.Id}";
            
            using (var searcher = new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    var parentId = Convert.ToInt32(obj["ParentProcessId"]);
                    var parentProcess = Process.GetProcessById(parentId);

                    // Modern Windows Terminal (wt.exe) will be the parent of anything inside.
                    // Making it impossible to check which shell is running by parent process.
                    if (parentProcess.ProcessName.ToLower() == "wt")
                    {
                        // PowerShell 5 + 7
                        if (Environment.GetEnvironmentVariable("PSModulePath") != null
                            || Environment.GetEnvironmentVariable("POWERSHELL_DISTRIBUTION_CHANNEL") != null)
                        {
                            return "powershell";
                        }
                    }
                    
                    return parentProcess.ProcessName.ToLower();
                }
            }
        }
        catch
        {
            return "unknown";
        }
        
        return "unknown";
    }
    
    public static bool IsRunningInPowerShell()
    {
        var parentName = GetParentProcessName();
        AnsiConsole.MarkupLineInterpolated($"[green]{parentName}[/]");
        return parentName.Contains("powershell") || parentName.Contains("pwsh");
    }
    
    public static bool IsRunningInCmd()
    {
        var parentName = GetParentProcessName();
        return parentName.Contains("cmd");
    }
}