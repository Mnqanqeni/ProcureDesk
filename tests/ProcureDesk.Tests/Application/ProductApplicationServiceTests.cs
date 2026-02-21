using ProcureDesk.Application;
using ProcureDesk.Domain;
using ProcureDesk.Infrastructure;
using System.Linq;
using Xunit;

namespace ProcureDesk.Tests.Application;

public class ProductApplicationServiceTests
{
    [Fact]
    public void Create_ShouldAddProduct_WhenValid()
    {
        var repo = new MockProductRepository();
        var svc = new ProductApplicationService(repo);

        var (ok, errors, product) = svc.Create("P001", "Widget", "tester");

        Assert.True(ok);
        Assert.Empty(errors);
        Assert.NotNull(product);
        Assert.NotNull(repo.Get("P001"));
    }

	[Fact]
	public void Create_ShouldFail_WhenCodeMissing()
	{
		var repo = new MockProductRepository();
		var svc = new ProductApplicationService(repo);

		var (ok, errors, product) = svc.Create("", "Widget", "tester");

		Assert.False(ok);
		Assert.Contains("Code is required.", errors);
		Assert.Null(product);
	}

    [Fact]
    public void UpdateName_ShouldUpdate_WhenFound()
    {
        var repo = new MockProductRepository();
        var (ok1, e1, p1) = Product.Create("P002", "Gizmo", "t");
        repo.Add(p1!);

        var svc = new ProductApplicationService(repo);
        var (ok, errors) = svc.UpdateName("P002", "Gizmo Plus", "updater");

        Assert.True(ok);
        Assert.Empty(errors);
        Assert.Equal("Gizmo Plus", repo.Get("P002")!.Name);
    }

    [Fact]
    public void Delete_ShouldRemove_WhenFound()
    {
        var repo = new MockProductRepository();
        var (ok1, e1, p1) = Product.Create("P003", "Thing", "t");
        repo.Add(p1!);

        var svc = new ProductApplicationService(repo);
        var (ok, errors) = svc.Delete("P003");

        Assert.True(ok);
        Assert.Null(repo.Get("P003"));
    }
}
