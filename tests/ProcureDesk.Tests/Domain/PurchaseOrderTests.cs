using ProcureDesk.Domain;
using Xunit;

namespace ProcureDesk.Tests;

public class PurchaseOrderTests
{
    [Fact]
    public void PurchaseOrder_Validate_Fails_WhenOrderNumberMissing()
    {
        var (isValid, errors) = PurchaseOrder.Validate("", "S001");
        Assert.False(isValid);
        Assert.Contains("Order number is required.", errors);
    }

    [Fact]
    public void PurchaseOrder_Validate_Fails_WhenSupplierCodeMissing()
    {
        var (isValid, errors) = PurchaseOrder.Validate("PO-1", "");
        Assert.False(isValid);
        Assert.Contains("Supplier code is required.", errors);
    }

    [Fact]
    public void PurchaseOrder_Validate_Passes_WhenValid()
    {
        var (isValid, errors) = PurchaseOrder.Validate("PO-1", "S001");
        Assert.True(isValid);
        Assert.Empty(errors);
    }

    [Fact]
    public void PurchaseOrderLine_Validate_Fails_WhenGoodCodeMissing()
    {
        Product? product = null;
        var (isValid, errors) = PurchaseOrderLine.Validate(product, 1, 10m);
        Assert.False(isValid);
        Assert.Contains("Product is required.", errors);
    }

    [Fact]
    public void PurchaseOrderLine_Validate_Fails_WhenQuantityNotPositive()
    {
        var (ok, errs, product) = Product.Create("B001", "Bolt", "test");
        var (isValid, errors) = PurchaseOrderLine.Validate(product, 0, 10m);
        Assert.False(isValid);
        Assert.Contains("Quantity must be greater than 0.", errors);
    }

    [Fact]
    public void PurchaseOrderLine_Validate_Fails_WhenUnitPriceNegative()
    {
        var (ok, errs, product) = Product.Create("B001", "Bolt", "test");
        var (isValid, errors) = PurchaseOrderLine.Validate(product, 1, -1m);
        Assert.False(isValid);
        Assert.Contains("Price cannot be negative.", errors);
    }

    [Fact]
    public void PurchaseOrderLine_Validate_Passes_WhenValid()
    {
        var (ok, errs, product) = Product.Create("B001", "Bolt", "test");
        var (isValid, errors) = PurchaseOrderLine.Validate(product, 1, 10m);
        Assert.True(isValid);
        Assert.Empty(errors);
    }
}
