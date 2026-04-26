using DesafioInvoiSys;
using Xunit;

namespace DesafioInvoiSys.Tests;

public class RecipientCnpjRuleTests
{
    [Fact]
    public void GivenNullRecipientCnpj_WhenValidating_ThenDoesNotAddError()
    {
        var rule = new RecipientCnpjRule();
        var document = new InputDocument { RecipientCnpj = null };
        var errors = new List<string>();

        rule.Validate(document, errors);

        Assert.Empty(errors);
    }

    [Fact]
    public void GivenInvalidRecipientCnpjLength_WhenValidating_ThenAddsInvalidRecipientCnpjError()
    {
        var rule = new RecipientCnpjRule();
        var document = new InputDocument { RecipientCnpj = "123" };
        var errors = new List<string>();

        rule.Validate(document, errors);

        Assert.Contains(ValidationMessages.InvalidRecipientCnpj, errors);
    }
}
