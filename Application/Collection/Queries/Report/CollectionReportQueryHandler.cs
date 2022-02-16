using System.Data;
using Application.Collection.Queries.SimpleQuery;
using Dapper;
using Domain.Services;
using MediatR;

namespace Application.Collection.Queries.Report;

public class CollectionReportQueryHandler: IRequestHandler<CollectionReportQuery, List<List<CollectionReportDto>>>
{
    private readonly CollectionService _collectionService;
    
    public CollectionReportQueryHandler(CollectionService collectionService)
    {
        _collectionService = collectionService;
    }

    public async Task<List<List<CollectionReportDto>>> Handle(CollectionReportQuery request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request), "request object needed to handle this task");
        List<List<CollectionReportDto>> collectionsReport = new List<List<CollectionReportDto>>();
        var collectionsFilterByDate = await _collectionService.GetCollectionsFilterByDate(request.Date);
        var collectionsGroupByForStation = collectionsFilterByDate.GroupBy(c => c.Station).ToList();

        foreach (var groupStations in collectionsGroupByForStation)
        {
            List<CollectionReportDto> collections = new List<CollectionReportDto>();
            var collectionReport = new CollectionReportDto();
            foreach (var collection in groupStations)
            {
                collectionsReport.Add(collections);
            }
        }

        return collectionsReport;

    }
    
    
}