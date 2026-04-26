namespace DesafioInvoiSys;

public static class ValidationMessages
{
    public const string MissingId = "id nao informado";
    public const string MissingType = "tipo nao informado";
    public const string InvalidType = "tipo invalido";
    public const string MissingNumber = "numero nao informado";
    public const string MissingSeries = "serie nao informada";
    public const string MissingValue = "valor nao informado";
    public const string ValueMustBeGreaterThanZero = "valor deve ser maior que zero";
    public const string MissingIssuerCnpj = "cnpjEmitente nao informado";
    public const string InvalidIssuerCnpj = "cnpjEmitente invalido";
    public const string InvalidRecipientCnpj = "cnpjDestinatario invalido";
    public const string MissingIssueDate = "dataEmissao nao informada";
    public const string InvalidIssueDate = "dataEmissao invalida";
    public const string DuplicateDocumentInBatch = "documento duplicado no lote";
}
