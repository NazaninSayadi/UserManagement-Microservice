using Microsoft.EntityFrameworkCore;
using RabbitMQ;
using User.BusinessLogic.Repositories;
using User.BusinessLogic.Services.Implementations;
using User.BusinessLogic.Services.Interfaces;
using User.Infrastructure;
using User.Infrastructure.Repositories;

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
        Title = "User service",
        Version = "v1"
    });
});
builder.Services.AddDbContext<UserContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository>(x => new UserRepository(x.GetService<UserContext>()));
builder.Services.AddScoped<IUserService>(x => new UserService(x.GetService<IUserRepository>()));

builder.Services.AddControllers();

if (serviceClientSettings.Enabled)
{
    builder.Services.AddHostedService<Receiver>();
    builder.Services.AddSingleton<IUserAddedSender, UserAddedSender>();
}
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Service");
});
app.MapControllers();

app.Run();
