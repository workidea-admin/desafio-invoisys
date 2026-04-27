namespace DesafioInvoiSys;

internal sealed class DocumentValidationSchema
{
    public DocumentValidationSchema(
        IReadOnlyCollection<string> supportedTypes,
        IReadOnlyList<IDocumentRule> rules)
    {
        SupportedTypes = supportedTypes;
        Rules = rules;
    }

    public IReadOnlyCollection<string> SupportedTypes { get; }
    public IReadOnlyList<IDocumentRule> Rules { get; }
}
