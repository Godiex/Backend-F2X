using Application.Collection.Queries;
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
}