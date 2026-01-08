using System.Text.Json;
using System.Text.Json.Serialization;

namespace MotorDArranque.Modelos
{
    public sealed class ExportJsonDTO
    {
        public List<Source> Sources { get; set; } = [];
    }

    public sealed class Source
    {
        public List<Package> Packages { get; set; } = [];
        public SourceDetails SourceDetails { get; set; } = new();
    }

    public sealed class Package
    {
        public string PackageIdentifier { get; set; } = "";
        public string Version { get; set; } = "";
    }

    public sealed class SourceDetails
    {
        public string Name { get; set; } = "";
    }
}
