using InventarioApp.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<MovimientoInventario> Movimientos => Set<MovimientoInventario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Guardar el enum como texto para simplificar filtros en Power BI.
        modelBuilder.Entity<MovimientoInventario>()
            .Property(x => x.Tipo)
            .HasConversion<string>();

        modelBuilder.Entity<Producto>()
            .Property(x => x.PrecioUnitario)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = 1, Nombre = "Electronica", Descripcion = "Equipos electronicos" },
            new Categoria { Id = 2, Nombre = "Papeleria", Descripcion = "Articulos de oficina" },
            new Categoria { Id = 3, Nombre = "Herramientas", Descripcion = "Equipos de trabajo" }
        );

        modelBuilder.Entity<Producto>().HasData(
            new Producto { Id = 1, Nombre = "Laptop Dell", CategoriaId = 1, PrecioUnitario = 15000m, Stock = 8, StockMinimo = 5, FechaRegistro = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Producto { Id = 2, Nombre = "Mouse Logitech", CategoriaId = 1, PrecioUnitario = 350m, Stock = 3, StockMinimo = 10, FechaRegistro = new DateTime(2026, 1, 11, 0, 0, 0, DateTimeKind.Utc) },
            new Producto { Id = 3, Nombre = "Resma Papel", CategoriaId = 2, PrecioUnitario = 120m, Stock = 50, StockMinimo = 20, FechaRegistro = new DateTime(2026, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
            new Producto { Id = 4, Nombre = "Taladro Bosch", CategoriaId = 3, PrecioUnitario = 2800m, Stock = 2, StockMinimo = 3, FechaRegistro = new DateTime(2026, 1, 13, 0, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<MovimientoInventario>().HasData(
            new MovimientoInventario { Id = 1, ProductoId = 1, Tipo = TipoMovimiento.Entrada, Cantidad = 10, Fecha = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc), Observacion = "Compra inicial" },
            new MovimientoInventario { Id = 2, ProductoId = 1, Tipo = TipoMovimiento.Salida, Cantidad = 2, Fecha = new DateTime(2026, 2, 5, 0, 0, 0, DateTimeKind.Utc), Observacion = "Venta cliente A" },
            new MovimientoInventario { Id = 3, ProductoId = 2, Tipo = TipoMovimiento.Entrada, Cantidad = 5, Fecha = new DateTime(2026, 2, 3, 0, 0, 0, DateTimeKind.Utc), Observacion = "Reposicion" },
            new MovimientoInventario { Id = 4, ProductoId = 4, Tipo = TipoMovimiento.Salida, Cantidad = 1, Fecha = new DateTime(2026, 2, 7, 0, 0, 0, DateTimeKind.Utc), Observacion = "Prestamo interno" }
        );
    }
}
