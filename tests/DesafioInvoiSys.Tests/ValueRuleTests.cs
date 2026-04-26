using DesafioInvoiSys;
using Xunit;

namespace DesafioInvoiSys.Tests;

public class ValueRuleTests
{
    [Fact]
    public void GivenNullValue_WhenValidating_ThenAddsMissingValueError()
    {
        var rule = new ValueRule();
        var document = new InputDocument { Value = null };
        var errors = new List<string>();

        rule.Validate(document, errors);

        Assert.Contains(ValidationMessages.MissingValue, errors);
    }

    [Fact]
    public void GivenZeroValue_WhenValidating_ThenAddsGreaterThanZeroError()
    {
        var rule = new ValueRule();
        var document = new InputDocument { Value = 0m };
        var errors = new List<string>();

        rule.Validate(document, errors);

        Assert.Contains(ValidationMessages.ValueMustBeGreaterThanZero, errors);
    }
}
