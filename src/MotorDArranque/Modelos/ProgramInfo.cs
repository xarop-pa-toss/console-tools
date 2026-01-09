using System;
using System.Collections.Generic;
using System.Text;

namespace MotorDArranque.Modelos
{
    public class ProgramInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string InstalledVersion { get; set; } = string.Empty;
        public string AvailableVersion { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;

        public ProgramInfo() { }

        public ProgramInfo(string name, string id, string installedVersion, string availableVersion, string source)
        {
            Name = name;
            Id = id;
            InstalledVersion = installedVersion;
            AvailableVersion = availableVersion;
            Source = source;
        }
    }

}
