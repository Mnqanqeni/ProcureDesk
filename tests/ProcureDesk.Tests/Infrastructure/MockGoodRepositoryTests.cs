using ProcureDesk.Domain;
using ProcureDesk.Infrastructure;
using Xunit;

namespace ProcureDesk.Tests;

public class MockGoodRepositoryTests
{
    [Fact]
    public void Add_ShouldStoreGood_AndFindByCodeShouldReturnIt()
    {
        var repo = new MockGoodRepository();
        var good = new Good("B001", "Bolt");

        repo.Add(good);

        var found = repo.FindByCode("B001");

        Assert.NotNull(found);
        Assert.Equal("Bolt", found!.Name);
    }

    [Fact]
    public void FindByCode_ShouldReturnNull_WhenGoodDoesNotExist()
    {
        var repo = new MockGoodRepository();

        var found = repo.FindByCode("X999");

        Assert.Null(found);
    }

    [Fact]
    public void List_ShouldReturnAllGoods()
    {
        var repo = new MockGoodRepository();
        repo.Add(new Good("B001", "Bolt"));
        repo.Add(new Good("N001", "Nut"));

        var goods = repo.List().ToList();

        Assert.Equal(2, goods.Count);
    }

    [Fact]
    public void List_ShouldReturnCopy_NotInternalList()
    {
        var repo = new MockGoodRepository();
        repo.Add(new Good("B001", "Bolt"));

        var list = repo.List().ToList();
        list.Clear(); 

        var goods = repo.List();

        Assert.Single(goods);
    }

    [Fact]
    public void Update_ShouldReplaceExistingGood()
    {
        var repo = new MockGoodRepository();
        var original = new Good("B001", "Bolt");
        repo.Add(original);

        var updated = new Good("B001", "Heavy Bolt");
        repo.Update(updated);

        var found = repo.FindByCode("B001");

        Assert.Equal("Heavy Bolt", found!.Name);
    }

    [Fact]
    public void Update_ShouldDoNothing_WhenGoodDoesNotExist()
    {
        var repo = new MockGoodRepository();

        repo.Update(new Good("X001", "Ghost Item"));

        Assert.Empty(repo.List());
    }

    [Fact]
    public void Delete_ShouldRemoveExistingGood()
    {
        var repo = new MockGoodRepository();
        repo.Add(new Good("B001", "Bolt"));

        repo.Delete("B001");

        Assert.Null(repo.FindByCode("B001"));
    }

    [Fact]
    public void Delete_ShouldDoNothing_WhenGoodDoesNotExist()
    {
        var repo = new MockGoodRepository();

        repo.Delete("X001");

        Assert.Empty(repo.List());
    }
}
