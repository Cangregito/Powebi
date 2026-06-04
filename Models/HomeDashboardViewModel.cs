namespace InventarioApp.Models;

public class HomeDashboardViewModel
{
    public int TotalProductos { get; set; }
    public int ProductosCriticos { get; set; }
    public int TotalMovimientos { get; set; }
    public decimal ValorInventario { get; set; }
}
