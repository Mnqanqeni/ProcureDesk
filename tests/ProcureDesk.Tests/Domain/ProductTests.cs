using ProcureDesk.Domain;
using Xunit;

namespace ProcureDesk.Tests;

public class ProductTests
{
	[Fact]
	public void Product_Validate_Fails_WhenCodeMissing()
	{
		var (isValid, errors) = Product.Validate("", "Widget");
		Assert.False(isValid);
		Assert.Contains("Code is required.", errors);
	}

	[Fact]
	public void Product_Validate_Fails_WhenNameMissing()
	{
		var (isValid, errors) = Product.Validate("P001", "");
		Assert.False(isValid);
		Assert.Contains("Name is required.", errors);
	}

	[Fact]
	public void Product_Create_Returns_Product_WhenValid()
	{
		var (isValid, errors, product) = Product.Create("P001", "Widget", "tester");
		Assert.True(isValid);
		Assert.Empty(errors);
		Assert.NotNull(product);
		Assert.Equal("P001", product!.Code);
		Assert.Equal("Widget", product.Name);
		Assert.Equal("tester", product.CreateUser);
		Assert.Equal("tester", product.EditUser);
		Assert.True(product.CreatedDate > DateTime.MinValue);
		Assert.True(product.EditDate > DateTime.MinValue);
	}
}
