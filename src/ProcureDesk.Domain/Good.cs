namespace ProcureDesk.Domain;

public class Good
{
    public string Code { get; init; }
    public string Name { get; private set; }

    public Good(string code, string name)
    {
        Code = NormalizeCode(code);
        Name = NormalizeName(name);
    }

    public void Rename(string newName)
    {
        Name = NormalizeName(newName);
    }

    private static string NormalizeCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be null or empty.", nameof(code));

        return code.Trim();
    }

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        return name.Trim();
    }

    public static (bool isValid, string message) Validate(string code, string name)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(code))
            errors.Add("Code is required.");

        if (string.IsNullOrWhiteSpace(name))
            errors.Add("Name is required.");

        return errors.Count == 0
            ? (true, string.Empty)
            : (false, string.Join(" ", errors));
    }
}
