namespace DPWH.EDMS.Application.Models;
public class SimpleKeyValue
{
    public SimpleKeyValue(string id, string name)
    {
        Id = id;
        Name = name;
    }
    public string Id { get; set; }
    public string Name { get; set; }
}
