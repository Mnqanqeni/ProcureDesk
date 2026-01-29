using ProcureDesk.Domain;
using Xunit;

namespace ProcureDesk.Tests;

public class PurchaseOrderTests
{
    [Fact]
    public void PurchaseOrder_Validate_Fails_WhenOrderNumberMissing()
    {
        var (isValid, message) = PurchaseOrder.Validate("", "S001");
        Assert.False(isValid);
        Assert.Contains("Order number is required.", message);
    }

    [Fact]
    public void PurchaseOrder_Validate_Fails_WhenSupplierCodeMissing()
    {
        var (isValid, message) = PurchaseOrder.Validate("PO-1", "");
        Assert.False(isValid);
        Assert.Contains("Supplier code is required.", message);
    }

    [Fact]
    public void PurchaseOrder_Validate_Passes_WhenValid()
    {
        var (isValid, message) = PurchaseOrder.Validate("PO-1", "S001");
        Assert.True(isValid);
        Assert.Equal(string.Empty, message);
    }

    [Fact]
    public void PurchaseOrderLine_Validate_Fails_WhenGoodCodeMissing()
    {
        var (isValid, message) = PurchaseOrderLine.Validate("", 1, 10m);
        Assert.False(isValid);
        Assert.Contains("Good code is required.", message);
    }

    [Fact]
    public void PurchaseOrderLine_Validate_Fails_WhenQuantityNotPositive()
    {
        var (isValid, message) = PurchaseOrderLine.Validate("B001", 0, 10m);
        Assert.False(isValid);
        Assert.Contains("Quantity must be greater than 0.", message);
    }

    [Fact]
    public void PurchaseOrderLine_Validate_Fails_WhenUnitPriceNegative()
    {
        var (isValid, message) = PurchaseOrderLine.Validate("B001", 1, -1m);
        Assert.False(isValid);
        Assert.Contains("Unit price cannot be negative.", message);
    }

    [Fact]
    public void PurchaseOrderLine_Validate_Passes_WhenValid()
    {
        var (isValid, message) = PurchaseOrderLine.Validate("B001", 1, 10m);
        Assert.True(isValid);
        Assert.Equal(string.Empty, message);
    }
}
