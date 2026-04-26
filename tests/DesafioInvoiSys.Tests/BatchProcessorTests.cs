using System.Text.Json;
using DesafioInvoiSys;
using Xunit;

namespace DesafioInvoiSys.Tests;

public class BatchProcessorTests
{
    private readonly BatchProcessor _sut = new();

    [Fact]
    public void ChallengeSample_ProducesExpectedSummary()
    {
        var json = File.ReadAllText(ResolveRepoPath("exemplos/exemplo-1-basico.json"));
        var batch = JsonSerializer.Deserialize<InputBatch>(json, BatchJsonContext.InputOptions);
        Assert.NotNull(batch);

        var output = _sut.Process(batch);
        Assert.Equal("LOTE-001", output.BatchId);
        Assert.Equal(2, output.TotalDocuments);
        Assert.Equal(1, output.ValidCount);
        Assert.Equal(1, output.InvalidCount);
        Assert.Equal(2, output.Documents.Count);

        var validDoc = output.Documents[0];
        Assert.Equal("DOC-001", validDoc.Id);
        Assert.Equal("VALIDO", validDoc.Status);
        Assert.Empty(validDoc.Errors);

        var invalidDoc = output.Documents[1];
        Assert.Equal("DOC-002", invalidDoc.Id);
        Assert.Equal("INVALIDO", invalidDoc.Status);
        Assert.Equal(
            [
                "numero nao informado",
                "valor deve ser maior que zero",
                "cnpjEmitente invalido",
                "cnpjDestinatario invalido"
            ],
            invalidDoc.Errors);
    }

    [Fact]
    public void SingleValidDocument_ProducesValidOutput()
    {
        var batch = new InputBatch
        {
            BatchId = "L1",
            Documents =
            [
                new InputDocument
                {
                    Id = "A",
                    Type = "NFE",
                    Number = "1",
                    Series = "1",
                    Value = 1m,
                    IssuerCnpj = "12345678000195",
                    IssueDate = "2026-01-15"
                }
            ]
        };

        var output = _sut.Process(batch);
        Assert.Equal(1, output.ValidCount);
        Assert.Equal(0, output.InvalidCount);
        Assert.Equal("VALIDO", output.Documents[0].Status);
        Assert.Empty(output.Documents[0].Errors);
    }

    [Fact]
    public void EmptyBatch_ProducesZeroSummary()
    {
        var batch = new InputBatch { BatchId = "X", Documents = [] };
        var output = _sut.Process(batch);
        Assert.Equal(0, output.TotalDocuments);
        Assert.Equal(0, output.ValidCount);
        Assert.Equal(0, output.InvalidCount);
        Assert.Empty(output.Documents);
    }

    [Fact]
    public void NullDocuments_AreHandledAsEmptyList()
    {
        var batch = new InputBatch { BatchId = "X", Documents = null };
        var output = _sut.Process(batch);
        Assert.Equal(0, output.TotalDocuments);
    }

    [Fact]
    public void DuplicateDocuments_MarkAllInvolvedAsInvalid()
    {
        var dup = new InputDocument
        {
            Id = "D1",
            Type = "NFE",
            Number = "10",
            Series = "1",
            Value = 100m,
            IssuerCnpj = "12345678000195",
            IssueDate = "2026-04-01"
        };
        var dup2 = new InputDocument
        {
            Id = "D2",
            Type = "NFE",
            Number = "10",
            Series = "1",
            Value = 200m,
            IssuerCnpj = "12.345.678/0001-95",
            IssueDate = "2026-04-01"
        };

        var batch = new InputBatch { BatchId = "L", Documents = [dup, dup2] };
        var output = _sut.Process(batch);

        Assert.Equal(0, output.ValidCount);
        Assert.Equal(2, output.InvalidCount);
        Assert.All(output.Documents, d => Assert.Contains(ValidationMessages.DuplicateDocumentInBatch, d.Errors));
    }

    [Fact]
    public void NfseType_IsInvalidInFirstStage()
    {
        var batch = new InputBatch
        {
            Documents =
            [
                new InputDocument
                {
                    Id = "1",
                    Type = "NFSE",
                    Number = "1",
                    Series = "1",
                    Value = 1m,
                    IssuerCnpj = "12345678000195",
                    IssueDate = "2026-04-01"
                }
            ]
        };

        var output = _sut.Process(batch);
        Assert.Equal("INVALIDO", output.Documents[0].Status);
        Assert.Contains(ValidationMessages.InvalidType, output.Documents[0].Errors);
    }

    [Fact]
    public void MissingRecipientCnpj_DoesNotGenerateError()
    {
        var batch = new InputBatch
        {
            Documents =
            [
                new InputDocument
                {
                    Id = "1",
                    Type = "NFE",
                    Number = "1",
                    Series = "1",
                    Value = 1m,
                    IssuerCnpj = "12345678000195",
                    RecipientCnpj = null,
                    IssueDate = "2026-04-01"
                }
            ]
        };

        var output = _sut.Process(batch);
        Assert.DoesNotContain(ValidationMessages.InvalidRecipientCnpj, output.Documents[0].Errors);
    }

    [Fact]
    public void EmptyRecipientCnpj_GeneratesInvalidStatus()
    {
        var batch = new InputBatch
        {
            Documents =
            [
                new InputDocument
                {
                    Id = "1",
                    Type = "NFE",
                    Number = "1",
                    Series = "1",
                    Value = 1m,
                    IssuerCnpj = "12345678000195",
                    RecipientCnpj = string.Empty,
                    IssueDate = "2026-04-01"
                }
            ]
        };

        var output = _sut.Process(batch);
        Assert.Equal("INVALIDO", output.Documents[0].Status);
        Assert.Contains(ValidationMessages.InvalidRecipientCnpj, output.Documents[0].Errors);
    }

    private static string ResolveRepoPath(string relative)
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "DesafioInvoiSys.sln")))
            dir = dir.Parent;
        if (dir is null)
            throw new InvalidOperationException("Não foi possível localizar a raiz do repositório (DesafioInvoiSys.sln).");
        return Path.Combine(dir.FullName, relative);
    }
}
