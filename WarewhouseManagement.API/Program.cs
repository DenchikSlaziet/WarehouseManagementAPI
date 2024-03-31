using Microsoft.EntityFrameworkCore;
using WarehouseManagement.API.Extensions;
using WarehouseManagement.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegistrationControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.RegistrationSwagger();

//builder.Services.AddDbContextFactory<WarehouseManagementContext>(options
    //=> options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);

builder.Services.RegistrationSRC();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.CustomizeSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
