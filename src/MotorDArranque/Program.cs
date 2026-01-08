using console_tools;
using console_tools.Modulos;
using console_tools.Utils;
using MotorDArranque;
using Spectre.Console;

// STARTUP
var Modulos = new Modulos();

Directory.CreateDirectory(AppPaths.UserTemp);

Utils.WriteGradient(Assets.InfoLogo, Color.Purple, Color.Aqua);

var panelTitulo = new Panel(
    new Markup(
        "[Invert Aqua]   MOTOR D'ARRANQUE   [/]\n\n" +
        "Ferramenta de instalação de software com Winget").Centered()
    ).BorderColor(Color.Purple)
    .HeaderAlignment(Justify.Center)
    .RoundedBorder();
AnsiConsole.Write(Align.Center(panelTitulo));

await Modulos.UpdateMotor();
    
//    AnsiConsole.Prompt(
//    new ConfirmationPrompt("Quer analisar os programas instalados e procurar actualizações?")
//    {
//        DefaultValue = false,
//        ChoicesStyle = Styles.Base.Foreground,
//        DefaultValueStyle = Styles.Base.Background
        
//    }
//    .Yes('s')
//    .No('n')
//    .InvalidChoiceMessage("Ou (s)im ou (n)ão jovem...")
//);

var mainMenu = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("[Bold Underline Turquoise2]Operações[/]")
        .WrapAround()
        .AddChoices(
            [
                "Lista de programas instalados",
                "Instalar",
                "Desinstalar"
            ])
        .HighlightStyle(new Style(Styles.Base.Background, decoration: Decoration.Bold)));

// RESULTADOS MAIN MENU
switch (mainMenu)
{
    case "Lista de programas instalados":
        await Modulos.TabelaInstalados();
        break;
    //case "Instalar":
    //    await Modulos.EcraInstalar;
    //    break;
    //case "Desinstalar":
    //    await Modulos.EcraDesinstalar;
    //    break;
    default:
        AnsiConsole.WriteLine("nothing");
        break;
}


// var name = AnsiConsole.Ask<string>("What's your [green]name[/]?");

// // Choose size
// var size = AnsiConsole.Prompt(
//     new SelectionPrompt<string>()
//         .Title("What [green]size pizza[/] would you like?")
//         .AddChoices("Small", "Medium", "Large", "Extra Large"));

// // Select toppings
// var toppings = AnsiConsole.Prompt(
//     new MultiSelectionPrompt<string>()
//         .Title("What [green]toppings[/] would you like?")
//         .NotRequired()
//         .InstructionsText("[grey](Press [blue]<space>[/] to toggle, [green]<enter>[/] to confirm)[/]")
//         .AddChoices("Pepperoni", "Mushrooms", "Sausage",
//             "Onions", "Green Peppers", "Black Olives",
//             "Extra Cheese", "Bacon", "Pineapple"));

// // Show order summary
// AnsiConsole.WriteLine();
// var panels = new Panel(
//         new Rows(
//             new Markup($"[bold]Customer:[/] {name}"),
//             new Markup($"[bold]Size:[/]     {size}"),
//             new Markup($"[bold]Toppings:[/] {(toppings.Count > 0 ? string.Join(", ", toppings) : "Plain cheese")}")))
//     .Header("[yellow]Order Summary[/]")
//     .Border(BoxBorder.Rounded);
// AnsiConsole.Write(panels);
// AnsiConsole.WriteLine();

// // Confirm order
// if (AnsiConsole.Confirm("Place this order?"))
// {
//     AnsiConsole.MarkupLine($"[green]Order placed! Thanks, {name}![/]");
// }
// else
// {
//     AnsiConsole.MarkupLine("[yellow]Order cancelled.[/]");
// }