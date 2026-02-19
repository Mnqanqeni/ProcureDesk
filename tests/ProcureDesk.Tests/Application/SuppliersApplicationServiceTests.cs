using ProcureDesk.Application;
using ProcureDesk.Domain;
using ProcureDesk.Infrastructure;
using Xunit;

namespace ProcureDesk.Tests.Application;

public class SuppliersApplicationServiceTests
{
    [Fact]
    public void Create_ShouldAddSupplier_WhenValid()
    {
        var repo = new MockSupplierRepository();
        var svc = new SuppliersApplicationService(repo);

        var (ok, errors, supplier) = svc.Create("S001", "ACME", "tester");

        Assert.True(ok);
        Assert.NotNull(supplier);
        Assert.NotNull(repo.GetByCode("S001"));
    }

    [Fact]
    public void Create_ShouldFail_WhenDuplicate()
    {
        var repo = new MockSupplierRepository();
        var (ok1, e1, s1) = Supplier.Create("S002", "BoltCo", "t");
        repo.Add(s1!);

        var svc = new SuppliersApplicationService(repo);
        var (ok, errors, supplier) = svc.Create("S002", "BoltCo", "tester");

        Assert.False(ok);
        Assert.Contains("Supplier code already exists.", errors);
    }

    [Fact]
    public void UpdateName_ShouldUpdate_WhenFound()
    {
        var repo = new MockSupplierRepository();
        var (ok1, e1, s1) = Supplier.Create("S003", "Old", "t");
        repo.Add(s1!);

        var svc = new SuppliersApplicationService(repo);
        var (ok, errors) = svc.UpdateName("S003", "NewName", "updater");

        Assert.True(ok);
        Assert.Equal("NewName", repo.GetByCode("S003")!.Name);
    }

    [Fact]
    public void Delete_ShouldRemove_WhenFound()
    {
        var repo = new MockSupplierRepository();
        var (ok1, e1, s1) = Supplier.Create("S004", "DeleteMe", "t");
        repo.Add(s1!);

        var svc = new SuppliersApplicationService(repo);
        var (ok, errors) = svc.Delete("S004");

        Assert.True(ok);
        Assert.Null(repo.GetByCode("S004"));
    }
}
