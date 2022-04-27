using Authentication.BusinessLogic.Repositories;
using Authentication.BusinessLogic.Services.Interfaces;
using Authentication.Infrastructure.Repositories;
using Authentication.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Authentication.BusinessLogic.Services.Implementations;
using Shared.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOptions();

var serviceClientSettingsConfig = builder.Configuration.GetSection("RabbitMq");
var serviceClientSettings = serviceClientSettingsConfig.Get<RabbitMqOptions>();
builder.Services.Configure<RabbitMqOptions>(serviceClientSettingsConfig);


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Auth service",
        Version = "v1"
    });
});
builder.Services.AddDbContext<AuthrnticationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAuthenticationRepository>(x => new AuthenticationRepository(x.GetService<AuthrnticationContext>()));
builder.Services.AddScoped<IAuthenticationService>(x => new AuthenticationService(x.GetService<IAuthenticationRepository>()));

builder.Services.AddControllers();

if (serviceClientSettings.Enabled)
{
    builder.Services.AddHostedService<Receiver>();
    builder.Services.AddSingleton<IAuthenticationAddedSender, AuthenticationAddedSender>();

}

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authentication Service");
});

app.Run();
