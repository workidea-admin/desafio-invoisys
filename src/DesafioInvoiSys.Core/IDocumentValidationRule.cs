namespace DesafioInvoiSys;

public interface IDocumentValidationRule
{
    void Validate(InputDocument document, List<string> errors);
}
