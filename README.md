# DesafioInvoiSys

Aplicação console em .NET para processar lote de documentos fiscais em JSON.

Nesta primeira etapa eu optei por suportar somente **NF-e** (`NFE`), como o desafio permite escolher um único tipo.

## Objetivo

- Ler arquivo JSON de entrada
- Validar cada documento do lote
- Classificar como `VALIDO` ou `INVALIDO`
- Retornar erros por documento inválido
- Gerar resumo consolidado do lote

## Pré-requisitos

- [.NET SDK 10.0](https://dotnet.microsoft.com/download) ou superior, compatível com `net10.0`
- macOS, Linux ou Windows

## Execução

Na raiz do repositório:

```bash
dotnet restore
dotnet test
dotnet run --project src/DesafioInvoiSys.App/DesafioInvoiSys.App.csproj -- exemplos/exemplo-1-basico.json
```

Saída:
- o comando recebe apenas o arquivo de entrada
- o arquivo de saída é gerado automaticamente no mesmo diretório com sufixo `-saida`
- exemplo: `exemplo-1-basico.json` -> `exemplo-1-basico-saida.json`

Códigos de saída da CLI:
- `0` sucesso
- `1` uso inválido
- `2` erro de leitura
- `3` JSON inválido
- `4` erro ao gravar saída

## Estrutura

- `src/DesafioInvoiSys.Core/DesafioInvoiSys.Core.csproj`: regras de negócio, validações, modelos e processamento do lote
- `src/DesafioInvoiSys.App/DesafioInvoiSys.App.csproj`: ponto de entrada da CLI
- `tests/DesafioInvoiSys.Tests/DesafioInvoiSys.Tests.csproj`: testes automatizados
- `exemplos/`: arquivos de entrada para execução local

Exemplos disponíveis:
- `exemplo-1-basico.json`: cenário básico com 1 válido e 1 inválido
- `exemplo-2-lote-misto.json`: lote misto com válido, inválidos de regra e duplicidade
- `exemplo-3-duplicidade-em-cadeia.json`: foco em duplicidade com variação de máscara e caixa
- `exemplo-4-campos-ausentes-e-formato-flexivel.json`: foco em campos ausentes, nulos e formatos alternativos

## Decisões técnicas

- Optei por aplicação console para manter aderência ao escopo e facilitar execução
- Usei `.NET 10` com `System.Text.Json` para reduzir dependências externas
- Separei em `Core` e `App` para isolar regra de negócio de I/O
- Estruturei as validações com uma DSL fluente em C# para facilitar evolução sem acoplamento por classe de regra
- A validação de duplicidade roda após validações de campo, pois depende do lote completo
- Em caso de duplicidade, marco todos os documentos envolvidos como inválidos
- Normalizei a chave de duplicidade (`tipo`, `cnpjEmitente`, `serie`, `numero`) para evitar falso negativo por máscara de CNPJ

## DSL de validação

A validação de documentos está centralizada em um schema fluente no `DocumentValidator`, usando:
- `SupportsType(...)` para declarar tipos suportados
- `Require(...)` para regra de obrigatório
- `AddRule(..., when: ...)` para regra parametrizada e condicional

Exemplo simplificado:

```csharp
new DocumentSchemaBuilder()
    .SupportsType("NFE")
    .Require(d => d.Id, ValidationMessages.MissingId)
    .AddRule(
        d => d.Value,
        v => v > 0m,
        ValidationMessages.ValueMustBeGreaterThanZero,
        value => value is not null)
    .Build();
```

Com isso, para suportar novos cenários de validação, a principal mudança fica na declaração do schema, preservando o motor de execução genérico.
