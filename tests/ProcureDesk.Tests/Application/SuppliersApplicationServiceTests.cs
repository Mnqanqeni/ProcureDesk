using ProcureDesk.Application;
using ProcureDesk.Domain;
using ProcureDesk.Infrastructure;
using Xunit;

namespace ProcureDesk.Tests;

public class SuppliersApplicationServiceTests
{
    [Fact]
    public void ListSuppliers_ShouldReturnAllSuppliers()
    {
        var repo = new MockSupplierRepository();
        repo.Add(new Supplier("S001", "ACME"));
        repo.Add(new Supplier("S002", "BoltCo"));

        var service = new SuppliersApplicationService(repo);

        var result = service.ListSuppliers();

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value!.Count());
    }

    [Fact]
    public void GetSupplierByCode_ShouldFail_WhenCodeIsEmpty()
    {
        var service = new SuppliersApplicationService(new MockSupplierRepository());

        var result = service.GetSupplierByCode("  ");

        Assert.False(result.IsSuccess);
        Assert.Equal("Code is required.", result.Error);
    }

    [Fact]
    public void GetSupplierByCode_ShouldFail_WhenSupplierNotFound()
    {
        var service = new SuppliersApplicationService(new MockSupplierRepository());

        var result = service.GetSupplierByCode("S001");

        Assert.False(result.IsSuccess);
        Assert.Equal("Supplier not found.", result.Error);
    }

    [Fact]
    public void GetSupplierByCode_ShouldReturnSupplier_WhenFound()
    {
        var repo = new MockSupplierRepository();
        repo.Add(new Supplier("S001", "ACME"));
        var service = new SuppliersApplicationService(repo);

        var result = service.GetSupplierByCode("S001");

        Assert.True(result.IsSuccess);
        Assert.Equal("S001", result.Value!.Code);
        Assert.Equal("ACME", result.Value!.Name);
    }

    [Fact]
    public void CreateSupplier_ShouldFail_WhenValidationFails()
    {
        var service = new SuppliersApplicationService(new MockSupplierRepository());

        var result = service.CreateSupplier("", "");

        Assert.False(result.IsSuccess);
        Assert.Contains("Code is required.", result.Error);
        Assert.Contains("Name is required.", result.Error);
    }

    [Fact]
    public void CreateSupplier_ShouldFail_WhenCodeAlreadyExists()
    {
        var repo = new MockSupplierRepository();
        repo.Add(new Supplier("S001", "ACME"));
        var service = new SuppliersApplicationService(repo);

        var result = service.CreateSupplier("S001", "Another Name");

        Assert.False(result.IsSuccess);
        Assert.Equal("A supplier with this code already exists.", result.Error);
    }

    [Fact]
    public void CreateSupplier_ShouldAddSupplier_WhenValid()
    {
        var repo = new MockSupplierRepository();
        var service = new SuppliersApplicationService(repo);

        var result = service.CreateSupplier("S001", "ACME");

        Assert.True(result.IsSuccess);
        Assert.NotNull(repo.FindByCode("S001"));
    }

    [Fact]
    public void RenameSupplier_ShouldFail_WhenCodeIsEmpty()
    {
        var service = new SuppliersApplicationService(new MockSupplierRepository());

        var result = service.RenameSupplier(" ", "New Name");

        Assert.False(result.IsSuccess);
        Assert.Equal("Code is required.", result.Error);
    }

    [Fact]
    public void RenameSupplier_ShouldFail_WhenSupplierNotFound()
    {
        var service = new SuppliersApplicationService(new MockSupplierRepository());

        var result = service.RenameSupplier("S001", "New Name");

        Assert.False(result.IsSuccess);
        Assert.Equal("Supplier not found.", result.Error);
    }

    [Fact]
    public void RenameSupplier_ShouldFail_WhenNewNameInvalid()
    {
        var repo = new MockSupplierRepository();
        repo.Add(new Supplier("S001", "ACME"));
        var service = new SuppliersApplicationService(repo);

        var result = service.RenameSupplier("S001", "");

        Assert.False(result.IsSuccess);
        Assert.Contains("Name is required.", result.Error);
    }

    [Fact]
    public void RenameSupplier_ShouldUpdateName_WhenValid()
    {
        var repo = new MockSupplierRepository();
        repo.Add(new Supplier("S001", "ACME"));
        var service = new SuppliersApplicationService(repo);

        var result = service.RenameSupplier("S001", "ACME SA");

        Assert.True(result.IsSuccess);
        Assert.Equal("ACME SA", repo.FindByCode("S001")!.Name);
    }

    [Fact]
    public void DeleteSupplier_ShouldFail_WhenCodeIsEmpty()
    {
        var service = new SuppliersApplicationService(new MockSupplierRepository());

        var result = service.DeleteSupplier(" ");

        Assert.False(result.IsSuccess);
        Assert.Equal("Code is required.", result.Error);
    }

    [Fact]
    public void DeleteSupplier_ShouldFail_WhenSupplierNotFound()
    {
        var service = new SuppliersApplicationService(new MockSupplierRepository());

        var result = service.DeleteSupplier("S001");

        Assert.False(result.IsSuccess);
        Assert.Equal("Supplier not found.", result.Error);
    }

    [Fact]
    public void DeleteSupplier_ShouldDelete_WhenSupplierExists()
    {
        var repo = new MockSupplierRepository();
        repo.Add(new Supplier("S001", "ACME"));
        var service = new SuppliersApplicationService(repo);

        var result = service.DeleteSupplier("S001");

        Assert.True(result.IsSuccess);
        Assert.Null(repo.FindByCode("S001"));
    }
}
