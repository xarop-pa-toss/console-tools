using System;
using System.Collections.Generic;
using System.Text;

namespace MotorDArranque.Modelos;

public readonly struct Resultado
{
    public bool IsSucesso { get; }
    public string? Erro { get; }

    private Resultado(bool sucesso, string? erro)
    {
        IsSucesso = sucesso;
        Erro = erro;
    }

    public static Resultado Ok() => new(true, String.Empty);
    public static Resultado Falha(string erro) => new(false, erro ?? "Sem erro explícito.");
}
