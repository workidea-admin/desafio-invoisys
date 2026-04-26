using System.Text.Json;
using DesafioInvoiSys;

const int ExitOk = 0;
const int ExitUsage = 1;
const int ExitIo = 2;
const int ExitJson = 3;
const int ExitWrite = 4;

if (args.Length != 1)
{
    return Fail("Uso: dotnet run --project src/DesafioInvoiSys.App/DesafioInvoiSys.App.csproj -- <exemplo-1-basico.json>", ExitUsage);
}

var inputPath = args[0];
var outputPath = OutputPathUtil.BuildOutputPath(inputPath);

if (!File.Exists(inputPath))
{
    return Fail($"Arquivo de input não encontrado: {inputPath}", ExitIo);
}

string jsonText;
try
{
    jsonText = File.ReadAllText(inputPath);
}
catch (Exception ex)
{
    return Fail($"Erro ao ler input: {ex.Message}", ExitIo);
}

InputBatch? batch;
try
{
    batch = JsonSerializer.Deserialize<InputBatch>(jsonText, BatchJsonContext.InputOptions);
}
catch (JsonException ex)
{
    return Fail($"JSON inválido: {ex.Message}", ExitJson);
}

if (batch is null)
{
    return Fail("JSON inválido: não foi possível interpretar o batch.", ExitJson);
}

var processor = new BatchProcessor();
var output = processor.Process(batch);

try
{
    var outputJson = JsonSerializer.Serialize(output, BatchJsonContext.OutputOptions);
    File.WriteAllText(outputPath, outputJson);
    Console.WriteLine($"Arquivo de saída gerado: {outputPath}");
}
catch (Exception ex)
{
    return Fail($"Erro ao gravar saída: {ex.Message}", ExitWrite);
}

return ExitOk;

static int Fail(string message, int code)
{
    Console.Error.WriteLine(message);
    return code;
}
