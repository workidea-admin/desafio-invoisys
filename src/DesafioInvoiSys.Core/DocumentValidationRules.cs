using System.Globalization;

namespace DesafioInvoiSys;

internal sealed class RequiredIdRule : IDocumentValidationRule
{
    public void Validate(InputDocument document, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(document.Id))
            errors.Add(ValidationMessages.MissingId);
    }
}

internal sealed class TypeRule : IDocumentValidationRule
{
    public void Validate(InputDocument document, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(document.Type))
        {
            errors.Add(ValidationMessages.MissingType);
            return;
        }

        if (!document.Type.Trim().Equals("NFE", StringComparison.OrdinalIgnoreCase))
            errors.Add(ValidationMessages.InvalidType);
    }
}

internal sealed class NumberRule : IDocumentValidationRule
{
    public void Validate(InputDocument document, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(document.Number))
            errors.Add(ValidationMessages.MissingNumber);
    }
}

internal sealed class SeriesRule : IDocumentValidationRule
{
    public void Validate(InputDocument document, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(document.Series))
            errors.Add(ValidationMessages.MissingSeries);
    }
}

internal sealed class ValueRule : IDocumentValidationRule
{
    public void Validate(InputDocument document, List<string> errors)
    {
        if (document.Value is null)
        {
            errors.Add(ValidationMessages.MissingValue);
            return;
        }

        if (document.Value <= 0m)
            errors.Add(ValidationMessages.ValueMustBeGreaterThanZero);
    }
}

internal sealed class IssuerCnpjRule : IDocumentValidationRule
{
    public void Validate(InputDocument document, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(document.IssuerCnpj))
        {
            errors.Add(ValidationMessages.MissingIssuerCnpj);
            return;
        }

        if (CnpjHelper.DigitsOnly(document.IssuerCnpj).Length != 14)
            errors.Add(ValidationMessages.InvalidIssuerCnpj);
    }
}

internal sealed class IssueDateRule : IDocumentValidationRule
{
    public void Validate(InputDocument document, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(document.IssueDate))
        {
            errors.Add(ValidationMessages.MissingIssueDate);
            return;
        }

        if (!TryParseDate(document.IssueDate))
            errors.Add(ValidationMessages.InvalidIssueDate);
    }

    private static bool TryParseDate(string text)
    {
        if (DateOnly.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            return true;

        return DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out _);
    }
}

internal sealed class RecipientCnpjRule : IDocumentValidationRule
{
    public void Validate(InputDocument document, List<string> errors)
    {
        if (!CnpjHelper.IsRecipientProvided(document.RecipientCnpj))
            return;

        if (CnpjHelper.DigitsOnly(document.RecipientCnpj).Length != 14)
            errors.Add(ValidationMessages.InvalidRecipientCnpj);
    }
}
