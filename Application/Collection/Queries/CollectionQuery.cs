using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Collection.Queries;

    public record CollectionQuery(
        [Required] int pageNumber,
        [Required] int pageSize
    ) : IRequest<List<CollectionDto>>;