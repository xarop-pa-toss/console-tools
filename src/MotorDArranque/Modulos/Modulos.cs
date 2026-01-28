using WGetNET;

namespace ConsoleTools.Modulos;

public partial class Modulos(WinGet wget, WinGetPackageManager packMgr)
{
    private readonly WinGet _wget = wget;
    private readonly WinGetPackageManager _packMgr = packMgr;
}