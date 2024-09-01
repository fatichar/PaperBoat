namespace PaperBoat.Model.Extracted;

public class Field(string name)
{
    public string Name { get; set; } = name;
    public string Id { get; set; }
    public string ValueType { get; set; }

    public Value Value { get; set; }
    public Snippet Snippet { get; set; }
}