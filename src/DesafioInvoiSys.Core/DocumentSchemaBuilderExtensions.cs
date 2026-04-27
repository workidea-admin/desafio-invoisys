namespace DesafioInvoiSys;

internal static class DocumentSchemaBuilderExtensions
{
    public static DocumentSchemaBuilder RequirePositiveValue(
        this DocumentSchemaBuilder builder,
        Func<InputDocument, decimal?> selector,
        string missingMessage,
        string invalidMessage) =>
        builder
            .AddRule(selector, value => value is not null, missingMessage)
            .AddRule(selector, value => value > 0m, invalidMessage, value => value is not null);

    public static DocumentSchemaBuilder RequireValidCnpj(
        this DocumentSchemaBuilder builder,
        Func<InputDocument, string?> selector,
        string missingMessage,
        string invalidMessage) =>
        builder
            .Require(selector, missingMessage)
            .AddRule(
                selector,
                cnpj => CnpjHelper.DigitsOnly(cnpj).Length == 14,
                invalidMessage,
                cnpj => !string.IsNullOrWhiteSpace(cnpj));

    public static DocumentSchemaBuilder ValidateOptionalCnpj(
        this DocumentSchemaBuilder builder,
        Func<InputDocument, string?> selector,
        string invalidMessage,
        Func<string?, bool> isProvided) =>
        builder.AddRule(
            selector,
            cnpj => CnpjHelper.DigitsOnly(cnpj).Length == 14,
            invalidMessage,
            isProvided);

    public static DocumentSchemaBuilder RequireDate(
        this DocumentSchemaBuilder builder,
        Func<InputDocument, string?> selector,
        string missingMessage,
        string invalidMessage,
        Func<string, bool> isValidDate) =>
        builder
            .Require(selector, missingMessage)
            .AddRule(
                selector,
                date => date is not null && isValidDate(date),
                invalidMessage,
                date => !string.IsNullOrWhiteSpace(date));
}
