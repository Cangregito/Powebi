using InventarioApp.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioApp.Data;

public sealed class MassiveSeedOptions
{
    public bool EnabledOnStartup { get; set; }
    public int SkipIfProductCountAtLeast { get; set; } = 800;
    public int TotalCategories { get; set; } = 24;
    public int TotalProducts { get; set; } = 5000;
    public int TotalMovements { get; set; } = 45000;
}

public sealed record MassiveSeedResult(
    bool Skipped,
    int CategoriesAdded,
    int ProductsAdded,
    int MovementsAdded,
    int FinalCategories,
    int FinalProducts,
    int FinalMovements);

public static class MassiveDataSeeder
{
    private static readonly string[] CategoryPool =
    {
        "Electronica", "Papeleria", "Herramientas", "Redes", "Mobiliario", "Limpieza",
        "Seguridad", "Almacen", "Perifericos", "Impresion", "Audio", "Video",
        "Cables", "Energia", "Mantenimiento", "Consumibles", "Transporte", "Sistemas",
        "Servidor", "Comunicaciones", "Empaque", "Etiquetado", "Laboratorio", "Recepcion"
    };

    private static readonly string[] ProductPrefixes =
    {
        "Kit", "Pack", "Unidad", "Modulo", "Set", "Caja", "Lote", "Serie"
    };

    private static readonly string[] ProductNames =
    {
        "Monitor", "Teclado", "Mouse", "Laptop", "Switch", "Router", "Camara", "Sensor",
        "Resma", "Grapadora", "Taladro", "Destornillador", "Cable", "Bateria", "Fuente", "Servidor"
    };

    private static readonly string[] EntryNotes =
    {
        "Compra proveedor", "Reposicion trimestral", "Ingreso por transferencia", "Ajuste positivo"
    };

    private static readonly string[] ExitNotes =
    {
        "Venta mayorista", "Consumo interno", "Salida por proyecto", "Ajuste por merma"
    };

    public static async Task<MassiveSeedResult> SeedAsync(AppDbContext dbContext, MassiveSeedOptions options, CancellationToken cancellationToken = default)
    {
        var existingProducts = await dbContext.Productos.CountAsync(cancellationToken);
        if (existingProducts >= options.SkipIfProductCountAtLeast)
        {
            return new MassiveSeedResult(
                true,
                0,
                0,
                0,
                await dbContext.Categorias.CountAsync(cancellationToken),
                existingProducts,
                await dbContext.Movimientos.CountAsync(cancellationToken));
        }

        var random = new Random(20260604);

        var categories = await dbContext.Categorias.OrderBy(c => c.Id).ToListAsync(cancellationToken);
        var categoriesToAdd = Math.Max(0, options.TotalCategories - categories.Count);
        for (var i = 0; i < categoriesToAdd; i++)
        {
            var poolIndex = (categories.Count + i) % CategoryPool.Length;
            dbContext.Categorias.Add(new Categoria
            {
                Nombre = $"{CategoryPool[poolIndex]} {(categories.Count + i + 1):00}",
                Descripcion = "Categoria generada para pruebas de carga"
            });
        }

        var categoriesAdded = 0;
        if (categoriesToAdd > 0)
        {
            categoriesAdded = await dbContext.SaveChangesAsync(cancellationToken);
        }

        categories = await dbContext.Categorias.OrderBy(c => c.Id).ToListAsync(cancellationToken);
        var categoryIds = categories.Select(c => c.Id).ToArray();

        var productsAdded = 0;
        var productsToAdd = Math.Max(0, options.TotalProducts - existingProducts);
        const int productBatchSize = 800;

        for (var batchStart = 0; batchStart < productsToAdd; batchStart += productBatchSize)
        {
            var batchCount = Math.Min(productBatchSize, productsToAdd - batchStart);
            var productsBatch = new List<Producto>(batchCount);

            for (var i = 0; i < batchCount; i++)
            {
                var globalIndex = existingProducts + batchStart + i + 1;
                var stockMin = random.Next(4, 45);
                var stock = random.Next(stockMin / 2, stockMin * 5);
                var categoryId = categoryIds[random.Next(categoryIds.Length)];
                var prefix = ProductPrefixes[random.Next(ProductPrefixes.Length)];
                var name = ProductNames[random.Next(ProductNames.Length)];
                var unitPrice = Math.Round((decimal)(random.NextDouble() * 20000 + 15), 2);

                productsBatch.Add(new Producto
                {
                    Nombre = $"{prefix} {name} {globalIndex:00000}",
                    CategoriaId = categoryId,
                    PrecioUnitario = unitPrice,
                    Stock = stock,
                    StockMinimo = stockMin,
                    FechaRegistro = DateTime.UtcNow.AddDays(-random.Next(1, 720))
                });
            }

            dbContext.Productos.AddRange(productsBatch);
            productsAdded += await dbContext.SaveChangesAsync(cancellationToken);
        }

        var existingMovements = await dbContext.Movimientos.CountAsync(cancellationToken);
        var movementsToAdd = Math.Max(0, options.TotalMovements - existingMovements);

        var productStates = await dbContext.Productos
            .Select(p => new ProductState { Id = p.Id, Stock = p.Stock })
            .ToListAsync(cancellationToken);

        var movementsAdded = 0;
        const int movementBatchSize = 1500;

        for (var batchStart = 0; batchStart < movementsToAdd; batchStart += movementBatchSize)
        {
            var batchCount = Math.Min(movementBatchSize, movementsToAdd - batchStart);
            var movementsBatch = new List<MovimientoInventario>(batchCount);

            for (var i = 0; i < batchCount; i++)
            {
                var state = productStates[random.Next(productStates.Count)];
                var wantEntry = random.NextDouble() < 0.58;
                var quantity = random.Next(1, 24);
                TipoMovimiento tipo;
                string note;

                if (wantEntry || state.Stock <= 0)
                {
                    tipo = TipoMovimiento.Entrada;
                    state.Stock += quantity;
                    note = EntryNotes[random.Next(EntryNotes.Length)];
                }
                else
                {
                    if (quantity > state.Stock)
                    {
                        quantity = Math.Max(1, state.Stock / 2);
                    }

                    tipo = TipoMovimiento.Salida;
                    state.Stock -= quantity;
                    note = ExitNotes[random.Next(ExitNotes.Length)];
                }

                movementsBatch.Add(new MovimientoInventario
                {
                    ProductoId = state.Id,
                    Tipo = tipo,
                    Cantidad = quantity,
                    Fecha = DateTime.UtcNow.AddDays(-random.Next(1, 540)).AddMinutes(-random.Next(0, 1440)),
                    Observacion = note
                });
            }

            dbContext.Movimientos.AddRange(movementsBatch);
            movementsAdded += await dbContext.SaveChangesAsync(cancellationToken);
        }

        var productsForUpdate = await dbContext.Productos.ToListAsync(cancellationToken);
        var stockByProductId = productStates.ToDictionary(x => x.Id, x => x.Stock);

        foreach (var product in productsForUpdate)
        {
            if (stockByProductId.TryGetValue(product.Id, out var finalStock))
            {
                product.Stock = Math.Max(0, finalStock);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return new MassiveSeedResult(
            false,
            categoriesAdded,
            productsAdded,
            movementsAdded,
            await dbContext.Categorias.CountAsync(cancellationToken),
            await dbContext.Productos.CountAsync(cancellationToken),
            await dbContext.Movimientos.CountAsync(cancellationToken));
    }

    private sealed class ProductState
    {
        public int Id { get; set; }
        public int Stock { get; set; }
    }
}
