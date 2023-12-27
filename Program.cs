using CustomerManager.Data;
using CustomerManager.DataAccess;
using CustomerManager.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var dataAccessType = builder.Configuration["DataAccessType"];
if (dataAccessType == "EntityFramework")
{
    builder.Services.AddScoped<IDataAccess, EfDataAccess>();
}
else if (dataAccessType == "SqlStoredProcedures")
{
    builder.Services.AddTransient<IDataAccess, SqlDataAccess>(_ => new SqlDataAccess("Data Source=(localdb)\\MSSQLLocalDB;Database=CustomerWebApiDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"));
}

builder.Services.AddTransient<IValidator<CustomerWithNumbersDTO>, CustomerWithNumbersDTO.Validator>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
