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

        var good = _repo.FindByCode(code.Trim());
        return good is null
            ? Result<Good>.Fail("Good not found.")
            : Result<Good>.Ok(good);
    }

    public Result CreateGood(string code, string name)
    {
        var (isValid, message) = Good.Validate(code, name);
        if (!isValid) return Result.Fail(message);

        code = code.Trim();

        if (_repo.FindByCode(code) is not null)
            return Result.Fail("A good with this code already exists.");

        _repo.Add(new Good(code, name));
        return Result.Ok();
    }

    public Result RenameGood(string code, string newName)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result.Fail("Code is required.");

        var good = _repo.FindByCode(code.Trim());
        if (good is null)
            return Result.Fail("Good not found.");

        // Domain behavior enforces rules and trims
        try
        {
            good.Rename(newName);
        }
        catch (ArgumentException ex)
        {
            // Convert domain exception into a friendly app error
            return Result.Fail(ex.Message);
        }

        _repo.Update(good);
        return Result.Ok();
    }

    public Result DeleteGood(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result.Fail("Code is required.");

        try
        {
            _repo.Delete(code.Trim());
            return Result.Ok();
        }
        catch (KeyNotFoundException)
        {
            return Result.Fail("Good not found.");
        }
    }
}
