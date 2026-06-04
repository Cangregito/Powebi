using System.ComponentModel.DataAnnotations;

namespace InventarioApp.Models;

public class Producto
{
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    [Range(0, double.MaxValue)]
    public decimal PrecioUnitario { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    [Range(0, int.MaxValue)]
    public int StockMinimo { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public ICollection<MovimientoInventario> Movimientos { get; set; } = new List<MovimientoInventario>();
}
