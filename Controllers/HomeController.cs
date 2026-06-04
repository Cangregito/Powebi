using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InventarioApp.Models;
using InventarioApp.Data;
using Microsoft.EntityFrameworkCore;

namespace InventarioApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var model = new HomeDashboardViewModel
        {
            TotalProductos = await _context.Productos.CountAsync(),
            ProductosCriticos = await _context.Productos.CountAsync(p => p.Stock < p.StockMinimo),
            TotalMovimientos = await _context.Movimientos.CountAsync(),
            ValorInventario = await _context.Productos
                .Select(p => p.Stock * p.PrecioUnitario)
                .DefaultIfEmpty(0)
                .SumAsync()
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
