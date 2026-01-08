using System.Reflection;
using Spectre.Console;

namespace console_tools.Modulos;

public partial class Modulos
{
	public async Task UpdateMotor()
	{
		Version version = Assembly.GetEntryAssembly().GetName().Version ?? new Version();
    }
}
