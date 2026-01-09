using System;
using System.Collections.Generic;
using System.Text;

namespace MotorDArranque.Modelos
{
    public record ProgramInfo(
        string Name,
        string Id,
        string InstalledVersion,
        string AvailableVersion,
        string Source
    );
}
