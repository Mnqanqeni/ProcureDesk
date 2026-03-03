namespace ProcureDesk.Api.Contracts.Products;

public sealed class CreateProductRequest
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
}