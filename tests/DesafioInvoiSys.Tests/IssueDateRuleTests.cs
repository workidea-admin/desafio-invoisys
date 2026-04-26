using DesafioInvoiSys;
using Xunit;

namespace DesafioInvoiSys.Tests;

public class IssueDateRuleTests
{
    [Fact]
    public void GivenInvalidIssueDate_WhenValidating_ThenAddsInvalidIssueDateError()
    {
        var rule = new IssueDateRule();
        var document = new InputDocument { IssueDate = "not-a-date" };
        var errors = new List<string>();

        rule.Validate(document, errors);

        Assert.Contains(ValidationMessages.InvalidIssueDate, errors);
    }
}
