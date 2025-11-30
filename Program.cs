using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using PaymentService.Database;
using PaymentService.Database.Repositories;
using PaymentService.Models.Entities;
using PaymentService.Models.Payment;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseMySql(
        builder.Configuration.GetConnectionString("DefaultMySql"),
        new MySqlServerVersion(new Version(8, 0, 29))
    );
});

builder.Services.AddSingleton<IPaymentMethodFactory, PaymentMethodFactory>();

builder.Services.AddTransient<IRepository<Invoice>, GenericRepository<Invoice>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});  

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
