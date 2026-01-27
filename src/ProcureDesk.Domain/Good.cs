namespace ProcureDesk.Domain;

public class Good
{
    public string Code;
    public string Name;

    public Good(string code, string name)
    {
        Code = code;
        Name = name;
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
