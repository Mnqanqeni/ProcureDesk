using ProcureDesk.Domain;

namespace ProcureDesk.Infrastructure;

public class MockGoodsRepository : IGoodRepository
{
    private readonly List<Good> _goods = new();

    public IEnumerable<Good> List() => _goods.AsReadOnly();

    public Good? FindByCode(string code)
        => _goods.FirstOrDefault(g => g.Code == code);

    public void Add(Good good)
    {
        if (FindByCode(good.Code) != null)
            throw new InvalidOperationException($"Good with code '{good.Code}' already exists.");

        _goods.Add(good);
    }

    public void Update(Good good)
    {
        var existing = FindByCode(good.Code);
        if (existing is null)
            throw new KeyNotFoundException($"Good with code '{good.Code}' not found.");

        var index = _goods.IndexOf(existing);
        _goods[index] = good;
    }

    public void Delete(string code)
    {
        var good = FindByCode(code);
        if (good is null)
            throw new KeyNotFoundException($"Good with code '{code}' not found.");

        _goods.Remove(good);
    }
}
