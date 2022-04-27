using Ocelot.Middleware;
using Ocelot.DependencyInjection;
using ApiGateway.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile($"ocelot.{env}.json").Build();
builder.Services.AddControllers();
builder.Services.AddOcelot(configuration).AddSingletonDefinedAggregator<Aggregator>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();
app.MapControllers();
app.UseOcelot().Wait();

app.Run();
