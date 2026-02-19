using ProcureDesk.Domain;
using ProcureDesk.Infrastructure;
using System.Linq;
using Xunit;

namespace ProcureDesk.Tests.Infrastructure;

public class MockSupplierProductRepositoryTests
{
    [Fact]
    public void Add_ShouldStoreLink()
    {
        var repo = new MockSupplierProductRepository();
        var (ok, errs, link) = SupplierProduct.Create("S001", "P001", "Supplier's Widget", "tester");

        repo.Add(link!);

        var found = repo.Get("S001", "P001");
        Assert.NotNull(found);
        Assert.Equal("Supplier's Widget", found!.SupplierProductName);
    }

    [Fact]
    public void Get_ShouldReturnNull_WhenNotFound()
    {
        var repo = new MockSupplierProductRepository();

        var found = repo.Get("S999", "P999");

        Assert.Null(found);
    }

    [Fact]
    public void GetBySupplier_ShouldReturnAllProductsForSupplier()
    {
        var repo = new MockSupplierProductRepository();
        var (ok1, e1, link1) = SupplierProduct.Create("S001", "P001", "Widget", "t");
        var (ok2, e2, link2) = SupplierProduct.Create("S001", "P002", "Gadget", "t");
        var (ok3, e3, link3) = SupplierProduct.Create("S002", "P001", "Thing", "t");

        repo.Add(link1!);
        repo.Add(link2!);
        repo.Add(link3!);

        var products = repo.GetBySupplier("S001");

        Assert.Equal(2, products.Count);
        Assert.All(products, p => Assert.Equal("S001", p.SupplierCode));
    }

    [Fact]
    public void GetByProduct_ShouldReturnAllSuppliersForProduct()
    {
        var repo = new MockSupplierProductRepository();
        var (ok1, e1, link1) = SupplierProduct.Create("S001", "P001", "Widget A", "t");
        var (ok2, e2, link2) = SupplierProduct.Create("S002", "P001", "Widget B", "t");
        var (ok3, e3, link3) = SupplierProduct.Create("S001", "P002", "Gadget", "t");

        repo.Add(link1!);
        repo.Add(link2!);
        repo.Add(link3!);

        var suppliers = repo.GetByProduct("P001");

        Assert.Equal(2, suppliers.Count);
        Assert.All(suppliers, s => Assert.Equal("P001", s.ProductCode));
    }

    [Fact]
    public void Update_ShouldModifyExistingLink()
    {
        var repo = new MockSupplierProductRepository();
        var (ok, e, link) = SupplierProduct.Create("S001", "P001", "Widget", "t");
        repo.Add(link!);

        Assert.NotNull(link);
        link!.Price = 150.50m;
        link.LeadTimeDays = 7;
        repo.Update(link);

        var found = repo.Get("S001", "P001");
        Assert.Equal(150.50m, found!.Price);
        Assert.Equal(7, found.LeadTimeDays);
    }

    [Fact]
    public void Delete_ShouldRemoveLink()
    {
        var repo = new MockSupplierProductRepository();
        var (ok, e, link) = SupplierProduct.Create("S001", "P001", "Widget", "t");
        repo.Add(link!);

        repo.Delete("S001", "P001");

        Assert.Null(repo.Get("S001", "P001"));
    }

    [Fact]
    public void Delete_ShouldDoNothing_WhenLinkNotFound()
    {
        var repo = new MockSupplierProductRepository();

        repo.Delete("S999", "P999");

        Assert.Null(repo.Get("S999", "P999"));
    }
}
