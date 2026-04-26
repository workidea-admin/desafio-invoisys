namespace DesafioInvoiSys;

public sealed class BatchProcessor
{
    public OutputBatch Process(InputBatch input)
    {
        var documents = input.Documents ?? new List<InputDocument>();
        var preparation = PrepareValidationAndDuplicateData(documents);

        var output = BuildInitialOutput(input, documents.Count);
        PopulateOutputDocuments(output, documents, preparation);
        return output;
    }

    private static OutputBatch BuildInitialOutput(InputBatch input, int totalDocuments) =>
        new()
        {
            BatchId = input.BatchId ?? string.Empty,
            TotalDocuments = totalDocuments
        };

    private static ValidationPreparation PrepareValidationAndDuplicateData(IReadOnlyList<InputDocument> documents)
    {
        var errorsByIndex = new List<List<string>>(documents.Count);
        var duplicateKeyByIndex = new List<string>(documents.Count);
        var duplicateKeyOccurrences = new Dictionary<string, int>(StringComparer.Ordinal);

        for (var i = 0; i < documents.Count; i++)
        {
            var document = documents[i];
            errorsByIndex.Add(DocumentValidator.ValidateFields(document));
            var duplicateKey = DocumentValidator.BuildDuplicateKey(document);
            duplicateKeyByIndex.Add(duplicateKey);
            duplicateKeyOccurrences[duplicateKey] = duplicateKeyOccurrences.TryGetValue(duplicateKey, out var count)
                ? count + 1
                : 1;
        }

        return new ValidationPreparation(errorsByIndex, duplicateKeyByIndex, duplicateKeyOccurrences);
    }

    private static void PopulateOutputDocuments(
        OutputBatch output,
        IReadOnlyList<InputDocument> documents,
        ValidationPreparation preparation)
    {
        var validCount = 0;
        var invalidCount = 0;

        for (var i = 0; i < documents.Count; i++)
        {
            var errors = new List<string>(preparation.ErrorsByIndex[i]);
            var duplicateKey = preparation.DuplicateKeyByIndex[i];
            if (preparation.DuplicateKeyOccurrences[duplicateKey] > 1)
                errors.Add(ValidationMessages.DuplicateDocumentInBatch);

            var document = documents[i];
            var status = errors.Count == 0 ? "VALIDO" : "INVALIDO";
            output.Documents.Add(new OutputDocument
            {
                Id = document.Id?.Trim() ?? string.Empty,
                Status = status,
                Errors = errors
            });

            if (status == "VALIDO")
                validCount++;
            else
                invalidCount++;
        }

        output.ValidCount = validCount;
        output.InvalidCount = invalidCount;
    }

    private readonly record struct ValidationPreparation(
        List<List<string>> ErrorsByIndex,
        List<string> DuplicateKeyByIndex,
        Dictionary<string, int> DuplicateKeyOccurrences);
}
