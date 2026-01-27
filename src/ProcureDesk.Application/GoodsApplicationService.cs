using ProcureDesk.Domain;

namespace ProcureDesk.Application;

public class GoodsApplicationService
{
    private readonly IGoodRepository _repo;

    public GoodsApplicationService(IGoodRepository repo)
    {
        _repo = repo;
    }

    public Result<IEnumerable<Good>> ListGoods()
        => Result<IEnumerable<Good>>.Ok(_repo.List());

    public Result<Good> GetGoodByCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result<Good>.Fail("Code is required.");

        var trimmedCode = code.Trim();

        var good = _repo.FindByCode(trimmedCode);
        return good is null
            ? Result<Good>.Fail("Good not found.")
            : Result<Good>.Ok(good);
    }

    public Result CreateGood(string code, string name)
    {
        var (isValid, message) = Good.Validate(code, name);
        if (!isValid)
            return Result.Fail(message);

        var trimmedCode = code.Trim();

        if (_repo.FindByCode(trimmedCode) is not null)
            return Result.Fail("A good with this code already exists.");

        // You said normalization is outside entity rules; do it here if you want clean data:
        var trimmedName = name.Trim();

        _repo.Add(new Good(trimmedCode, trimmedName));
        return Result.Ok();
    }

    public Result RenameGood(string code, string newName)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result.Fail("Code is required.");

        // Validate update input (your Good.Validate only checks required fields)
        // We reuse it by passing the existing code + new name.
        var (isValid, message) = Good.Validate(code, newName);
        if (!isValid)
            return Result.Fail(message);

        var trimmedCode = code.Trim();

        var good = _repo.FindByCode(trimmedCode);
        if (good is null)
            return Result.Fail("Good not found.");

        // No domain method, no exceptions, just assignment (normalize here if desired)
        good.Name = newName.Trim();

        _repo.Update(good);
        return Result.Ok();
    }

    public Result DeleteGood(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result.Fail("Code is required.");

        var trimmedCode = code.Trim();

        // Don’t rely on exceptions; check first
        var good = _repo.FindByCode(trimmedCode);
        if (good is null)
            return Result.Fail("Good not found.");

        _repo.Delete(trimmedCode);
        return Result.Ok();
    }
}
