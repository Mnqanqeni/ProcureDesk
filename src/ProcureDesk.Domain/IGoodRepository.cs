namespace ProcureDesk.Domain;

public interface IGoodRepository
{
    IEnumerable<Good> List();
    Good? FindByCode(string code);
    void Add(Good good);
    void Update(Good good);
    void Delete(string code);
}
