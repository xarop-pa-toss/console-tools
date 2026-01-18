using System.Diagnostics;
using System.Management;

namespace ConsoleTools.Utils;

// MADE WITH CLAUDE
public class ShellDetector
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
        return parentName.Contains("powershell") || parentName.Contains("pwsh");
    }
    
    public static bool IsRunningInCmd()
    {
        var parentName = GetParentProcessName();
        return parentName.Contains("cmd");
    }
}