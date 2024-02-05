using CustomerService;
using CustomerService.Database;
using CustomerService.Database.Models;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Serilog;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Log.Logger = new LoggerConfiguration()
 .WriteTo.Console().CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddDbContext<CustomerDBContext>();

builder.Services.AddScoped<IValidator<Customer>, CustomerValidator>();
builder.Services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(ExceptionHandlingBehavior<,,>));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
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

app.UseSerilogRequestLogging();

app.Run();
