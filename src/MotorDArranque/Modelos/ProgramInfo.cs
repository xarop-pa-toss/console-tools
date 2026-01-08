using System;
using System.Collections.Generic;
using System.Text;

namespace MotorDArranque.Modelos
{
    public record ProgramInfo(
        string Id,
        string InstalledVersion,
        string AvailableVersion,
        string Source
    );
}
