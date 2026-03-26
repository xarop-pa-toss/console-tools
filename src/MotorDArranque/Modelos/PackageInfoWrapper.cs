using ConsoleTools.SpectreUI;

namespace MotorDArranque.Modelos;

/// <summary>
/// Wrapper for WGet.NET package to implement IPackageInfo interface
/// Uses dynamic to work with internal WGet.NET types
/// </summary>
public class PackageInfoWrapper : IPackageInfo
{
    private readonly dynamic _package;

    public PackageInfoWrapper(dynamic package)
    {
        _package = package;
    }

    public string Name => _package.Name;
    public string Id => _package.Id;
    public string VersionString => _package.VersionString;
    public string AvailableVersionString => _package.AvailableVersionString;
    public Version Version => _package.Version;
    public Version AvailableVersion => _package.AvailableVersion;

    // Expose the original package if needed
    public dynamic OriginalPackage => _package;
}
