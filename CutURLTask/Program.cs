using CutURLTask.Data;
using CutURLTask.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using MySql.EntityFrameworkCore.Extensions;
using MySqlX.XDevAPI;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация: сначала secrets, потом env
builder.Configuration
  .AddEnvironmentVariables();

var conn = builder.Configuration.GetConnectionString("MySQLDB") ?? throw new InvalidOperationException("Нет строки MySQLDB!");
string goodconn = Environment.ExpandEnvironmentVariables(conn) ?? throw new InvalidOperationException("Ошибка при подстановке перменных среды в строку подключения MySQLDB!");

// Add services to the container.
builder.Services.AddDbContext<CutUrlDbContext>(options =>
    options.UseMySQL(goodconn));

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();


// Проверка подключения к нашей Базе и применение миграций 
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CutUrlDbContext>();
    try        
    {

        if (!builder.Environment.IsDevelopment())
        { // В DEV - использую EnsureCreated — чтобы быстро поднять пустую базу 
            if (db.Database.EnsureCreated())
                Console.WriteLine("[DB-TEST] База создана через EnsureCreated.");
            else
                Console.WriteLine("[DB-TEST] База уже существует.");
        }
        else
        {
            // В PROD — всегда миграции 
            //db.Database.EnsureDeleted();
            db.Database.Migrate();
            Console.WriteLine("[DB-TEST] Миграции применены успешно.");
        }
    }
    catch (Exception ex)
    {
        // Ожидаю, что ошибки будут в применении миграций
        Console.WriteLine($"[DB-TEST] неожиданная ошибка: {ex.Message}!");
        Console.WriteLine($"[DB-TEST] строка подключения: {goodconn}!");

        throw;
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

// Маршрут для коротких ссылок 
app.MapControllerRoute(
    name: "short",
    pattern: "{code}",
    defaults: new { controller = "Cuturl", action = "RedirectToUrl" });

// Маршрут для Информационного представления
app.MapControllerRoute(
    name: "details", 
    pattern: "Details/{code}",
    defaults: new { controller = "Cuturl", action = "Details" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cuturl}/{action=Generate}/{id?}")
    .WithStaticAssets();


app.Run();
