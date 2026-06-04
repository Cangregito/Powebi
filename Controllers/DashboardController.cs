using InventarioApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventarioApp.Controllers;

public class DashboardController : Controller
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index() => View();

    // KPIs generales
    [HttpGet]
    public async Task<IActionResult> GetKpis()
    {
        var productos = await _context.Productos
            .Select(p => new { p.Stock, p.PrecioUnitario, p.StockMinimo })
            .ToListAsync();

        var totalMovimientos = await _context.Movimientos.CountAsync();
        var entradas = await _context.Movimientos.CountAsync(m => m.Tipo == Models.TipoMovimiento.Entrada);
        var salidas = await _context.Movimientos.CountAsync(m => m.Tipo == Models.TipoMovimiento.Salida);
        var criticos = productos.Count(p => p.Stock < p.StockMinimo);
        var valorTotal = productos.Sum(p => p.Stock * p.PrecioUnitario);
        var totalCategorias = await _context.Categorias.CountAsync();

        return Json(new
        {
            totalProductos = productos.Count,
            totalMovimientos,
            entradas,
            salidas,
            productosCriticos = criticos,
            valorInventario = valorTotal,
            totalCategorias
        });
    }

    // Movimientos agrupados por mes (ultimos 12 meses)
    [HttpGet]
    public async Task<IActionResult> GetMovimientosPorMes()
    {
        var desde = DateTime.UtcNow.AddMonths(-11);
        var movimientos = await _context.Movimientos
            .Where(m => m.Fecha >= desde)
            .Select(m => new { m.Fecha, m.Tipo, m.Cantidad })
            .ToListAsync();

        var meses = Enumerable.Range(0, 12)
            .Select(i => desde.AddMonths(i))
            .ToList();

        var labels = meses.Select(m => m.ToString("MMM yyyy")).ToArray();

        var entradas = meses.Select(mes =>
            movimientos
                .Where(m => m.Fecha.Year == mes.Year && m.Fecha.Month == mes.Month &&
                            m.Tipo == Models.TipoMovimiento.Entrada)
                .Sum(m => m.Cantidad)).ToArray();

        var salidas = meses.Select(mes =>
            movimientos
                .Where(m => m.Fecha.Year == mes.Year && m.Fecha.Month == mes.Month &&
                            m.Tipo == Models.TipoMovimiento.Salida)
                .Sum(m => m.Cantidad)).ToArray();

        return Json(new { labels, entradas, salidas });
    }

    // Stock total y valor por categoria
    [HttpGet]
    public async Task<IActionResult> GetStockPorCategoria()
    {
        var data = await _context.Productos
            .Include(p => p.Categoria)
            .GroupBy(p => p.Categoria!.Nombre)
            .Select(g => new
            {
                categoria = g.Key,
                totalStock = g.Sum(p => p.Stock),
                totalProductos = g.Count()
            })
            .OrderByDescending(x => x.totalStock)
            .Take(12)
            .ToListAsync();

        return Json(new
        {
            labels = data.Select(d => d.categoria).ToArray(),
            stock = data.Select(d => d.totalStock).ToArray(),
            productos = data.Select(d => d.totalProductos).ToArray()
        });
    }

    // Distribucion entradas vs salidas (dona)
    [HttpGet]
    public async Task<IActionResult> GetBalanceMovimientos()
    {
        var entradas = await _context.Movimientos
            .Where(m => m.Tipo == Models.TipoMovimiento.Entrada)
            .SumAsync(m => (long)m.Cantidad);

        var salidas = await _context.Movimientos
            .Where(m => m.Tipo == Models.TipoMovimiento.Salida)
            .SumAsync(m => (long)m.Cantidad);

        return Json(new { entradas, salidas });
    }

    // Top 10 productos por valor total (Stock * Precio)
    [HttpGet]
    public async Task<IActionResult> GetTopProductosPorValor()
    {
        var todos = await _context.Productos
            .Select(p => new { p.Nombre, p.Stock, p.PrecioUnitario })
            .ToListAsync();

        var top = todos
            .Select(p => new { p.Nombre, valor = p.Stock * p.PrecioUnitario })
            .OrderByDescending(p => p.valor)
            .Take(10)
            .ToList();

        return Json(new
        {
            labels = top.Select(p => p.Nombre.Length > 22 ? p.Nombre[..22] + "…" : p.Nombre).ToArray(),
            valores = top.Select(p => Math.Round(p.valor, 2)).ToArray()
        });
    }

    // Estado de stock: normal, critico, sin stock
    [HttpGet]
    public async Task<IActionResult> GetEstadoStock()
    {
        var productos = await _context.Productos
            .Select(p => new { p.Stock, p.StockMinimo })
            .ToListAsync();

        var sinStock = productos.Count(p => p.Stock == 0);
        var critico = productos.Count(p => p.Stock > 0 && p.Stock < p.StockMinimo);
        var normal = productos.Count(p => p.Stock >= p.StockMinimo);

        return Json(new { sinStock, critico, normal });
    }
}
