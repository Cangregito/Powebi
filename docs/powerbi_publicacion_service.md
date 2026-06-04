# Publicacion profesional en Power BI Service

## 1. Preparacion del modelo

1. Abrir Power BI Desktop.
2. Importar archivos CSV desde la app en la opcion Exportar Datos.
3. Confirmar relaciones:
   - Categorias[Id] -> Productos[CategoriaId]
   - Productos[Id] -> Movimientos[ProductoId]
4. Crear tabla Calendario y relacionarla con Movimientos[Fecha].
5. Crear medidas desde docs/powerbi_assets/medidas_dax.md.

## 2. Construccion del reporte

Pagina 1: Resumen Ejecutivo
- Tarjeta: Total Productos
- Tarjeta: Valor Total Inventario
- Tarjeta: Productos en Stock Critico
- Barras: Stock por Categoria
- Dona: Estado Stock

Pagina 2: Analisis de Movimientos
- Linea: Movimientos por Mes
- Tabla: Producto, Tipo, Cantidad, Fecha
- Segmentador: Tipo
- Segmentador: Categoria

## 3. Estilo visual

1. Vista > Temas > Examinar temas.
2. Seleccionar docs/powerbi_assets/tema_inventario.json.
3. Revisar contrastes y titulos de todos los visuales.

## 4. Publicacion a Service

1. Inicio > Publicar.
2. Seleccionar workspace.
3. Validar reporte en app.powerbi.com.
4. Configurar nombre final y descripcion.
5. Generar enlace compartible segun politicas del tenant.

## 5. Evidencia de entrega

Guardar en el repositorio:
- Captura pagina 1.
- Captura pagina 2.
- URL del reporte publicado.
- Fecha de publicacion.

## 6. Criterio de aprobacion

Se considera cerrado cuando:
- El enlace abre el reporte.
- Los KPIs coinciden con los datos de la app.
- Los filtros por Categoria y Tipo funcionan.

## 7. Incluir Power BI dentro de la app web (iframe)

Si quieres ver el dashboard dentro de la misma aplicacion MVC:

1. En Power BI Service abre el reporte y usa Archivo > Insertar informe > Sitio web o portal.
2. Copia la URL de insercion (embed URL).
3. Abre `appsettings.json` y pega la URL en `PowerBI:EmbedUrl`.
4. Ejecuta la app y entra al menu Dashboard BI.

Notas:
- En algunos tenants, Publish to web puede estar bloqueado por politicas.
- Para un escenario corporativo seguro, usar Power BI Embedded con autenticacion y token.
