using Business.Interfaces;
using Business.Services;
using Data.DbContext;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContexts>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IAddRepository, AddRepository>();
builder.Services.AddScoped<IAdInterface, AdService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
                  options.SwaggerEndpoint("/openapi/v1.json", "Auction API"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"))
             .ExcludeFromDescription();

app.Run();
