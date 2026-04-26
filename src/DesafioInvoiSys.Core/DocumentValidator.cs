namespace DesafioInvoiSys;

public static class DocumentValidator
{
    private static readonly IDocumentValidationRule[] Rules =
    [
        new RequiredIdRule(),
        new TypeRule(),
        new NumberRule(),
        new SeriesRule(),
        new ValueRule(),
        new IssuerCnpjRule(),
        new IssueDateRule(),
        new RecipientCnpjRule()
    ];

    public static List<string> ValidateFields(InputDocument document)
    {
        var errors = new List<string>();

        foreach (var rule in Rules)
        {
            rule.Validate(document, errors);
        }

        return errors;
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
