using System.Data;
using Dapper;
using MediatR;

namespace Application.Collection.Queries;

public class CollectionQueryHandler: IRequestHandler<CollectionQuery, List<CollectionDto>>
{
    private readonly IDbConnection _dapperSource;
    
    public CollectionQueryHandler(IDbConnection dapperSource)
    {
        _dapperSource = dapperSource;
    }

    public async Task<List<CollectionDto>> Handle(CollectionQuery request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request), "request object needed to handle this task");
        return (await _dapperSource.QueryAsync<CollectionDto>(
            String.Concat(
            "SELECT * ",
            "FROM diego.Recaudos ",
            "ORDER BY Date DESC ",
            $"OFFSET {(request.pageNumber - 1) * request.pageSize} ROWS FETCH NEXT {request.pageSize} ROWS ONLY "
        ))).ToList();
    }
    
    
}