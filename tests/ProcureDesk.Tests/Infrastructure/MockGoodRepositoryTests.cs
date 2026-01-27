using System;
using System.Linq;
using ProcureDesk.Domain;
using ProcureDesk.Infrastructure;
using Xunit;

namespace ProcureDesk.Tests.Infrastructure;

public class MockGoodsRepositoryTests
{
    [Fact]
    public void List_ShouldBeEmpty_WhenNoGoods()
    {
        var repo = new MockGoodsRepository();

        var goods = repo.List();

        Assert.Empty(goods);
    }

    [Fact]
    public void Add_ShouldStoreGood()
    {
        var repo = new MockGoodsRepository();
        var good = new Good("SKU001", "Widget A");

        repo.Add(good);

        var list = repo.List().ToList();
        Assert.Single(list);
        Assert.Equal("SKU001", list[0].Code);
        Assert.Equal("Widget A", list[0].Name);
    }

    [Fact]
    public void Add_ShouldThrow_WhenDuplicateCode()
    {
        var repo = new MockGoodsRepository();
        repo.Add(new Good("SKU123", "Widget"));

        var ex = Assert.Throws<InvalidOperationException>(() =>
            repo.Add(new Good("SKU123", "Another Widget")));

        Assert.Contains("already exists", ex.Message);
    }

    [Fact]
    public void FindByCode_ShouldReturnNull_WhenNotFound()
    {
        var repo = new MockGoodsRepository();

        var found = repo.FindByCode("MISSING");

        Assert.Null(found);
    }

    [Fact]
    public void FindByCode_ShouldReturnGood_WhenFound()
    {
        var repo = new MockGoodsRepository();
        repo.Add(new Good("SKU123", "Widget"));

        var found = repo.FindByCode("SKU123");

        Assert.NotNull(found);
        Assert.Equal("SKU123", found!.Code);
        Assert.Equal("Widget", found.Name);
    }

    [Fact]
    public void Update_ShouldReplaceExistingGood()
    {
        var repo = new MockGoodsRepository();
        repo.Add(new Good("SKU123", "Old"));

        repo.Update(new Good("SKU123", "New"));

        var updated = repo.FindByCode("SKU123");
        Assert.NotNull(updated);
        Assert.Equal("New", updated!.Name);
    }

    [Fact]
    public void Update_ShouldThrow_WhenGoodNotFound()
    {
        var repo = new MockGoodsRepository();

        var ex = Assert.Throws<KeyNotFoundException>(() =>
            repo.Update(new Good("SKU404", "Missing")));

        Assert.Contains("not found", ex.Message);
    }

    [Fact]
    public void Delete_ShouldRemoveGood_WhenExists()
    {
        var repo = new MockGoodsRepository();
        repo.Add(new Good("SKU123", "Widget"));

        repo.Delete("SKU123");

        Assert.Empty(repo.List());
        Assert.Null(repo.FindByCode("SKU123"));
    }

    [Fact]
    public void Delete_ShouldThrow_WhenGoodNotFound()
    {
        var repo = new MockGoodsRepository();

        var ex = Assert.Throws<KeyNotFoundException>(() => repo.Delete("SKU404"));
        Assert.Contains("not found", ex.Message);
    }
}
