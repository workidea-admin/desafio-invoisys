using DesafioInvoiSys;
using Xunit;

namespace DesafioInvoiSys.Tests;

public class OutputPathUtilTests
{
    [Theory]
    [InlineData("exemplo-1-basico.json", "exemplo-1-basico-saida.json")]
    [InlineData("input.json", "input-saida.json")]
    [InlineData("file", "file-saida")]
    public void BuildOutputPath_AppendsSuffixToRelativeFile(string input, string expected)
    {
        var actual = OutputPathUtil.BuildOutputPath(input);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BuildOutputPath_KeepsDirectoryWhenPathIsProvided()
    {
        var input = Path.Combine("exemplos", "exemplo-1-basico.json");
        var expected = Path.Combine("exemplos", "exemplo-1-basico-saida.json");

        var actual = OutputPathUtil.BuildOutputPath(input);

        Assert.Equal(expected, actual);
    }
}
