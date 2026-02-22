namespace ConsoleTools.Core;

public readonly struct Result
{
    public bool IsSuccess { get; }
    public string? Info { get; }
    public string? Warning { get; }
    public string? Error { get; }

    private Result(bool success, string? info, string? warning, string? error)
    {
        IsSuccess = success;
        Info = info ?? string.Empty;
        Warning = warning ?? string.Empty;
        Error = error ?? string.Empty;
    }

    public static Result Ok(string? info = "", string? warning = "") =>
        new(true, info, warning, null);

    public static Result Failure(string? info = "", string? warning = "", string? error = "") =>
        new(false, info, warning, error ?? "Unknown error.");
}
