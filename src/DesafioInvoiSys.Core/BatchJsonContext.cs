using System.Text.Json;
using System.Text.Json.Serialization;

namespace DesafioInvoiSys;

public static class BatchJsonContext
{
    public static JsonSerializerOptions InputOptions { get; } = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };

    public static JsonSerializerOptions OutputOptions { get; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        WriteIndented = true
    };
}
