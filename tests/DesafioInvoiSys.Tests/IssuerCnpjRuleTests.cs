using DesafioInvoiSys;
using Xunit;

namespace DesafioInvoiSys.Tests;

public class IssuerCnpjRuleTests
{
    [Fact]
    public void GivenEmptyIssuerCnpj_WhenValidating_ThenAddsMissingIssuerCnpjError()
    {
        var rule = new IssuerCnpjRule();
        var document = new InputDocument { IssuerCnpj = "" };
        var errors = new List<string>();

        rule.Validate(document, errors);

        Assert.Contains(ValidationMessages.MissingIssuerCnpj, errors);
    }

    [Fact]
    public void GivenInvalidIssuerCnpjLength_WhenValidating_ThenAddsInvalidIssuerCnpjError()
    {
        var rule = new IssuerCnpjRule();
        var document = new InputDocument { IssuerCnpj = "123" };
        var errors = new List<string>();

        rule.Validate(document, errors);

        Assert.Contains(ValidationMessages.InvalidIssuerCnpj, errors);
    }
}
