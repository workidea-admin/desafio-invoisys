namespace DesafioInvoiSys;

internal sealed class DocumentSchemaBuilder
{
    private readonly HashSet<string> _supportedTypes = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<IDocumentRule> _rules = new();

    public DocumentSchemaBuilder SupportsType(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Supported type must be informed.", nameof(type));

        _supportedTypes.Add(type.Trim());
        return this;
    }

    public DocumentSchemaBuilder Require(Func<InputDocument, string?> selector, string missingMessage) =>
        AddRule(selector, value => !string.IsNullOrWhiteSpace(value), missingMessage);

    public DocumentSchemaBuilder AddRule<TValue>(
        Func<InputDocument, TValue> selector,
        Func<TValue, bool> predicate,
        string errorMessage,
        Func<TValue, bool>? when = null)
    {
        ArgumentNullException.ThrowIfNull(selector);
        ArgumentNullException.ThrowIfNull(predicate);
        if (string.IsNullOrWhiteSpace(errorMessage))
            throw new ArgumentException("Error message must be informed.", nameof(errorMessage));

        _rules.Add(new GenericDocumentRule<TValue>(selector, predicate, errorMessage, when));
        return this;
    }

    public DocumentValidationSchema Build() =>
        new(_supportedTypes.ToArray(), _rules.ToArray());
}
