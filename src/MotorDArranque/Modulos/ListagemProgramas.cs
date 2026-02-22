using MotorDArranque.Modelos;
using ConsoleTools.ConsoleUI;


namespace ConsoleTools.Modulos;

public partial class Modulos
{
    public async Task<Resultado> ListagemProgramas()
    {
        var listaProgramas = await _packMgr.GetInstalledPackagesAsync();
        var listaProgWinget = listaProgramas
            .FindAll(p => p.SourceName == "winget")
            .OrderByDescending(p => p.Version != p.AvailableVersion)
            .ThenBy(p => p.Name)
            .Select(p => new PackageInfoWrapper(p))
            .ToList();

        // Use the reusable PackageSelector component
        var progList = PackageSelector.SelectPackagesAsStrings(listaProgWinget);

        _menuHandler.Handle(await ProgOpsMenu(progList));

        return Resultado.Ok();
    }
}