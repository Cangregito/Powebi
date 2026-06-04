# Medidas DAX del proyecto

Copia y pega estas medidas en Power BI Desktop.

## Tabla Productos

```dax
Total Productos =
COUNTROWS(Productos)
```

```dax
Productos en Stock Critico =
CALCULATE(
    COUNTROWS(Productos),
    Productos[Stock] < Productos[StockMinimo]
)
```

```dax
Valor Total Inventario =
SUMX(
    Productos,
    Productos[Stock] * Productos[PrecioUnitario]
)
```

```dax
Porcentaje Critico =
DIVIDE(
    [Productos en Stock Critico],
    [Total Productos],
    0
)
```

## Tabla Movimientos

```dax
Total Movimientos =
COUNTROWS(Movimientos)
```

```dax
Entradas Este Mes =
CALCULATE(
    COUNTROWS(Movimientos),
    Movimientos[Tipo] = "Entrada",
    MONTH(Movimientos[Fecha]) = MONTH(TODAY()),
    YEAR(Movimientos[Fecha]) = YEAR(TODAY())
)
```

```dax
Salidas Este Mes =
CALCULATE(
    COUNTROWS(Movimientos),
    Movimientos[Tipo] = "Salida",
    MONTH(Movimientos[Fecha]) = MONTH(TODAY()),
    YEAR(Movimientos[Fecha]) = YEAR(TODAY())
)
```

## Tabla de soporte para estado

```dax
Estado Stock =
IF(
    Productos[Stock] < Productos[StockMinimo],
    "Critico",
    "Normal"
)
```
