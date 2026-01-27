using ProcureDesk.Domain;
using Xunit;

namespace ProcureDesk.Domain.Tests;

public class GoodTests
{
    [Fact]
    public void Validate_ShouldFail_WhenCodeIsNull()
    {
        var (isValid, message) = Good.Validate(null!, "Bolt");

        Assert.False(isValid);
        Assert.Contains("Code is required.", message);
    }

    [Fact]
    public void Validate_ShouldFail_WhenCodeIsEmpty()
    {
        var (isValid, message) = Good.Validate("", "Bolt");

        Assert.False(isValid);
        Assert.Contains("Code is required.", message);
    }

    [Fact]
    public void Validate_ShouldFail_WhenNameIsNull()
    {
        var (isValid, message) = Good.Validate("B001", null!);

        Assert.False(isValid);
        Assert.Contains("Name is required.", message);
    }

    [Fact]
    public void Validate_ShouldFail_WhenNameIsEmpty()
    {
        var (isValid, message) = Good.Validate("B001", "");

        Assert.False(isValid);
        Assert.Contains("Name is required.", message);
    }

    [Fact]
    public void Validate_ShouldFail_WhenBothCodeAndNameAreInvalid()
    {
        var (isValid, message) = Good.Validate("", "");

        Assert.False(isValid);
        Assert.Contains("Code is required.", message);
        Assert.Contains("Name is required.", message);
    }

    [Fact]
    public void Validate_ShouldPass_WhenCodeAndNameAreValid()
    {
        var (isValid, message) = Good.Validate("B001", "Bolt");

        Assert.True(isValid);
        Assert.Equal(string.Empty, message);
    }

    [Fact]
    public void Validate_ShouldPass_WhenCodeAndNameHaveWhitespace()
    {
        var (isValid, message) = Good.Validate("  B001  ", "  Bolt  ");

        Assert.True(isValid);
        Assert.Equal(string.Empty, message);
    }
}
