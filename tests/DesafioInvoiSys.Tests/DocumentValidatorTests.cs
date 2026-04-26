using DesafioInvoiSys;
using Xunit;

namespace DesafioInvoiSys.Tests;

public class DocumentValidatorTests
{
    private static InputDocument BuildBaselineDocument() => new()
    {
        Id = "1",
        Type = "NFE",
        Number = "1",
        Series = "1",
        Value = 1m,
        IssuerCnpj = "12345678000195",
        IssueDate = "2026-04-01"
    };

    [Fact]
    public void BaselineDocument_HasNoValidationErrors()
    {
        var errors = DocumentValidator.ValidateFields(BuildBaselineDocument());
        Assert.Empty(errors);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void EmptyNumber_ReturnsMissingNumberError(string inputNumber)
    {
        var document = BuildBaselineDocument();
        document.Number = inputNumber;
        var errors = DocumentValidator.ValidateFields(document);
        Assert.Contains(ValidationMessages.MissingNumber, errors);
    }

    [Fact]
    public void ZeroValue_ReturnsValueMustBeGreaterThanZeroError()
    {
        var document = BuildBaselineDocument();
        document.Value = 0m;
        var errors = DocumentValidator.ValidateFields(document);
        Assert.Contains(ValidationMessages.ValueMustBeGreaterThanZero, errors);
    }

    [Fact]
    public void NullValue_ReturnsMissingValueError()
    {
        var document = BuildBaselineDocument();
        document.Value = null;
        var errors = DocumentValidator.ValidateFields(document);
        Assert.Contains(ValidationMessages.MissingValue, errors);
    }

    [Fact]
    public void UnsupportedType_ReturnsInvalidTypeError()
    {
        var document = BuildBaselineDocument();
        document.Type = "CTE";
        var errors = DocumentValidator.ValidateFields(document);
        Assert.Contains(ValidationMessages.InvalidType, errors);
    }

    [Fact]
    public void InvalidIssueDate_ReturnsInvalidIssueDateError()
    {
        var document = BuildBaselineDocument();
        document.IssueDate = "not-a-date";
        var errors = DocumentValidator.ValidateFields(document);
        Assert.Contains(ValidationMessages.InvalidIssueDate, errors);
    }

    [Fact]
    public void ShortIssuerCnpj_ReturnsInvalidIssuerCnpjError()
    {
        var document = BuildBaselineDocument();
        document.IssuerCnpj = "123";
        var errors = DocumentValidator.ValidateFields(document);
        Assert.Contains(ValidationMessages.InvalidIssuerCnpj, errors);
    }
}
