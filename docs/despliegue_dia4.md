# Dia 4 - Despliegue de app y publicacion de dashboard

Este documento traduce el plan a pasos ejecutables con el estado actual del proyecto.

## 1. Deploy rapido en Railway

1. Subir proyecto a GitHub.
2. Crear proyecto en Railway desde el repositorio.
3. Railway detecta .NET y usa railway.json para arranque.
4. Configurar variables en Railway:
   - ASPNETCORE_ENVIRONMENT=Production
   - ConnectionStrings__DefaultConnection=Data Source=/data/inventario.db
5. Desplegar.

Nota: la app aplica migraciones automaticamente al iniciar.

## 2. Deploy en Azure App Service

1. Crear App Service en plan F1.
2. Publicar desde Visual Studio o GitHub Actions.
3. Configurar Application Settings:
   - ASPNETCORE_ENVIRONMENT=Production
   - ConnectionStrings__DefaultConnection=Data Source=D:\\home\\site\\wwwroot\\inventario.db
4. Reiniciar app.

## 3. Publicacion de dashboard Power BI Service

1. Abrir PBIX en Power BI Desktop.
2. Publicar al workspace.
3. Probar visuales y filtros.
4. Generar enlace de comparticion o incrustado segun politicas del tenant.

## 4. Checklist de validacion post deploy

- App abre URL publica sin error.
- Inventario permite crear, editar y eliminar productos.
- Movimientos actualizan stock correctamente.
- Exportaciones CSV funcionan.
- Enlace del dashboard disponible.
