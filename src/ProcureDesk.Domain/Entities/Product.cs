namespace ProcureDesk.Domain;

public class Product
{
    public required string Code { get; init; } 
    public required string Name { get; set; } 

    public required DateTime CreatedDate { get; set; }
    public required DateTime EditDate { get; set; }
    public required string CreateUser { get; set; }
    public required string EditUser { get; set; }

    private Product() {

     }

    public static (bool isValid, List<string> errors, Product? product) Create(string code, string name, string user)
    {
        var (isValid, errors) = Validate(code, name);
        if (!isValid)
            return (false, errors, null);


        return (true, errors, new Product
        {
            Code = code,
            Name = name,
            CreatedDate = DateTime.UtcNow,
            EditDate = DateTime.UtcNow,
            CreateUser = user,
            EditUser = user
        })   ;
    }

    public static (bool isValid, List<string> errors) Validate(string code, string name)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(code))
            errors.Add("Code is required.");

        if (string.IsNullOrWhiteSpace(name))
            errors.Add("Name is required.");

        return (errors.Count == 0, errors);
    }
}
