using DataAggregator.Application.Services.Abstractions;
using DataAggregator.Application.Services;
using DataAggregator.Infrastructure.Db;
using DataAggregator.Infrastructure.Tenants;
using Microsoft.EntityFrameworkCore;
using DataAggregator.Infrastructure.NotificationsWriter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AggregatorDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<ITenantsReader, TenantsReader>();
builder.Services.AddScoped<INotificationsBrokerWriter, NotificationsWriter>();

builder.Services.AddScoped<ITenantDataSource, Tenant101DataSource>();
builder.Services.AddScoped<ITenantDataSource, Tenant145DataSource>();
builder.Services.AddScoped<ITenantDataSource, Tenant2DataSource>();

builder.Services.AddSingleton<IAuxiliaryClientCodeGenerator, AuxiliaryClientCodeGenerator>();

builder.Services.AddScoped<IRunAggregationUseCase, RunAggregationUseCase>();

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
