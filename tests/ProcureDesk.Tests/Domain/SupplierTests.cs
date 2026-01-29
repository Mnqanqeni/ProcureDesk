using ProcureDesk.Domain;
using Xunit;

namespace ProcureDesk.Tests;

public class SupplierTests
{
    [Fact]
    public void Validate_Fails_WhenCodeMissing()
    {
        var (ok, msg) = Supplier.Validate("", "ACME");
        Assert.False(ok);
        Assert.Contains("Code is required.", msg);
    }

    [Fact]
    public void Validate_Fails_WhenNameMissing()
    {
        var (ok, msg) = Supplier.Validate("S001", "");
        Assert.False(ok);
        Assert.Contains("Name is required.", msg);
    }

    [Fact]
    public void Validate_Passes_WhenValid()
    {
        var (ok, msg) = Supplier.Validate("S001", "ACME");
        Assert.True(ok);
        Assert.Equal(string.Empty, msg);
    }
}
