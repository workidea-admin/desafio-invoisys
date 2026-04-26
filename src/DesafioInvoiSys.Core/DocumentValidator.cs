using System.Globalization;

namespace DesafioInvoiSys;

public static class DocumentValidator
{
    public static List<string> ValidateFields(InputDocument document)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(document.Id))
            errors.Add(ValidationMessages.MissingId);

        if (string.IsNullOrWhiteSpace(document.Type))
        {
            errors.Add(ValidationMessages.MissingType);
        }
        else if (!IsSupportedType(document.Type.Trim()))
        {
            errors.Add(ValidationMessages.InvalidType);
        }

        if (string.IsNullOrWhiteSpace(document.Number))
            errors.Add(ValidationMessages.MissingNumber);

        if (string.IsNullOrWhiteSpace(document.Series))
            errors.Add(ValidationMessages.MissingSeries);

        if (document.Value is null)
            errors.Add(ValidationMessages.MissingValue);
        else if (document.Value <= 0m)
            errors.Add(ValidationMessages.ValueMustBeGreaterThanZero);

        if (string.IsNullOrWhiteSpace(document.IssuerCnpj))
            errors.Add(ValidationMessages.MissingIssuerCnpj);
        else if (CnpjHelper.DigitsOnly(document.IssuerCnpj).Length != 14)
            errors.Add(ValidationMessages.InvalidIssuerCnpj);

        if (string.IsNullOrWhiteSpace(document.IssueDate))
            errors.Add(ValidationMessages.MissingIssueDate);
        else if (!TryParseDate(document.IssueDate, out _))
            errors.Add(ValidationMessages.InvalidIssueDate);

        if (CnpjHelper.IsRecipientProvided(document.RecipientCnpj))
        {
            if (CnpjHelper.DigitsOnly(document.RecipientCnpj).Length != 14)
                errors.Add(ValidationMessages.InvalidRecipientCnpj);
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

    private static bool IsSupportedType(string trimmedType) =>
        trimmedType.Equals("NFE", StringComparison.OrdinalIgnoreCase);

    private static bool TryParseDate(string text, out DateOnly date)
    {
        if (DateOnly.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            return true;

        if (DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var parsedDateTime))
        {
            date = DateOnly.FromDateTime(parsedDateTime);
            return true;
        }

        date = default;
        return false;
    }
}
