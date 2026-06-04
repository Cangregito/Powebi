# Sistema de Control de Inventario con Dashboard Analitico

Proyecto de demostracion para entrevista de Analista de Datos Jr con Power BI.

Repositorio: https://github.com/Cangregito/Powebi.git

## 1. Proposito del proyecto

Construir una aplicacion web para administrar inventario (productos, categorias y movimientos) y usar esos datos como fuente analitica en Power BI para visualizar KPIs clave:

- Total de productos
- Productos en stock critico
- Valor total del inventario
- Tendencias de entradas y salidas

## 2. Capturas de pantalla

Pendiente: agregar imagenes de:

- Pantalla de Inventario
- Pantalla de Movimientos
- Dashboard pagina 1 y pagina 2 en Power BI

## 3. Arquitectura

```text
Usuario
  -> ASP.NET Core MVC (Razor + C#)
  -> Entity Framework Core
  -> SQLite (inventario.db)
  -> Export CSV / Modelo Power BI
  -> Dashboard en Power BI Desktop/Service
```

## 4. Tecnologias

- .NET 8
- ASP.NET Core MVC
- C#
- Entity Framework Core 8
- SQLite (desarrollo local actual)
- Power BI Desktop
- Power BI Service

## 5. Como ejecutar localmente

1. Requisitos:
   - .NET SDK 8
2. Restaurar dependencias:

```bash
dotnet restore
```

3. Aplicar base de datos:

```bash
dotnet ef database update
```

4. Ejecutar la aplicacion:

```bash
dotnet run
```

5. Abrir en navegador la URL que muestra la consola.

## 6. Flujo para Power BI

1. En la app, abrir el menu Exportar Datos.
2. Descargar los archivos:
   - categorias.csv
   - productos.csv
   - movimientos.csv
3. En Power BI Desktop usar Obtener datos > Texto/CSV.
4. Revisar guia detallada en docs/powerbi_dia3.md.

## 7. Enlaces de publicacion

- Dashboard Power BI Service: pendiente
- App desplegada: pendiente

## 8. Despliegue

- Guia completa: docs/despliegue_dia4.md
- Railway: archivo railway.json incluido
- La app ejecuta migraciones automaticamente al iniciar

## 9. Estado actual

Completado:

- CRUD de Productos
- Registro de Movimientos con validacion de stock
- Filtro en Inventario sin recarga
- Estilos responsivos con Grid para Inventario
- Exportacion CSV para alimentar Power BI

Pendiente:

- Publicacion de app en internet
- Publicacion del dashboard en Power BI Service
