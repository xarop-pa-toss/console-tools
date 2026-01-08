using MotorDArranque.Modelos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MotorDArranque.WingetOps
{ 
    internal class GetProgramasInstalados
    {
        public List<ProgramInfo> Programs { get; private set; } = new();

        private GetProgramasInstalados(string wingetExportJsonPath) 
        {
            return ListaProgramas(wingetExportJsonPath);
        }

        public static async Task<List<ProgramInfo>> ListaProgramas(string wingetExportJsonPath)
        {
            var json = File.ReadAllText(wingetExportJsonPath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var export = JsonSerializer.Deserialize<ExportJsonDTO>(json, options);

            List<ProgramInfo> listaProgramas =
                export!.Sources
                    .SelectMany(source =>
                        source.Packages.Select(pkg =>
                            new ProgramInfo(
                                Id: pkg.PackageIdentifier,
                                InstalledVersion: pkg.Version,
                                AvailableVersion: "", //Não presente neste JSON
                                Source: source.SourceDetails.Name
                            )))
                    .OrderByDescending(x => x.Source)
                    .ThenBy(x => x.Id)
                    .ToList();



            return listaProgramas;
        }
    }
}
