using System.Text;

namespace DesafioInvoiSys;

public static class CnpjHelper
{
    public static string DigitsOnly(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        var builder = new StringBuilder(value.Length);
        foreach (var ch in value)
        {
            if (char.IsDigit(ch))
                builder.Append(ch);
        }

        return builder.ToString();
    }

    public static bool IsRecipientProvided(string? recipientCnpj) => recipientCnpj is not null;
}
