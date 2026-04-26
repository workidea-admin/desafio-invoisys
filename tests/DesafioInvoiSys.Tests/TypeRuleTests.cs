using DesafioInvoiSys;
using Xunit;

namespace DesafioInvoiSys.Tests;

public class TypeRuleTests
{
    [Fact]
    public void GivenEmptyType_WhenValidating_ThenAddsMissingTypeError()
    {
        var rule = new TypeRule();
        var document = new InputDocument { Type = " " };
        var errors = new List<string>();

        rule.Validate(document, errors);

        Assert.Contains(ValidationMessages.MissingType, errors);
    }

    [Fact]
    public void GivenUnsupportedType_WhenValidating_ThenAddsInvalidTypeError()
    {
        var rule = new TypeRule();
        var document = new InputDocument { Type = "CTE" };
        var errors = new List<string>();

        rule.Validate(document, errors);

        Assert.Contains(ValidationMessages.InvalidType, errors);
    }
}
