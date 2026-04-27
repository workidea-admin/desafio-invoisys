namespace DesafioInvoiSys;

internal interface IDocumentRule
{
    void Validate(InputDocument document, List<string> errors);
}
