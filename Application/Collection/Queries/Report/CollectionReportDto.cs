namespace Application.Collection.Queries.Report;

public class CollectionReportDto
{
    public string Station { get; set; }
    
}

public class CollectionDetailDto
{
    public int AmountTotal { get; set; }
    public double TotalTabulatedValue { get; set; }
    public DateTime Date { get; set; }

}