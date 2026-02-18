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
        Info = info ?? String.Empty;
        Aviso = aviso ?? String.Empty;
        Erro = erro ?? String.Empty;
    }

    public static Resultado Ok(string? info = "", string? aviso = "") => 
        new(true, info, aviso, null);
    public static Resultado Falha(string? info = "", string? aviso = "", string? erro = "") => 
        new(false, info, aviso, erro ?? "Erro desconhecido.");
}
