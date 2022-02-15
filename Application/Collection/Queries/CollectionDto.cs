namespace Application.Collection.Queries;

public class CollectionDto
{
    public Guid Id { get; set; }
    public string Station { get; set; }
    public string Sense { get; set; }
    public int Hour { get; set; }
    public string Category { get; set; }
    public int Amount { get; set; }
    public double TabulatedValue { get; set; }
    public DateTime Date { get; set; }
}