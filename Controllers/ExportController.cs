using System.Globalization;
using System.Text;
using InventarioApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventarioApp.Controllers;

public class ExportController : Controller
{
    private readonly AppDbContext _context;

    public ExportController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<FileContentResult> CategoriasCsv()
    {
        var categorias = await _context.Categorias
            .OrderBy(c => c.Id)
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("Id,Nombre,Descripcion");

        foreach (var c in categorias)
        {
            sb.AppendLine($"{c.Id},{Csv(c.Nombre)},{Csv(c.Descripcion)}");
        }

        return CsvFile(sb.ToString(), "categorias.csv");
    }

    public async Task<FileContentResult> ProductosCsv()
    {
        var productos = await _context.Productos
            .OrderBy(p => p.Id)
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("Id,Nombre,CategoriaId,PrecioUnitario,Stock,StockMinimo,FechaRegistro");

        foreach (var p in productos)
        {
            sb.AppendLine(string.Join(",", new[]
            {
                p.Id.ToString(),
                Csv(p.Nombre),
                p.CategoriaId.ToString(),
                p.PrecioUnitario.ToString(CultureInfo.InvariantCulture),
                p.Stock.ToString(),
                p.StockMinimo.ToString(),
                p.FechaRegistro.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)
            }));
        }

        return CsvFile(sb.ToString(), "productos.csv");
    }

    public async Task<FileContentResult> MovimientosCsv()
    {
        var movimientos = await _context.Movimientos
            .OrderBy(m => m.Id)
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("Id,ProductoId,Tipo,Cantidad,Fecha,Observacion");

        foreach (var m in movimientos)
        {
            sb.AppendLine(string.Join(",", new[]
            {
                m.Id.ToString(),
                m.ProductoId.ToString(),
                Csv(m.Tipo.ToString()),
                m.Cantidad.ToString(),
                m.Fecha.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                Csv(m.Observacion)
            }));
        }

        return CsvFile(sb.ToString(), "movimientos.csv");
    }

    private static FileContentResult CsvFile(string content, string fileName)
    {
        return new FileContentResult(Encoding.UTF8.GetBytes(content), "text/csv")
        {
            FileDownloadName = fileName
        };
    }

    private static string Csv(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        return $"\"{value.Replace("\"", "\"\"")}\"";
    }
}
