using ProcureDesk.Domain;
using ProcureDesk.Infrastructure;
using System.Linq;
using Xunit;

namespace ProcureDesk.Tests.Infrastructure;

public class MockPurchaseOrderRepositoryTests
{
    [Fact]
    public void Add_ShouldStorePurchaseOrder()
    {
        var repo = new MockPurchaseOrderRepository();
        var (ok, errs, product) = Product.Create("P001", "Widget", "t");
        var (ok2, errs2, po) = PurchaseOrder.Create("PO001", "S001", product!, 5, 100m, "tester");

        repo.Add(po!);

        var found = repo.FindByOrderNumber("PO001");
        Assert.NotNull(found);
        Assert.Equal("PO001", found!.OrderNumber);
    }

    [Fact]
    public void FindByOrderNumber_ShouldReturnNull_WhenNotFound()
    {
        var repo = new MockPurchaseOrderRepository();

        var found = repo.FindByOrderNumber("PO999");

        Assert.Null(found);
    }

    [Fact]
    public void List_ShouldReturnAllOrders()
    {
        var repo = new MockPurchaseOrderRepository();
        var (ok, e, product) = Product.Create("P001", "Widget", "t");
        
        var (ok1, errs1, po1) = PurchaseOrder.Create("PO001", "S001", product!, 5, 100m, "tester");
        var (ok2, errs2, po2) = PurchaseOrder.Create("PO002", "S002", product!, 3, 50m, "tester");
        
        repo.Add(po1!);
        repo.Add(po2!);

        var orders = repo.List().ToList();

        Assert.Equal(2, orders.Count);
    }

    [Fact]
    public void Update_ShouldReplaceOrder()
    {
        var repo = new MockPurchaseOrderRepository();
        var (ok, e, product) = Product.Create("P001", "Widget", "t");
        var (ok2, errs, po) = PurchaseOrder.Create("PO001", "S001", product!, 5, 100m, "tester");
        repo.Add(po!);

        var (okSub, errsSub) = po!.Submit("updater");
        Assert.True(okSub);
        repo.Update(po);

        var found = repo.FindByOrderNumber("PO001");
        Assert.Equal(PurchaseOrderStatus.Submitted, found!.Status);
    }

    [Fact]
    public void Delete_ShouldRemoveOrder()
    {
        var repo = new MockPurchaseOrderRepository();
        var (ok, e, product) = Product.Create("P001", "Widget", "t");
        var (ok2, errs, po) = PurchaseOrder.Create("PO001", "S001", product!, 5, 100m, "tester");
        repo.Add(po!);

        repo.Delete("PO001");

        Assert.Null(repo.FindByOrderNumber("PO001"));
    }
}
