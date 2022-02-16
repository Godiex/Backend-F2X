using Application.Collection.Queries;
using Application.Collection.Queries.Report;
using Application.Collection.Queries.SimpleQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CollectionController
{

    readonly IMediator _mediator = default!;

    public CollectionController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<List<CollectionDto>> Get([FromQuery] int pageNumber, [FromQuery] int pageSize) => 
        await _mediator.Send(new CollectionQuery(pageNumber, pageSize));
    
    [HttpGet("report/{date:datetime}")]
    public async Task<List<List<CollectionReportDto>>> GetReport(DateTime date) => 
        await _mediator.Send(new CollectionReportQuery(date));
}