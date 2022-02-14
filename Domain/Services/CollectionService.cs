using Domain.Entities;
using Domain.Ports;

namespace Domain.Services;

[DomainService]
public class CollectionService
{
    readonly IGenericRepository<Collection> _repositoryCollection;
    
    
}