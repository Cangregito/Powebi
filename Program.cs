using InventarioApp.Data;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

var seedMassiveOnly = args.Any(a => string.Equals(a, "--seed-massive", StringComparison.OrdinalIgnoreCase));
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=inventario.db";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();

    var seedOptions = builder.Configuration.GetSection("MassiveSeed").Get<MassiveSeedOptions>() ?? new MassiveSeedOptions();
    if (seedMassiveOnly || seedOptions.EnabledOnStartup)
    {
        var result = await MassiveDataSeeder.SeedAsync(dbContext, seedOptions);
        Console.WriteLine($"[MassiveSeed] skipped={result.Skipped} categoriesAdded={result.CategoriesAdded} productsAdded={result.ProductsAdded} movementsAdded={result.MovementsAdded} finalCategories={result.FinalCategories} finalProducts={result.FinalProducts} finalMovements={result.FinalMovements}");
    }
}

if (seedMassiveOnly)
{
    return;
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
