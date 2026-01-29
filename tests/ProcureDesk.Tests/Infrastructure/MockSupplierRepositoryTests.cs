using ProcureDesk.Domain;
using ProcureDesk.Infrastructure;
using Xunit;

namespace ProcureDesk.Tests;

public class MockSupplierRepositoryTests
{
    [Fact]
    public void Add_ShouldStoreSupplier_AndFindByCodeShouldReturnIt()
    {
        var repo = new MockSupplierRepository();
        var supplier = new Supplier("S001", "ACME");

        repo.Add(supplier);

        var found = repo.FindByCode("S001");

        Assert.NotNull(found);
        Assert.Equal("S001", found!.Code);
        Assert.Equal("ACME", found!.Name);
    }

    [Fact]
    public void FindByCode_ShouldReturnNull_WhenSupplierDoesNotExist()
    {
        var repo = new MockSupplierRepository();

        var found = repo.FindByCode("S999");

        Assert.Null(found);
    }

    [Fact]
    public void List_ShouldReturnAllSuppliers()
    {
        var repo = new MockSupplierRepository();
        repo.Add(new Supplier("S001", "ACME"));
        repo.Add(new Supplier("S002", "BoltCo"));

        var suppliers = repo.List().ToList();

        Assert.Equal(2, suppliers.Count);
        Assert.Contains(suppliers, s => s.Code == "S001");
        Assert.Contains(suppliers, s => s.Code == "S002");
    }

    [Fact]
    public void List_ShouldReturnCopy_NotInternalList()
    {
        var repo = new MockSupplierRepository();
        repo.Add(new Supplier("S001", "ACME"));

        var list = repo.List().ToList();
        list.Clear(); // modify returned list

        Assert.Single(repo.List()); // internal collection unaffected
    }

    [Fact]
    public void Update_ShouldReplaceExistingSupplier()
    {
        var repo = new MockSupplierRepository();
        repo.Add(new Supplier("S001", "ACME"));

        repo.Update(new Supplier("S001", "ACME SA"));

        var found = repo.FindByCode("S001");

        Assert.NotNull(found);
        Assert.Equal("ACME SA", found!.Name);
    }

    [Fact]
    public void Update_ShouldDoNothing_WhenSupplierDoesNotExist()
    {
        var repo = new MockSupplierRepository();

        repo.Update(new Supplier("S001", "ACME"));

        Assert.Empty(repo.List());
    }

    [Fact]
    public void Delete_ShouldRemoveExistingSupplier()
    {
        var repo = new MockSupplierRepository();
        repo.Add(new Supplier("S001", "ACME"));

        repo.Delete("S001");

        Assert.Null(repo.FindByCode("S001"));
        Assert.Empty(repo.List());
    }

    [Fact]
    public void Delete_ShouldDoNothing_WhenSupplierDoesNotExist()
    {
        var repo = new MockSupplierRepository();

        repo.Delete("S001");

        Assert.Empty(repo.List());
    }
}
