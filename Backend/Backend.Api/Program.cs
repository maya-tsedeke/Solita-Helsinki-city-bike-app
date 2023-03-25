using Backend.Applications.Mapping;
using Backend.Infrastructure.Repositories;
using Backend.Applications.Interfaces.Services;
using Backend.Infrastructure.Services;
using Backend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Backend.Applications.Interfaces.Repositories;


var builder = WebApplication.CreateBuilder(args);
//Register DBcontext
builder.Services.AddDbContext<AppDbcontext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Journey
builder.Services.AddScoped<IJourneyService, JourneyService>();
builder.Services.AddScoped<IJourneyRepository, JourneyRepository>();
//Station
builder.Services.AddScoped<IStationRepository, StationRepository>();
builder.Services.AddScoped<IStationService, StationService>();

// Register AutoMapper;
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<mappingProfile>();
});

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
