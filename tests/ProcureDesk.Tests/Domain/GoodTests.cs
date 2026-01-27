using System;
using ProcureDesk.Domain;
using Xunit;

namespace ProcureDesk.Tests.Domain;

public class GoodTests
{
    [Fact]
    public void Constructor_ShouldTrimCodeAndName()
    {
        var good = new Good("  SKU001  ", "  Widget A  ");

        Assert.Equal("SKU001", good.Code);
        Assert.Equal("Widget A", good.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrow_WhenCodeIsNullOrWhitespace(string? code)
    {
        var ex = Assert.Throws<ArgumentException>(() => new Good(code!, "Widget"));
        Assert.Equal("code", ex.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrow_WhenNameIsNullOrWhitespace(string? name)
    {
        var ex = Assert.Throws<ArgumentException>(() => new Good("SKU123", name!));
        Assert.Equal("name", ex.ParamName);
    }

    [Fact]
    public void Rename_ShouldUpdateName_AndTrim()
    {
        var good = new Good("SKU123", "Old");

        good.Rename("   New Name   ");

        Assert.Equal("New Name", good.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Rename_ShouldThrow_WhenNewNameIsNullOrWhitespace(string? newName)
    {
        var good = new Good("SKU123", "Widget");

        var ex = Assert.Throws<ArgumentException>(() => good.Rename(newName!));
        Assert.Equal("name", ex.ParamName); // from NormalizeName(nameof(name))
    }

    [Fact]
    public void Validate_ShouldReturnFalse_WhenCodeMissing()
    {
        var (isValid, message) = Good.Validate("", "Widget");

        Assert.False(isValid);
        Assert.Contains("Code is required.", message);
    }

    [Fact]
    public void Validate_ShouldReturnFalse_WhenNameMissing()
    {
        var (isValid, message) = Good.Validate("SKU123", "");

        Assert.False(isValid);
        Assert.Contains("Name is required.", message);
    }

    [Fact]
    public void Validate_ShouldReturnFalse_WithBothErrors_WhenBothMissing()
    {
        var (isValid, message) = Good.Validate("", "");

        Assert.False(isValid);
        Assert.Contains("Code is required.", message);
        Assert.Contains("Name is required.", message);
    }

    [Fact]
    public void Validate_ShouldReturnTrue_WhenInputsAreValid()
    {
        var (isValid, message) = Good.Validate("SKU123", "Widget");

        Assert.True(isValid);
        Assert.Equal(string.Empty, message);
    }
}
