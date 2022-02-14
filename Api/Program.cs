using System.Reflection;
using Api.Filters;
using Infrastructure.Context;
using Infrastructure.Extensions;
using Infrastructure.Seeder;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddControllers(opts =>
{
    opts.Filters.Add(typeof(AppExceptionFilterAttribute));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(Assembly.Load("Application"), typeof(Program).Assembly);
builder.Services.AddAutoMapper(Assembly.Load("Application"));

builder.Services.AddDbContext<PersistenceContext>(opt =>
{
    opt.UseSqlServer(config.GetConnectionString("database"), sqlopts =>
    {
        sqlopts.MigrationsHistoryTable("_MigrationHistory", config.GetValue<string>("SchemaName"));
    });
});

builder.Services.AddHealthChecks().AddSqlServer(config["ConnectionStrings:database"]);

builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Counting and Collection Api", Version = "v1" });
});

Log.Logger = new LoggerConfiguration().Enrich.FromLogContext()    
    .WriteTo.Console().CreateLogger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Counting and Collection Api"));
}

app.UseRouting().UseHttpMetrics().UseEndpoints(endpoints =>
{
    endpoints.MapGet("/Test/Author", () => new { version = 1.0, by = "Diego A. Villa" });
    endpoints.MapMetrics();
    endpoints.MapHealthChecks("/health");
});

using var scope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope();
var context = scope?.ServiceProvider.GetRequiredService<PersistenceContext>();
var start = new Initiation(context);
start.Initialize(config);

app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
