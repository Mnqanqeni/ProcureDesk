namespace ProcureDesk.Domain;

public interface IPurchaseOrderRepository
{
    IEnumerable<PurchaseOrder> List();
    PurchaseOrder? FindByOrderNumber(string orderNumber);
    void Add(PurchaseOrder po);
    void Update(PurchaseOrder po);
    void Delete(string orderNumber);
}
