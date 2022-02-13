using Domain.Entities.Base;

namespace Domain.Entities;

public class Collection : EntityBase<Guid>
{
    public string Station { get; set; }
    public string Sense { get; set; }
    public int Hour { get; set; }
    public string Category { get; set; }
    public int Amount { get; set; }
    public double TabulatedValue { get; set; }
    public DateTime Date { get; set; }
}
