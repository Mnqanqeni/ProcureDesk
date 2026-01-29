using ProcureDesk.Domain;

namespace ProcureDesk.Infrastructure;

public class MockGoodRepository : IGoodRepository
{
    private readonly List<Good> _goods = new();

    public IEnumerable<Good> List()
        => _goods.ToList();

    public Good? FindByCode(string code)
        => _goods.FirstOrDefault(g => g.Code == code);

    public void Add(Good good)
        => _goods.Add(good);

    public void Update(Good good)
    {
        var existing = FindByCode(good.Code);
        if (existing is not null)
        {
            _goods.Remove(existing);
            _goods.Add(good);
        }
    }

    public void Delete(string code)
    {
        var existing = FindByCode(code);
        if (existing is not null)
            _goods.Remove(existing);
    }
}