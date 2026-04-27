namespace DesafioInvoiSys;

internal sealed class ValidationEngine
{
    public List<string> Validate(InputDocument document, DocumentValidationSchema schema)
    {
        var errors = new List<string>();

        ValidateType(document, schema, errors);

        foreach (var rule in schema.Rules)
        {
            rule.Validate(document, errors);
        }

        return errors;
    }

    private static void ValidateType(
        InputDocument document,
        DocumentValidationSchema schema,
        List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(document.Type))
        {
            errors.Add(ValidationMessages.MissingType);
            return;
        }

        var trimmed = document.Type.Trim();
        if (schema.SupportedTypes.Count > 0 && !schema.SupportedTypes.Contains(trimmed, StringComparer.OrdinalIgnoreCase))
            errors.Add(ValidationMessages.InvalidType);
    }
}
