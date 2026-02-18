using ProcureDesk.Domain;
using Xunit;

namespace ProcureDesk.Tests;

public class SupplierTests
{
	[Fact]
	public void Supplier_Validate_Fails_WhenCodeMissing()
	{
		var (isValid, errors) = Supplier.Validate("", "SupplierName");
		Assert.False(isValid);
		Assert.Contains("Code is required.", errors);
	}

	[Fact]
	public void Supplier_Validate_Fails_WhenNameMissing()
	{
		var (isValid, errors) = Supplier.Validate("S001", "");
		Assert.False(isValid);
		Assert.Contains("Name is required.", errors);
	}

	[Fact]
	public void Supplier_Create_Returns_Supplier_WhenValid()
	{
		var (isValid, errors, supplier) = Supplier.Create("S001", "Acme", "tester");
		Assert.True(isValid);
		Assert.Empty(errors);
		Assert.NotNull(supplier);
		Assert.Equal("S001", supplier!.Code);
		Assert.Equal("Acme", supplier.Name);
		Assert.Equal("tester", supplier.CreateUser);
		Assert.Equal("tester", supplier.EditUser);
		Assert.True(supplier.CreatedDate > DateTime.MinValue);
		Assert.True(supplier.EditDate > DateTime.MinValue);
	}
}
