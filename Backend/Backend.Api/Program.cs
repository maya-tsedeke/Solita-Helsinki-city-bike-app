using Microsoft.AspNetCore.Hosting;
using AutoMapper;
using Backend.Applications.Mapping;
using Backend.Infrastructure.Repositories;
using Backend.Applications.Interfaces.Services;
using Backend.Infrastructure.Services;
using Backend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Backend.Applications.Interfaces.Repositories;
using Backend.Domain.Entities;
using Backend.Domain.DTOs;

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


//Import Journey dependency injection
builder.Services.AddScoped < ICSV_Repository<JourneyDto>, CSV_Repository <JourneyDto>> (); 
builder.Services.AddScoped<ICSV_ImportService<JourneyDto>,CSV_ImportService<JourneyDto>> ();
//Import Station dependency injection
builder.Services.AddScoped<IImportStationRepository<Station>, ImportStationRepository<Station>>();
builder.Services.AddScoped<IImportStationService<Station>, ImportStationService<Station>>();

// Register AutoMapper;
//builder.Services.AddAutoMapper(typeof(mappingProfile));
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
