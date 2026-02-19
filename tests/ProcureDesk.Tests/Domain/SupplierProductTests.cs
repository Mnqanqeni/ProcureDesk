using ProcureDesk.Domain;
using Xunit;

namespace ProcureDesk.Tests.Domain;

public class SupplierProductTests
{
    [Fact]
    public void Create_ShouldReturnValid_WhenDataOk()
    {
        var (ok, errors, link) = SupplierProduct.Create("S001", "P001", "Item Name", "user1");

        Assert.True(ok);
        Assert.Empty(errors);
        Assert.NotNull(link);
        Assert.Equal("S001", link!.SupplierCode);
        Assert.Equal("P001", link.ProductCode);
        Assert.Equal("Item Name", link.SupplierProductName);
    }

    [Fact]
    public void Create_ShouldFail_WhenSupplierCodeMissing()
    {
        var (ok, errors, link) = SupplierProduct.Create("", "P001", "Item Name", "user1");

        Assert.False(ok);
        Assert.Contains("Supplier code is required.", errors);
    }

    [Fact]
    public void Create_ShouldFail_WhenProductCodeMissing()
    {
        var (ok, errors, link) = SupplierProduct.Create("S001", "", "Item Name", "user1");

        Assert.False(ok);
        Assert.Contains("Product code is required.", errors);
    }

    [Fact]
    public void Create_ShouldFail_WhenNameMissing()
    {
        var (ok, errors, link) = SupplierProduct.Create("S001", "P001", "", "user1");

        Assert.False(ok);
        Assert.Contains("Supplier product name is required.", errors);
    }

    [Fact]
    public void Validate_ShouldPass_WhenValid()
    {
        var (ok, errors) = SupplierProduct.Validate("S001", "P001", "Item Name");

        Assert.True(ok);
        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_ShouldFail_WhenSupplierCodeNull()
    {
        var (ok, errors) = SupplierProduct.Validate(null!, "P001", "Item Name");

        Assert.False(ok);
        Assert.Contains("Supplier code is required.", errors);
    }

    [Fact]
    public void Validate_ShouldFail_WhenProductCodeNull()
    {
        var (ok, errors) = SupplierProduct.Validate("S001", null!, "Item Name");

        Assert.False(ok);
        Assert.Contains("Product code is required.", errors);
    }

    [Fact]
    public void Validate_ShouldFail_WhenNameNull()
    {
        var (ok, errors) = SupplierProduct.Validate("S001", "P001", null!);

        Assert.False(ok);
        Assert.Contains("Supplier product name is required.", errors);
    }
}
