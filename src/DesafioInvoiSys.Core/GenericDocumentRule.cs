namespace DesafioInvoiSys;

internal sealed class GenericDocumentRule<TValue> : IDocumentRule
{
    private readonly Func<InputDocument, TValue> _selector;
    private readonly Func<TValue, bool> _predicate;
    private readonly string _errorMessage;
    private readonly Func<TValue, bool>? _when;

    public GenericDocumentRule(
        Func<InputDocument, TValue> selector,
        Func<TValue, bool> predicate,
        string errorMessage,
        Func<TValue, bool>? when)
    {
        ArgumentNullException.ThrowIfNull(selector);
        ArgumentNullException.ThrowIfNull(predicate);
        if (string.IsNullOrWhiteSpace(errorMessage))
            throw new ArgumentException("Error message must be informed.", nameof(errorMessage));

        _selector = selector;
        _predicate = predicate;
        _errorMessage = errorMessage;
        _when = when;
    }

    public void Validate(InputDocument document, List<string> errors)
    {
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(errors);

        var value = _selector(document);
        if (_when is not null && !_when(value))
            return;

        if (!_predicate(value))
            errors.Add(_errorMessage);
    }
}
