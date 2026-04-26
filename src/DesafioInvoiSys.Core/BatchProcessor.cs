namespace DesafioInvoiSys;

public sealed class BatchProcessor
{
    public OutputBatch Process(InputBatch input)
    {
        var documents = input.Documents ?? new List<InputDocument>();
        var errorsByIndex = new List<List<string>>(documents.Count);

        for (var i = 0; i < documents.Count; i++)
            errorsByIndex.Add(DocumentValidator.ValidateFields(documents[i]));

        ApplyDuplicateRuleInBatch(documents, errorsByIndex);

        var output = new OutputBatch
        {
            BatchId = input.BatchId ?? string.Empty,
            TotalDocuments = documents.Count
        };

        for (var i = 0; i < documents.Count; i++)
        {
            var errors = errorsByIndex[i];
            var doc = documents[i];
            output.Documents.Add(new OutputDocument
            {
                Id = doc.Id?.Trim() ?? string.Empty,
                Status = errors.Count == 0 ? "VALIDO" : "INVALIDO",
                Errors = errors
            });
        }

        output.ValidCount = output.Documents.Count(d => d.Status == "VALIDO");
        output.InvalidCount = output.Documents.Count(d => d.Status == "INVALIDO");

        return output;
    }

    private static void ApplyDuplicateRuleInBatch(
        IReadOnlyList<InputDocument> documents,
        List<List<string>> errorsByIndex)
    {
        if (documents.Count == 0)
            return;

        var groups = documents
            .Select((doc, index) => (doc, index))
            .GroupBy(x => DocumentValidator.BuildDuplicateKey(x.doc));

        foreach (var group in groups)
        {
            var items = group.ToList();
            if (items.Count <= 1)
                continue;

            foreach (var (_, index) in items)
                errorsByIndex[index].Add(ValidationMessages.DuplicateDocumentInBatch);
        }
    }
}
