using Microsoft.Extensions.Configuration;
using Infrastructure.Context;
using Infrastructure.Seeder.Entities;

namespace Infrastructure.Seeder;

public class Initiation
{
    private readonly PersistenceContext _context;
    public Initiation(PersistenceContext context)
    {
        _context = context;
    }

    public void Initialize(IConfiguration config)
    {
        if (!_context.Collections.Any()) CollectionInitialize.Initialize(_context, config);
    }
}