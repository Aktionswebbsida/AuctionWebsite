using AuctionApi.Hubs;
using Business.Interfaces;
using Business.Services;
using Data.DbContext;
using Data.DTOs;
using Data.Entities;
using Data.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddTransient<IHubContext>(sp =>
    (IHubContext)sp.GetRequiredService<IHubContext<BidHub>>());

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMainApp", policy =>
    {
        policy.WithOrigins("https://localhost:7152")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContexts>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IAdRepository, AdRepository>();
builder.Services.AddScoped<IAdInterface, AdService>();
builder.Services.AddScoped<IBidRepository, BidRepository>();
builder.Services.AddScoped<IBidInterface, BidService>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
                  options.SwaggerEndpoint("/openapi/v1.json", "Auction API"));
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowMainApp");
app.UseAuthorization();
app.MapPost("/bids", async (BidCreateDto bidCreateDto, IBidInterface bidservice) =>
{
    await bidservice.CreateBidAsync(bidCreateDto);
   
    return Results.Accepted();


});

app.MapPut("/bids/{id}", async (int id, BidUpdateDto bidUpdateDto, IBidInterface bidservice) =>
{
    await bidservice.UpdateBid(id, bidUpdateDto);
  
    return Results.Accepted();


});
app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"))
             .ExcludeFromDescription();
app.MapHub<BidHub>("/bid");
app.Run();
