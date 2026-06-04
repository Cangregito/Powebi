using InventarioApp.Data;
using InventarioApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventarioApp.Controllers;

public class MovimientosController : Controller
{
    private readonly AppDbContext _context;

    public MovimientosController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 200)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 50, 500);

        var totalRegistros = await _context.Movimientos.CountAsync();
        var totalPaginas = Math.Max(1, (int)Math.Ceiling(totalRegistros / (double)pageSize));
        if (page > totalPaginas)
        {
            page = totalPaginas;
        }

        var movimientos = await _context.Movimientos
            .Include(m => m.Producto)
            .OrderByDescending(m => m.Fecha)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewBag.Page = page;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalPages = totalPaginas;
        ViewBag.TotalRecords = totalRegistros;

        return View(movimientos);
    }

    public IActionResult Create()
    {
        CargarProductos();
        ViewBag.Tipos = new SelectList(Enum.GetValues<TipoMovimiento>());
        return View(new MovimientoInventario { Fecha = DateTime.UtcNow });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProductoId,Tipo,Cantidad,Fecha,Observacion")] MovimientoInventario movimiento)
    {
        var producto = await _context.Productos.FindAsync(movimiento.ProductoId);
        if (producto is null)
        {
            ModelState.AddModelError(nameof(movimiento.ProductoId), "Producto invalido.");
        }

        if (producto is not null && movimiento.Tipo == TipoMovimiento.Salida && movimiento.Cantidad > producto.Stock)
        {
            ModelState.AddModelError(nameof(movimiento.Cantidad), "No hay stock suficiente para registrar esta salida.");
        }

        if (!ModelState.IsValid)
        {
            CargarProductos(movimiento.ProductoId);
            ViewBag.Tipos = new SelectList(Enum.GetValues<TipoMovimiento>(), movimiento.Tipo);
            return View(movimiento);
        }

        if (movimiento.Tipo == TipoMovimiento.Entrada)
        {
            producto!.Stock += movimiento.Cantidad;
        }
        else
        {
            producto!.Stock -= movimiento.Cantidad;
        }

        _context.Movimientos.Add(movimiento);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private void CargarProductos(int? productoId = null)
    {
        ViewBag.Productos = new SelectList(_context.Productos.OrderBy(p => p.Nombre), "Id", "Nombre", productoId);
    }
}
