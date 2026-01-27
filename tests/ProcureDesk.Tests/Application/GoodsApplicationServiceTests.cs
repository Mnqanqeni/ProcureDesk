using ProcureDesk.Application;
using ProcureDesk.Domain;
using ProcureDesk.Infrastructure;
using Xunit;

namespace ProcureDesk.Tests;

public class GoodsApplicationServiceTests
{
    [Fact]
    public void ListGoods_ShouldReturnAllGoods()
    {
        var repo = new MockGoodRepository();
        repo.Add(new Good("B001", "Bolt"));
        repo.Add(new Good("N001", "Nut"));

        var service = new GoodsApplicationService(repo);

        var result = service.ListGoods();

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value!.Count());
    }

    [Fact]
    public void GetGoodByCode_ShouldFail_WhenCodeIsEmpty()
    {
        var service = new GoodsApplicationService(new MockGoodRepository());

        var result = service.GetGoodByCode("  ");

        Assert.False(result.IsSuccess);
        Assert.Equal("Code is required.", result.Error);
    }

    [Fact]
    public void GetGoodByCode_ShouldFail_WhenGoodNotFound()
    {
        var service = new GoodsApplicationService(new MockGoodRepository());

        var result = service.GetGoodByCode("B001");

        Assert.False(result.IsSuccess);
        Assert.Equal("Good not found.", result.Error);
    }

    [Fact]
    public void GetGoodByCode_ShouldReturnGood_WhenFound()
    {
        var repo = new MockGoodRepository();
        repo.Add(new Good("B001", "Bolt"));
        var service = new GoodsApplicationService(repo);

        var result = service.GetGoodByCode("B001");

        Assert.True(result.IsSuccess);
        Assert.Equal("B001", result.Value!.Code);
        Assert.Equal("Bolt", result.Value!.Name);
    }

    [Fact]
    public void CreateGood_ShouldFail_WhenValidationFails()
    {
        var service = new GoodsApplicationService(new MockGoodRepository());

        var result = service.CreateGood("", "");

        Assert.False(result.IsSuccess);
        Assert.Contains("Code is required.", result.Error);
        Assert.Contains("Name is required.", result.Error);
    }

    [Fact]
    public void CreateGood_ShouldFail_WhenCodeAlreadyExists()
    {
        var repo = new MockGoodRepository();
        repo.Add(new Good("B001", "Bolt"));
        var service = new GoodsApplicationService(repo);

        var result = service.CreateGood("B001", "Another Bolt");

        Assert.False(result.IsSuccess);
        Assert.Equal("A good with this code already exists.", result.Error);
    }

    [Fact]
    public void CreateGood_ShouldAddGood_WhenValid()
    {
        var repo = new MockGoodRepository();
        var service = new GoodsApplicationService(repo);

        var result = service.CreateGood("B001", "Bolt");

        Assert.True(result.IsSuccess);
        Assert.NotNull(repo.FindByCode("B001"));
    }

    [Fact]
    public void RenameGood_ShouldFail_WhenCodeIsEmpty()
    {
        var service = new GoodsApplicationService(new MockGoodRepository());

        var result = service.RenameGood(" ", "New Name");

        Assert.False(result.IsSuccess);
        Assert.Equal("Code is required.", result.Error);
    }

    [Fact]
    public void RenameGood_ShouldFail_WhenGoodNotFound()
    {
        var service = new GoodsApplicationService(new MockGoodRepository());

        var result = service.RenameGood("B001", "New Name");

        Assert.False(result.IsSuccess);
        Assert.Equal("Good not found.", result.Error);
    }

    [Fact]
    public void RenameGood_ShouldFail_WhenNewNameInvalid()
    {
        var repo = new MockGoodRepository();
        repo.Add(new Good("B001", "Bolt"));
        var service = new GoodsApplicationService(repo);

        var result = service.RenameGood("B001", "");

        Assert.False(result.IsSuccess);
        Assert.Contains("Name is required.", result.Error);
    }

    [Fact]
    public void RenameGood_ShouldUpdateName_WhenValid()
    {
        var repo = new MockGoodRepository();
        repo.Add(new Good("B001", "Bolt"));
        var service = new GoodsApplicationService(repo);

        var result = service.RenameGood("B001", "Heavy Bolt");

        Assert.True(result.IsSuccess);
        Assert.Equal("Heavy Bolt", repo.FindByCode("B001")!.Name);
    }

    [Fact]
    public void DeleteGood_ShouldFail_WhenCodeIsEmpty()
    {
        var service = new GoodsApplicationService(new MockGoodRepository());

        var result = service.DeleteGood(" ");

        Assert.False(result.IsSuccess);
        Assert.Equal("Code is required.", result.Error);
    }

    [Fact]
    public void DeleteGood_ShouldFail_WhenGoodNotFound()
    {
        var service = new GoodsApplicationService(new MockGoodRepository());

        var result = service.DeleteGood("B001");

        Assert.False(result.IsSuccess);
        Assert.Equal("Good not found.", result.Error);
    }

    [Fact]
    public void DeleteGood_ShouldDelete_WhenGoodExists()
    {
        var repo = new MockGoodRepository();
        repo.Add(new Good("B001", "Bolt"));
        var service = new GoodsApplicationService(repo);

        var result = service.DeleteGood("B001");

        Assert.True(result.IsSuccess);
        Assert.Null(repo.FindByCode("B001"));
    }
}
