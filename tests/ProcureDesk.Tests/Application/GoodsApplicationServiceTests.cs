using ProcureDesk.Application;
using ProcureDesk.Domain;
using ProcureDesk.Infrastructure;
using Xunit;

namespace ProcureDesk.Tests.Application;

public class GoodsApplicationServiceTests
{
    private readonly MockGoodsRepository _repo;
    private readonly GoodsApplicationService _service;

    public GoodsApplicationServiceTests()
    {
        _repo = new MockGoodsRepository();
        _service = new GoodsApplicationService(_repo);
    }

    [Fact]
    public void CreateGood_ShouldSucceed_WhenValidAndUnique()
    {
        var result = _service.CreateGood("SKU123", "Widget");

        Assert.True(result.IsSuccess);
        Assert.Equal(string.Empty, result.Error);

        var stored = _repo.FindByCode("SKU123");
        Assert.NotNull(stored);
        Assert.Equal("Widget", stored!.Name);
    }

    [Fact]
    public void CreateGood_ShouldFail_WhenDuplicateCode()
    {
        _service.CreateGood("SKU123", "Widget");

        var result = _service.CreateGood("SKU123", "Another Widget");

        Assert.False(result.IsSuccess);
        Assert.Equal("A good with this code already exists.", result.Error);
    }

    [Fact]
    public void CreateGood_ShouldFail_WhenInvalidInput()
    {
        var result = _service.CreateGood("", "Widget");

        Assert.False(result.IsSuccess);
        Assert.Contains("Code is required.", result.Error);
    }

    [Fact]
    public void RenameGood_ShouldSucceed_WhenGoodExists_AndNewNameValid()
    {
        _service.CreateGood("SKU123", "Old Name");

        var result = _service.RenameGood("SKU123", "New Name");

        Assert.True(result.IsSuccess);
        Assert.Equal(string.Empty, result.Error);

        var updated = _repo.FindByCode("SKU123");
        Assert.NotNull(updated);
        Assert.Equal("New Name", updated!.Name);
    }

    [Fact]
    public void RenameGood_ShouldFail_WhenGoodNotFound()
    {
        var result = _service.RenameGood("SKU999", "New Name");

        Assert.False(result.IsSuccess);
        Assert.Equal("Good not found.", result.Error);
    }

    [Fact]
    public void RenameGood_ShouldFail_WhenNewNameInvalid()
    {
        _service.CreateGood("SKU123", "Old Name");

        var result = _service.RenameGood("SKU123", "");

        Assert.False(result.IsSuccess);
        // Domain throws: "Name cannot be null or empty."
        Assert.Contains("Name cannot be null or empty.", result.Error);
    }

    [Fact]
    public void DeleteGood_ShouldSucceed_WhenGoodExists()
    {
        _service.CreateGood("SKU123", "Widget");

        var result = _service.DeleteGood("SKU123");

        Assert.True(result.IsSuccess);
        Assert.Equal(string.Empty, result.Error);
        Assert.Null(_repo.FindByCode("SKU123"));
    }

    [Fact]
    public void DeleteGood_ShouldFail_WhenGoodNotFound()
    {
        var result = _service.DeleteGood("SKU404");

        Assert.False(result.IsSuccess);
        Assert.Equal("Good not found.", result.Error);
    }
}
