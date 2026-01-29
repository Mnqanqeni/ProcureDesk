using ProcureDesk.Domain;

namespace ProcureDesk.Application;

public class PurchaseOrdersApplicationService
{
    private readonly IPurchaseOrderRepository _poRepo;
    private readonly ISupplierRepository _supplierRepo;
    private readonly IGoodRepository _goodRepo;

    public PurchaseOrdersApplicationService(
        IPurchaseOrderRepository poRepo,
        ISupplierRepository supplierRepo,
        IGoodRepository goodRepo)
    {
        _poRepo = poRepo;
        _supplierRepo = supplierRepo;
        _goodRepo = goodRepo;
    }

    public IEnumerable<PurchaseOrder> ListPurchaseOrders()
        => _poRepo.List();

    public (PurchaseOrder? po, string message) GetPurchaseOrder(string orderNumber)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
            return (null, "Order number is required.");

        var key = orderNumber.Trim();
        var po = _poRepo.FindByOrderNumber(key);

        return po is null
            ? (null, "Purchase order not found.")
            : (po, string.Empty);
    }

    public (bool isValid, string message) CreatePurchaseOrder(string orderNumber, string supplierCode)
    {
        var (isValid, message) = PurchaseOrder.Validate(orderNumber, supplierCode);
        if (!isValid) return (false, message);

        var poNumber = orderNumber.Trim();
        var supCode = supplierCode.Trim();

        if (_poRepo.FindByOrderNumber(poNumber) is not null)
            return (false, "A purchase order with this order number already exists.");

        if (_supplierRepo.FindByCode(supCode) is null)
            return (false, "Supplier not found.");

        _poRepo.Add(new PurchaseOrder(poNumber, supCode));
        return (true, string.Empty);
    }

    public (bool isValid, string message) AddLine(string orderNumber, string goodCode, int quantity, decimal unitPrice)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
            return (false, "Order number is required.");

        var poNumber = orderNumber.Trim();
        var po = _poRepo.FindByOrderNumber(poNumber);
        if (po is null)
            return (false, "Purchase order not found.");

        var (isValid, message) = PurchaseOrderLine.Validate(goodCode, quantity, unitPrice);
        if (!isValid) return (false, message);

        var gCode = goodCode.Trim();
        if (_goodRepo.FindByCode(gCode) is null)
            return (false, "Good not found.");

        var nextLineNumber = po.Lines.Count == 0 ? 1 : po.Lines.Max(l => l.LineNumber) + 1;

        po.Lines.Add(new PurchaseOrderLine(
            lineNumber: nextLineNumber,
            goodCode: gCode,
            quantity: quantity,
            unitPrice: unitPrice
        ));

        _poRepo.Update(po);
        return (true, string.Empty);
    }

    public (bool isValid, string message) RemoveLine(string orderNumber, int lineNumber)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
            return (false, "Order number is required.");

        var poNumber = orderNumber.Trim();
        var po = _poRepo.FindByOrderNumber(poNumber);
        if (po is null)
            return (false, "Purchase order not found.");

        var line = po.Lines.FirstOrDefault(l => l.LineNumber == lineNumber);
        if (line is null)
            return (false, "Purchase order line not found.");

        po.Lines.Remove(line);
        _poRepo.Update(po);
        return (true, string.Empty);
    }

    public (bool isValid, string message) DeletePurchaseOrder(string orderNumber)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
            return (false, "Order number is required.");

        var poNumber = orderNumber.Trim();
        var existing = _poRepo.FindByOrderNumber(poNumber);
        if (existing is null)
            return (false, "Purchase order not found.");

        _poRepo.Delete(poNumber);
        return (true, string.Empty);
    }
}
