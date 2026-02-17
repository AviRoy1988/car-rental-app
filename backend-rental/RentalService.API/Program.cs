using Microsoft.EntityFrameworkCore;
using RentalService.API.Data;
using RentalService.API.Middleware;
using RentalService.API.Models;
using RentalService.API.Models.DTOs;
using RentalService.API.Pricing;
using RentalService.API.Repositories;
using RentalService.API.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext());

try
{
    Log.Information("Starting Rental Service API");

    // Add services to the container
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "Rental Service API", Version = "v1" });
    });

    // Add AutoMapper
    builder.Services.AddAutoMapper(typeof(Program));

    // Add CORS policy for frontend
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });

    // Configure Database
    builder.Services.AddDbContext<RentalDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            sqlOptions => sqlOptions.EnableRetryOnFailure()
        )
    );

    // Configure Price Calculation Settings
    builder.Services.Configure<PriceCalculationConfig>(
        builder.Configuration.GetSection("PriceCalculation"));

    // Register Repositories
    builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    builder.Services.AddScoped<IRentalRepository, RentalRepository>();

    // Register Price Calculators (Strategy Pattern)
    builder.Services.AddScoped<IPriceCalculator, SmallCarPriceCalculator>();
    builder.Services.AddScoped<IPriceCalculator, CombiPriceCalculator>();
    builder.Services.AddScoped<IPriceCalculator, TruckPriceCalculator>();
    builder.Services.AddScoped<PriceCalculatorFactory>();

    // Register Services
    builder.Services.AddScoped<IRentalService, RentalService.API.Services.RentalService>();
    builder.Services.AddScoped<IInvoiceService, InvoiceService>();

    var app = builder.Build();

    // Configure the HTTP request pipeline
    // Add Serilog request logging
    app.UseSerilogRequestLogging();

    // Add global exception handling middleware
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // Enable CORS
    app.UseCors("AllowFrontend");

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
