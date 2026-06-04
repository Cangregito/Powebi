# Narrativa de Entrevista - Proyecto Inventario + Power BI

## 1. Por que elegi este proyecto

Elegi un sistema de inventario porque me permite demostrar capacidades de extremo a extremo: desarrollo de backend con C#, persistencia con Entity Framework y analisis de negocio con Power BI usando indicadores concretos.

## 2. Decisiones tecnicas principales

- Use arquitectura MVC para separar logica, vistas y acceso a datos.
- Use Entity Framework Core con migraciones para versionar el esquema.
- Modele movimientos de inventario como entradas y salidas para soportar analisis temporal.
- Mantuve el enum TipoMovimiento como texto en base para simplificar los filtros DAX.

## 3. Como manejo stock critico

En la web, se evalua por producto si Stock es menor que StockMinimo y se muestra estado visual critico o normal. En analitica, la medida DAX cuenta productos en condicion critica para alertar rapidamente.

## 4. Que mejoraria con mas tiempo

- Autenticacion y roles con Identity.
- Alertas automaticas por correo para stock critico.
- Despliegue productivo con Azure SQL y refresco gobernado en Power BI Service.

## 5. Mensaje corto de cierre

El proyecto demuestra que puedo transformar datos operativos en decisiones: desde el registro en la aplicacion hasta la visualizacion de KPIs en dashboard.
