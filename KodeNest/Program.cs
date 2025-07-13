using KodeNest.Repository.Implementation;
using KodeNest.Repository.Interface;
using KodeNest.Service.Implementation;
using KodeNest.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using DataLogger; // 👈 For DatLogger
using System.IO;  // 👈 Required for Directory
using System;     // 👈 Required for AppDomain

var builder = WebApplication.CreateBuilder(args);

// ✅ Ensure Logs folder exists for log4net
var logsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
Directory.CreateDirectory(logsPath);

// ✅ Register AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Register DatLogger with constructor injection
builder.Services.AddScoped<DatLogger>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connStr = config.GetConnectionString("DefaultConnection");
    return new DatLogger(connStr);
});

// ✅ Register Repositories and Services
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<IHomeService, HomeService>();

// ✅ Swagger & Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();

// ✅ Middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
