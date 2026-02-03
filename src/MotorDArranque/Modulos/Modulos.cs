using Microsoft.Extensions.Logging;
using WGetNET;

namespace ConsoleTools.Modulos;

public partial class Modulos(WinGet wget, WinGetPackageManager packMgr, ILogger<Modulos> logger)
{
    private readonly WinGet _wget = wget;
    private readonly WinGetPackageManager _packMgr = packMgr;
    private readonly ILogger<Modulos> _logger = logger;
}