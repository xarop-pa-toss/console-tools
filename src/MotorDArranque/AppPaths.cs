using System;
using System.Collections.Generic;
using System.Text;

namespace MotorDArranque
{
    internal static class AppPaths
    {
        public readonly static string AppDirInUserTemp = Path.Combine(Path.GetTempPath(), "MotorDArranque");
        public readonly static string InstallRoot = String.Empty;
    }
}
