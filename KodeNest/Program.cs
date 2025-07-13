using System.IO;
using System.Reflection;
using DataLogger;
using KodeNest.Repository.Implementation;
using KodeNest.Repository.Interface;
using KodeNest.Service.Implementation;
using KodeNest.Service.Interface;
using log4net;
using log4net.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Ensure Logs folder exists (log4net will create applog.txt itself)
string logsDir = Path.Combine(AppContext.BaseDirectory, "Logs");
if (!Directory.Exists(logsDir))
{
    Directory.CreateDirectory(logsDir);
}

string todayLogPath = Path.Combine(logsDir, $"applog.txt");
if (!File.Exists(todayLogPath))
{
    File.Create(todayLogPath).Dispose(); // create and release handle
}

// 🔧 Configure log4net using log4net.config
var logRepo = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepo, new FileInfo("log4net.config"));

// ✅ Register EF DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Register custom logger
builder.Services.AddScoped<DatLogger>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    //var connStr = config.GetConnectionString("DefaultConnection");
    return new DatLogger();
});

// ✅ Register repository & service layers
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<IHomeService, HomeService>();

// ✅ Swagger & controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

// ✅ Build app
var app = builder.Build();

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
