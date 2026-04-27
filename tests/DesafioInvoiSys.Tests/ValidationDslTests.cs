using DesafioInvoiSys;
using Xunit;

namespace DesafioInvoiSys.Tests;

public class ValidationDslTests
{
    [Fact]
    public void SupportsType_WithBlankValue_ThrowsArgumentException()
    {
        var builder = new DocumentSchemaBuilder();
        Assert.Throws<ArgumentException>(() => builder.SupportsType(" "));
    }

    [Fact]
    public void AddRule_WithBlankMessage_ThrowsArgumentException()
    {
        var builder = new DocumentSchemaBuilder();
        Assert.Throws<ArgumentException>(() => builder.AddRule(d => d.Id, _ => true, " "));
    }

    [Fact]
    public void Rules_AreEvaluatedInDeclarationOrder()
    {
        var schema = new DocumentSchemaBuilder()
            .AddRule(d => d.Id, v => !string.IsNullOrWhiteSpace(v), "erro-1")
            .AddRule(d => d.Number, v => !string.IsNullOrWhiteSpace(v), "erro-2")
            .Build();

        var engine = new ValidationEngine();
        var document = new InputDocument { Type = "NFE", Id = "", Number = "" };

        var errors = engine.Validate(document, schema);

        Assert.Equal(["erro-1", "erro-2"], errors);
    }

    [Fact]
    public void ConditionalRule_DoesNotRun_WhenPredicateIsFalse()
    {
        var schema = new DocumentSchemaBuilder()
            .AddRule(
                d => d.RecipientCnpj,
                cnpj => CnpjHelper.DigitsOnly(cnpj).Length == 14,
                ValidationMessages.InvalidRecipientCnpj,
                CnpjHelper.IsRecipientProvided)
            .Build();

        var engine = new ValidationEngine();
        var document = new InputDocument { Type = "NFE", RecipientCnpj = null };

        var errors = engine.Validate(document, schema);

        Assert.Empty(errors);
    }

    [Fact]
    public void ParametrizedRule_UsesProvidedPredicateAndMessage()
    {
        var schema = new DocumentSchemaBuilder()
            .AddRule(d => d.Value, v => v is > 10m, "valorMinimo10")
            .Build();

        var engine = new ValidationEngine();
        var document = new InputDocument { Type = "NFE", Value = 5m };

        var errors = engine.Validate(document, schema);

        Assert.Equal(["valorMinimo10"], errors);
    }
}
