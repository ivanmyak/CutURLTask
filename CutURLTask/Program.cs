var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

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


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cuturl}/{action=Generate}/{id?}")
    .WithStaticAssets();


app.Run();
