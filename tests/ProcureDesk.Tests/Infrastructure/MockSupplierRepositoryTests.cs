using ProcureDesk.Domain;
using ProcureDesk.Infrastructure;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace ProcureDesk.Tests;

public class MockSupplierRepositoryTests
{
    [Fact]
    public void Add_ShouldStoreSupplier_AndGetByCodeShouldReturnIt()
    {
        var repo = new MockSupplierRepository();
        var (ok, errors, supplier) = Supplier.Create("S001", "ACME", "test");
        Assert.True(ok);

        repo.Add(supplier!);

        var found = repo.GetByCode("S001");

        Assert.NotNull(found);
        Assert.Equal("S001", found!.Code);
        Assert.Equal("ACME", found!.Name);
    }

    [Fact]
    public void GetByCode_ShouldReturnNull_WhenSupplierDoesNotExist()
    {
        var repo = new MockSupplierRepository();

        var found = repo.GetByCode("S999");

        Assert.Null(found);
    }

    [Fact]
    public void List_ShouldReturnAllSuppliers()
    {
        var repo = new MockSupplierRepository();
        var (ok1, e1, s1) = Supplier.Create("S001", "ACME", "test");
        var (ok2, e2, s2) = Supplier.Create("S002", "BoltCo", "test");
        repo.Add(s1!);
        repo.Add(s2!);

        var suppliers = repo.List().ToList();

        Assert.Equal(2, suppliers.Count);
        Assert.Contains(suppliers, s => s.Code == "S001");
        Assert.Contains(suppliers, s => s.Code == "S002");
    }

    [Fact]
    public void List_ShouldReturnCopy_NotInternalList()
    {
        var repo = new MockSupplierRepository();
        var (ok, err, s) = Supplier.Create("S001", "ACME", "test");
        repo.Add(s!);

        var list = repo.List().ToList();
        list.Clear(); // modify returned list

        Assert.Single(repo.List()); // internal collection unaffected
    }

    [Fact]
    public void Update_ShouldReplaceExistingSupplier()
    {
        var repo = new MockSupplierRepository();
        var (ok, e, original) = Supplier.Create("S001", "ACME", "test");
        repo.Add(original!);

        var (ok2, e2, updated) = Supplier.Create("S001", "ACME SA", "test");
        repo.Update(updated!);

        var found = repo.GetByCode("S001");

        Assert.NotNull(found);
        Assert.Equal("ACME SA", found!.Name);
    }

    [Fact]
    public void Update_ShouldDoNothing_WhenSupplierDoesNotExist()
    {
        var repo = new MockSupplierRepository();

        var (ok, e, s) = Supplier.Create("S001", "ACME", "test");

        Assert.Throws<KeyNotFoundException>(() => repo.Update(s!));
    }

    [Fact]
    public void Delete_ShouldRemoveExistingSupplier()
    {
        var repo = new MockSupplierRepository();
        var (ok, e, s) = Supplier.Create("S001", "ACME", "test");
        repo.Add(s!);

        repo.Delete("S001");

        Assert.Null(repo.GetByCode("S001"));
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
