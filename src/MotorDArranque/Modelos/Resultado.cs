namespace MotorDArranque.Modelos;

public readonly struct Resultado
{
    public bool IsSucesso { get; }
    public string? Info { get; }
    public string? Aviso { get; }
    public string? Erro { get; }

    private Resultado(bool sucesso, string? info, string? aviso, string? erro)
    {
        IsSucesso = sucesso;
        Info = info ?? string.Empty;
        Aviso = aviso ?? string.Empty;
        Erro = erro ?? string.Empty;
    }

    public static Resultado Ok(string info = "", string aviso = "") => 
        new(true, info, aviso, null);
    public static Resultado Falha(string info = "", string aviso = "", string erro = "") => 
        new(false, info, aviso, erro ?? "Erro desconhecido.");
}
