using System;
using System.Collections.Generic;
using System.Text;

namespace MotorDArranque
{
    internal static class AppPaths
    {
        public static readonly string UserTemp = Path.Combine(Path.GetTempPath(), "MotorDArranque");
        public static readonly string InstallRoot = String.Empty;
    }
}
