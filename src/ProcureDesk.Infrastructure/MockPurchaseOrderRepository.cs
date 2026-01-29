using ProcureDesk.Domain;

namespace ProcureDesk.Infrastructure;

public class MockPurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly List<PurchaseOrder> _purchaseOrders = new();

    public IEnumerable<PurchaseOrder> List()
        => _purchaseOrders.ToList();

    public PurchaseOrder? FindByOrderNumber(string orderNumber)
        => _purchaseOrders.FirstOrDefault(p => p.OrderNumber == orderNumber);

    public void Add(PurchaseOrder po)
        => _purchaseOrders.Add(po);

    public void Update(PurchaseOrder po)
    {
        var existing = FindByOrderNumber(po.OrderNumber);
        if (existing is not null)
        {
            _purchaseOrders.Remove(existing);
            _purchaseOrders.Add(po);
        }
    }

    public void Delete(string orderNumber)
    {
        var existing = FindByOrderNumber(orderNumber);
        if (existing is not null)
            _purchaseOrders.Remove(existing);
    }
}
