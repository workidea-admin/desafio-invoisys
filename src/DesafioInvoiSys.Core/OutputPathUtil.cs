namespace DesafioInvoiSys;

public static class OutputPathUtil
{
    public static string BuildOutputPath(string inputPath)
    {
        var directory = Path.GetDirectoryName(inputPath);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputPath);
        var extension = Path.GetExtension(inputPath);

        var outputFileName = string.IsNullOrWhiteSpace(extension)
            ? $"{fileNameWithoutExtension}-saida"
            : $"{fileNameWithoutExtension}-saida{extension}";

        return string.IsNullOrWhiteSpace(directory)
            ? outputFileName
            : Path.Combine(directory, outputFileName);
    }
}
