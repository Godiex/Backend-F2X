using Domain.Entities;
using Infrastructure.Context;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Seeder.Entities;

public static class CollectionInitialize
{
    private const int NUMBERS_OF_MONTHS = 8;
    private static HttpClientCollectionFactory? httpClientCollection;
    private static List<Collection> collections = new List<Collection>();
    public static async Task Initialize(PersistenceContext persistenceContext, IConfiguration config)
    { 
        httpClientCollection = new HttpClientCollectionFactory(config);
        var token = await httpClientCollection.LoginInApi();
        var a = await FillCollection(token);
        await persistenceContext.Collections.AddRangeAsync(collections);
        await persistenceContext.SaveChangesAsync();
    }
    
    private static async Task<List<Collection>> FillCollection(string token)
    {
        DateTime currentDate = DateTime.Now;
        DateTime initialDate = currentDate.AddMonths(-NUMBERS_OF_MONTHS);
        List<Collection> collectionsOfDay;
        while (initialDate < currentDate)
        {
            var collectionAmount = await httpClientCollection?.GetAmountVehicle(token, initialDate)!;
            var collectionTabulatedValue = await httpClientCollection.GetCollectionVehicle(token, initialDate);
            if (collectionTabulatedValue is {Count: > 0} && collectionAmount is {Count: > 0})
            {
                collectionsOfDay = new List<Collection>();
                collectionsOfDay.AddRange(collectionAmount.Select((t, i) => new Collection
                {
                    Amount = t.Cantidad,
                    Category = t.Categoria,
                    Sense = t.Sentido,
                    Station = t.Estacion,
                    Date = initialDate,
                    Hour = t.Hora,
                    TabulatedValue = collectionTabulatedValue[i].ValorTabulado,
                }));
                collections.AddRange(collectionsOfDay);
            }
            initialDate = initialDate.AddDays(1);
        }
        return collections;
    }
    
}