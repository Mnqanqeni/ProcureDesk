namespace ProcureDesk.Domain;

public class Product
{
    public  string Code { get; init; }= null!;
    public string Name { get; private set; }= null!;

    public DateTime CreatedDate { get; private set; }
    public DateTime EditDate { get; private set; }
    public string CreateUser { get; private set; }= null!;
    public string EditUser { get; private set; }=   null!;

    private Product() { }

    public static (bool isValid, List<string> errors, Product? product)
        Create(string code, string name, string user)
    {

        var (isValid, errors) = Validate(code, name, user);
        if (!isValid) return (false, errors, null);

        var now = DateTime.UtcNow;

        var product = new Product
        {
            Code = code,
            Name = name,
            CreateUser = user,
            EditUser = user,
            CreatedDate = now,
            EditDate = now
        };

        return (true, new List<string>(), product);
    }

    public (bool isValid, List<string> errors)
        Rename(string newName, string user)
    {

        var (isValid, errors) = Validate(newName, user);
        if (!isValid) return (false, errors);

        Name = newName;
        Touch(user);

        return (true, new List<string>());
    }


    private void Touch(string user)
    {
        EditUser = user;
        EditDate = DateTime.UtcNow;
    }

    private static (bool isValid, List<string> errors)
        Validate(string code, string name, string user)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(code))
            errors.Add("Code is required.");

        if (string.IsNullOrWhiteSpace(name))
            errors.Add("Name is required.");

        if (string.IsNullOrWhiteSpace(user))
            errors.Add("User is required.");

        return (errors.Count == 0, errors);
    }

    private static (bool isValid, List<string> errors)
        Validate(string name, string user)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(name))
            errors.Add("Name is required.");

        if (string.IsNullOrWhiteSpace(user))
            errors.Add("User is required.");

        return (errors.Count == 0, errors);
    }
}