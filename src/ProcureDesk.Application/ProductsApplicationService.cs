using ProcureDesk.Domain;

namespace ProcureDesk.Application;

public class GoodsApplicationService
{
    private readonly IGoodRepository _repo;

    public GoodsApplicationService(IGoodRepository repo)
    {
        _repo = repo;
    }

    public (bool IsSuccess, IEnumerable<Good>? Value, string Error) ListGoods()
        => (true, _repo.List(), string.Empty);

    public (bool IsSuccess, Good? Value, string Error) GetGoodByCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return (false, default(Good), "Code is required.");

        var trimmedCode = code.Trim();

        var good = _repo.FindByCode(trimmedCode);
        return good is null
            ? (false, default(Good), "Good not found.")
            : (true, good, string.Empty);
    }

    public (bool IsSuccess, string Error) CreateGood(string code, string name)
    {
        var (isValid, message) = Good.Validate(code, name);
        if (!isValid)
            return (false, message);

        var trimmedCode = code.Trim();

        if (_repo.FindByCode(trimmedCode) is not null)
            return (false, "A good with this code already exists.");

        // You said normalization is outside entity rules; do it here if you want clean data:
        var trimmedName = name.Trim();

        _repo.Add(new Good(trimmedCode, trimmedName));
        return (true, string.Empty);
    }

    public (bool IsSuccess, string Error) RenameGood(string code, string newName)
    {
        if (string.IsNullOrWhiteSpace(code))
            return (false, "Code is required.");

        // Validate update input (your Good.Validate only checks required fields)
        // We reuse it by passing the existing code + new name.
        var (isValid, message) = Good.Validate(code, newName);
        if (!isValid)
            return (false, message);

        var trimmedCode = code.Trim();

        var good = _repo.FindByCode(trimmedCode);
        if (good is null)
            return (false, "Good not found.");

        // No domain method, no exceptions, just assignment (normalize here if desired)
        good.Name = newName.Trim();

        _repo.Update(good);
        return (true, string.Empty);
    }

    public (bool IsSuccess, string Error) DeleteGood(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return (false, "Code is required.");

        var trimmedCode = code.Trim();

        // Don’t rely on exceptions; check first
        var good = _repo.FindByCode(trimmedCode);
        if (good is null)
            return (false, "Good not found.");

        _repo.Delete(trimmedCode);
        return (true, string.Empty);
    }
}
