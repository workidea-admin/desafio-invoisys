using System.Globalization;

namespace DesafioInvoiSys;

public static class DocumentValidator
{
    private static readonly ValidationEngine Engine = new();
    private static readonly DocumentValidationSchema Schema = BuildSchema();

    public static List<string> ValidateFields(InputDocument document) => Engine.Validate(document, Schema);

    private static DocumentValidationSchema BuildSchema() =>
        new DocumentSchemaBuilder()
            .SupportsType("NFE")
            .Require(d => d.Id, ValidationMessages.MissingId)
            .Require(d => d.Number, ValidationMessages.MissingNumber)
            .Require(d => d.Series, ValidationMessages.MissingSeries)
            .RequirePositiveValue(d => d.Value, ValidationMessages.MissingValue, ValidationMessages.ValueMustBeGreaterThanZero)
            .RequireValidCnpj(d => d.IssuerCnpj, ValidationMessages.MissingIssuerCnpj, ValidationMessages.InvalidIssuerCnpj)
            .RequireDate(d => d.IssueDate, ValidationMessages.MissingIssueDate, ValidationMessages.InvalidIssueDate, TryParseDate)
            .ValidateOptionalCnpj(d => d.RecipientCnpj, ValidationMessages.InvalidRecipientCnpj, CnpjHelper.IsRecipientProvided)
            .Build();

    private static bool TryParseDate(string text)
    {
        if (DateOnly.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            return true;

        return DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out _);
    }

    public static string BuildDuplicateKey(InputDocument document)
    {
        var type = (document.Type ?? string.Empty).Trim().ToUpperInvariant();
        var issuerDigits = CnpjHelper.DigitsOnly(document.IssuerCnpj);
        var series = (document.Series ?? string.Empty).Trim();
        var number = (document.Number ?? string.Empty).Trim();
        return $"{type}|{issuerDigits}|{series}|{number}";
    }
}
