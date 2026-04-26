using System.Text.Json.Serialization;

namespace DesafioInvoiSys;

public sealed class InputBatch
{
    [JsonPropertyName("loteId")]
    public string? BatchId { get; set; }

    [JsonPropertyName("documentos")]
    public List<InputDocument>? Documents { get; set; }
}

public sealed class InputDocument
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("tipo")]
    public string? Type { get; set; }

    [JsonPropertyName("numero")]
    public string? Number { get; set; }

    [JsonPropertyName("serie")]
    public string? Series { get; set; }

    [JsonPropertyName("valor")]
    public decimal? Value { get; set; }

    [JsonPropertyName("cnpjEmitente")]
    public string? IssuerCnpj { get; set; }

    [JsonPropertyName("cnpjDestinatario")]
    public string? RecipientCnpj { get; set; }

    [JsonPropertyName("dataEmissao")]
    public string? IssueDate { get; set; }
}

public sealed class OutputBatch
{
    [JsonPropertyName("loteId")]
    public string BatchId { get; set; } = string.Empty;

    [JsonPropertyName("totalDocumentos")]
    public int TotalDocuments { get; set; }

    [JsonPropertyName("validos")]
    public int ValidCount { get; set; }

    [JsonPropertyName("invalidos")]
    public int InvalidCount { get; set; }

    [JsonPropertyName("documentos")]
    public List<OutputDocument> Documents { get; set; } = new();
}

public sealed class OutputDocument
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("erros")]
    public List<string> Errors { get; set; } = new();
}
