using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Api
{
    public class PersistenceContextFactory : IDesignTimeDbContextFactory<PersistenceContext>
    {
        public PersistenceContext CreateDbContext(string[] args)
        {
             var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
                
            var optionsBuilder = new DbContextOptionsBuilder<PersistenceContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("database"), sqlopts =>
            {
                sqlopts.MigrationsHistoryTable("_MigrationHistory", config.GetValue<string>("SchemaName"));
            });

            return new PersistenceContext(optionsBuilder.Options, config);
        }
    }
}