using InventarioApp.Data;
using InventarioApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventarioApp.Controllers;

public class ProductosController : Controller
{
    private readonly AppDbContext _context;

    public ProductosController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var productos = await _context.Productos
            .Include(p => p.Categoria)
            .OrderBy(p => p.Nombre)
            .ToListAsync();

        return View(productos);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var producto = await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (producto is null)
        {
            return NotFound();
        }

        return View(producto);
    }

    public IActionResult Create()
    {
        CargarCategorias();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre,CategoriaId,PrecioUnitario,Stock,StockMinimo")] Producto producto)
    {
        if (!ModelState.IsValid)
        {
            CargarCategorias(producto.CategoriaId);
            return View(producto);
        }

        _context.Add(producto);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var producto = await _context.Productos.FindAsync(id);
        if (producto is null)
        {
            return NotFound();
        }

        CargarCategorias(producto.CategoriaId);
        return View(producto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,CategoriaId,PrecioUnitario,Stock,StockMinimo")] Producto producto)
    {
        if (id != producto.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            CargarCategorias(producto.CategoriaId);
            return View(producto);
        }

        try
        {
            _context.Update(producto);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductoExiste(producto.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var producto = await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (producto is null)
        {
            return NotFound();
        }

        return View(producto);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto is not null)
        {
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ProductoExiste(int id)
    {
        return _context.Productos.Any(e => e.Id == id);
    }

    private void CargarCategorias(int? categoriaSeleccionada = null)
    {
        ViewBag.Categorias = new SelectList(_context.Categorias.OrderBy(c => c.Nombre), "Id", "Nombre", categoriaSeleccionada);
    }
}
