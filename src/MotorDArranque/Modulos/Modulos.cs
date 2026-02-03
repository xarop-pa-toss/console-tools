using Microsoft.Extensions.Logging;
using MotorDArranque.Modulos;
using WGetNET;

namespace ConsoleTools.Modulos;

public partial class Modulos(WinGet wget, WinGetPackageManager packMgr, MenuHandler menuHandler, ILogger<Modulos> logger)
{
    private readonly WinGet _wget = wget;
    private readonly WinGetPackageManager _packMgr = packMgr;
    private readonly MenuHandler _mh = menuHandler;
    private readonly ILogger<Modulos> _logger = logger;
}