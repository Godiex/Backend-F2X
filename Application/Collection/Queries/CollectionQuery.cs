using MediatR;

namespace Application.Collection.Queries;

public record CollectionQuery() : IRequest<List<CollectionDto>>;