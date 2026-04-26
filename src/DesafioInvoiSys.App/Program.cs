using System.Text.Json;
using DesafioInvoiSys;

const int ExitOk = 0;
const int ExitUsage = 1;
const int ExitIo = 2;
const int ExitJson = 3;
const int ExitWrite = 4;

if (!TryGetInputPath(args, out var inputPath, out var inputPathError))
{
    return Fail(inputPathError, ExitUsage);
}

var outputPath = OutputPathUtil.BuildOutputPath(inputPath);

if (!TryReadInputFile(inputPath, out var jsonText, out var readError))
{
    return Fail(readError, ExitIo);
}

if (!TryDeserializeBatch(jsonText, out var batch, out var deserializeError))
{
    return Fail(deserializeError, ExitJson);
}

var processor = new BatchProcessor();
var output = processor.Process(batch);

if (!TryWriteOutput(outputPath, output, out var writeError))
{
    return Fail(writeError, ExitWrite);
}

return ExitOk;

static bool TryGetInputPath(string[] args, out string inputPath, out string error)
{
    if (args.Length == 1)
    {
        inputPath = args[0];
        error = string.Empty;
        return true;
    }

    inputPath = string.Empty;
    error = "Uso: dotnet run --project src/DesafioInvoiSys.App/DesafioInvoiSys.App.csproj -- <exemplo-1-basico.json>";
    return false;
}

static bool TryReadInputFile(string inputPath, out string jsonText, out string error)
{
    if (!File.Exists(inputPath))
    {
        jsonText = string.Empty;
        error = $"Arquivo de input não encontrado: {inputPath}";
        return false;
    }

    try
    {
        jsonText = File.ReadAllText(inputPath);
        error = string.Empty;
        return true;
    }
    catch (Exception ex)
    {
        jsonText = string.Empty;
        error = $"Erro ao ler input: {ex.Message}";
        return false;
    }
}

static bool TryDeserializeBatch(string jsonText, out InputBatch batch, out string error)
{
    try
    {
        var parsedBatch = JsonSerializer.Deserialize<InputBatch>(jsonText, BatchJsonContext.InputOptions);
        if (parsedBatch is null)
        {
            batch = default!;
            error = "JSON inválido: não foi possível interpretar o batch.";
            return false;
        }

        batch = parsedBatch;
        error = string.Empty;
        return true;
    }
    catch (JsonException ex)
    {
        batch = default!;
        error = $"JSON inválido: {ex.Message}";
        return false;
    }
}

static bool TryWriteOutput(string outputPath, OutputBatch output, out string error)
{
    try
    {
        var outputJson = JsonSerializer.Serialize(output, BatchJsonContext.OutputOptions);
        File.WriteAllText(outputPath, outputJson);
        Console.WriteLine($"Arquivo de saída gerado: {outputPath}");
        error = string.Empty;
        return true;
    }
    catch (Exception ex)
    {
        error = $"Erro ao gravar saída: {ex.Message}";
        return false;
    }
}

static int Fail(string message, int code)
{
    Console.Error.WriteLine(message);
    return code;
}
