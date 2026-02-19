using ProcureDesk.Application;
using ProcureDesk.Domain;
using ProcureDesk.Infrastructure;
using Xunit;

namespace ProcureDesk.Tests.Application;

public class SupplierProductApplicationServiceTests
{
    [Fact]
    public void Create_ShouldAddLink_WhenValid()
    {
        var repo = new MockSupplierProductRepository();
        var svc = new SupplierProductApplicationService(repo);

        var (ok, errors) = svc.Create("S001", "P001", "Supplier's Widget", "tester");

        Assert.True(ok);
        Assert.Empty(errors);
        Assert.NotNull(repo.Get("S001", "P001"));
    }

    [Fact]
    public void Create_ShouldFail_WhenDuplicate()
    {
        var repo = new MockSupplierProductRepository();
        var (ok1, e1, link) = SupplierProduct.Create("S001", "P001", "Widget", "t");
        repo.Add(link!);

        var svc = new SupplierProductApplicationService(repo);
        var (ok, errors) = svc.Create("S001", "P001", "Other Widget", "tester");

        Assert.False(ok);
        Assert.Contains("Supplier already supplies this product.", errors);
    }

    [Fact]
    public void UpdatePrice_ShouldUpdate_WhenFound()
    {
        var repo = new MockSupplierProductRepository();
        var (ok, e, link) = SupplierProduct.Create("S001", "P001", "Widget", "t");
        repo.Add(link!);

        var svc = new SupplierProductApplicationService(repo);
        svc.UpdatePrice("S001", "P001", 99.99m, "updater");

        var found = repo.Get("S001", "P001");
        Assert.Equal(99.99m, found!.Price);
    }

    [Fact]
    public void UpdatePrice_ShouldDoNothing_WhenNotFound()
    {
        var repo = new MockSupplierProductRepository();
        var svc = new SupplierProductApplicationService(repo);

        svc.UpdatePrice("S999", "P999", 100m, "updater");

        Assert.Null(repo.Get("S999", "P999"));
    }

    [Fact]
    public void UpdateLeadTime_ShouldUpdate_WhenFound()
    {
        var repo = new MockSupplierProductRepository();
        var (ok, e, link) = SupplierProduct.Create("S001", "P001", "Widget", "t");
        repo.Add(link!);

        var svc = new SupplierProductApplicationService(repo);
        svc.UpdateLeadTime("S001", "P001", 14, "updater");

        var found = repo.Get("S001", "P001");
        Assert.Equal(14, found!.LeadTimeDays);
    }

    [Fact]
    public void UpdateLeadTime_ShouldDoNothing_WhenNotFound()
    {
        var repo = new MockSupplierProductRepository();
        var svc = new SupplierProductApplicationService(repo);

        svc.UpdateLeadTime("S999", "P999", 10, "updater");

        Assert.Null(repo.Get("S999", "P999"));
    }
}
