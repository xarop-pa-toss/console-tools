namespace ConsoleTools.Core;

public class PackageInfo
{
    public string Name { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string InstalledVersion { get; set; } = string.Empty;
    public string AvailableVersion { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;

    public PackageInfo() { }

    public PackageInfo(string name, string id, string installedVersion, string availableVersion, string source)
    {
        Name = name;
        Id = id;
        InstalledVersion = installedVersion;
        AvailableVersion = availableVersion;
        Source = source;
    }
}
