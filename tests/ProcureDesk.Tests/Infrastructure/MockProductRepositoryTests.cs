using ProcureDesk.Domain;
using ProcureDesk.Infrastructure;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace ProcureDesk.Tests;

public class MockProductRepositoryTests
{
    [Fact]
    public void Add_ShouldStoreProduct_AndGetByCodeShouldReturnIt()
    {
        var repo = new MockProductRepository();
        var (ok, errors, product) = Product.Create("B001", "Bolt", "test");
        Assert.True(ok);

        repo.Add(product!);

        var found = repo.GetByCode("B001");

        Assert.NotNull(found);
        Assert.Equal("Bolt", found!.Name);
    }

    [Fact]
    public void GetByCode_ShouldReturnNull_WhenProductDoesNotExist()
    {
        var repo = new MockProductRepository();

        var found = repo.GetByCode("X999");

        Assert.Null(found);
    }

    [Fact]
    public void List_ShouldReturnAllGoods()
    {
        var repo = new MockProductRepository();
        var (ok1, e1, p1) = Product.Create("B001", "Bolt", "test");
        var (ok2, e2, p2) = Product.Create("N001", "Nut", "test");
        repo.Add(p1!);
        repo.Add(p2!);

        var goods = repo.List().ToList();

        Assert.Equal(2, goods.Count);
    }

    [Fact]
    public void List_ShouldReturnCopy_NotInternalList()
    {
        var repo = new MockProductRepository();
        var (ok, e, p) = Product.Create("B001", "Bolt", "test");
        repo.Add(p!);

        var list = repo.List().ToList();
        list.Clear(); 

        var goods = repo.List();

        Assert.Single(goods);
    }

    [Fact]
    public void Update_ShouldReplaceExistingProduct()
    {
        var repo = new MockProductRepository();
        var (ok, e, original) = Product.Create("B001", "Bolt", "test");
        repo.Add(original!);

        var (ok2, e2, updated) = Product.Create("B001", "Heavy Bolt", "test");
        repo.Update(updated!);

        var found = repo.GetByCode("B001");

        Assert.Equal("Heavy Bolt", found!.Name);
    }

    [Fact]
    public void Update_ShouldDoNothing_WhenGoodDoesNotExist()
    {
        var repo = new MockProductRepository();

        var (ok, e, p) = Product.Create("X001", "Ghost Item", "test");

        Assert.Throws<KeyNotFoundException>(() => repo.Update(p!));
    }

    [Fact]
    public void Delete_ShouldRemoveExistingProduct()
    {
        var repo = new MockProductRepository();
        var (ok, e, p) = Product.Create("B001", "Bolt", "test");
        repo.Add(p!);

        repo.Delete("B001");

        Assert.Null(repo.GetByCode("B001"));
    }

    [Fact]
    public void Delete_ShouldDoNothing_WhenGoodDoesNotExist()
    {
        var repo = new MockProductRepository();

        repo.Delete("X001");

        Assert.Empty(repo.List());
    }
}
