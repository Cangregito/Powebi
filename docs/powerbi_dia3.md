# Dia 3 - Modelo Power BI (ejecucion real)

Este archivo implementa el paso del plan para el modelo analitico con el estado actual del proyecto.

## 1. Obtener datos

Opcion recomendada en este entorno:

1. Ejecutar la app.
2. Ir a la pagina Exportar Datos.
3. Descargar:
   - categorias.csv
   - productos.csv
   - movimientos.csv
4. En Power BI Desktop: Inicio > Obtener datos > Texto/CSV.
5. Cargar los 3 archivos.

## 2. Relacionar tablas

- Categorias[Id] 1 -> * Productos[CategoriaId]
- Productos[Id] 1 -> * Movimientos[ProductoId]

## 3. Tabla calendario

```dax
Calendario =
CALENDAR(
    DATE(2024, 1, 1),
    DATE(2026, 12, 31)
)

Anio = YEAR(Calendario[Date])
Mes = MONTH(Calendario[Date])
NombreMes = FORMAT(Calendario[Date], "MMMM")
Trimestre = "T" & QUARTER(Calendario[Date])
```

Relacionar Calendario[Date] con Movimientos[Fecha].

## 4. Medidas DAX

```dax
Total Productos =
COUNTROWS(Productos)

Productos en Stock Critico =
CALCULATE(
    COUNTROWS(Productos),
    Productos[Stock] < Productos[StockMinimo]
)

Valor Total Inventario =
SUMX(
    Productos,
    Productos[Stock] * Productos[PrecioUnitario]
)

Porcentaje Critico =
DIVIDE(
    [Productos en Stock Critico],
    [Total Productos],
    0
)

Total Movimientos =
COUNTROWS(Movimientos)

Entradas Este Mes =
CALCULATE(
    COUNTROWS(Movimientos),
    Movimientos[Tipo] = "Entrada",
    MONTH(Movimientos[Fecha]) = MONTH(TODAY()),
    YEAR(Movimientos[Fecha]) = YEAR(TODAY())
)

Salidas Este Mes =
CALCULATE(
    COUNTROWS(Movimientos),
    Movimientos[Tipo] = "Salida",
    MONTH(Movimientos[Fecha]) = MONTH(TODAY()),
    YEAR(Movimientos[Fecha]) = YEAR(TODAY())
)
```

## 5. Layout recomendado del reporte

Pagina 1 - Resumen Ejecutivo
- Tarjeta: Total Productos
- Tarjeta: Valor Total Inventario
- Tarjeta: Productos en Stock Critico
- Barra: Stock por Categoria
- Dona: Critico vs Normal

Pagina 2 - Analisis de Movimientos
- Linea: Movimientos por mes
- Tabla: Producto, Tipo, Cantidad, Fecha
- Segmentador: Tipo
- Segmentador: Categoria
