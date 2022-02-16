using Domain.Entities;
using Domain.Ports;

namespace Domain.Services;

[DomainService]
public class CollectionService
{
    readonly IGenericRepository<Collection> _repositoryCollection;

    public CollectionService(IGenericRepository<Collection> repositoryCollection)
    {
        _repositoryCollection = repositoryCollection ?? throw new ArgumentNullException(nameof(repositoryCollection), "No repo available");
    }

    public async Task<List<Collection>> GetCollectionsFilterByDate(DateTime date)
    {
        return (await _repositoryCollection.GetAsync(
            c=> c.Date.Year == date.Year && c.Date.Month == date.Month, x => x.OrderBy(c => c.Station
            ))).ToList();
        
    }

}