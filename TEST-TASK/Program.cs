using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using TEST_TASK.ProviderOne;
using TEST_TASK.ProviderTwo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("TestTask", client =>
{
    client.BaseAddress = new Uri("https://localhost:7220");
});

builder.Services.AddScoped<ISearchServiceProvider, ProviderOneSearchService>();
builder.Services.AddScoped<ISearchServiceProvider, ProviderTwoSearchService>();
builder.Services.AddScoped<ISearchService, SearchService>();

// SearchCache
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
