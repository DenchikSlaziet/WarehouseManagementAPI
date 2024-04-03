using Microsoft.EntityFrameworkCore;
using NSwag;
using WarehouseManagement.API.Extensions;
using WarehouseManagement.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegistrationControllers();
builder.Services.AddOpenApiDocument(options =>
{
    options.PostProcess = document =>
    {
        document.Info = new OpenApiInfo
        {
            Version = "v1",
            Title = "WarehouseManagementAPI",
            Description = "Документация WarehouseManagementApi"
        };
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.RegistrationSwagger();

builder.Services.AddDbContextFactory<WarehouseManagementContext>(options
    => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);

builder.Services.RegistrationSRC();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    //app.CustomizeSwaggerUI();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
