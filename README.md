# RestaurantApp

Aplicación web ASP.NET Core MVC para la gestión básica de un restaurante (productos, ingredientes, pedidos, usuarios).

## Descripción

Proyecto con fines de aprendizaje que implementa un catálogo de productos con ingredientes, carrito de compras y gestión de pedidos. Usa Entity Framework Core para acceso a datos y migraciones.

## Requisitos

- .NET 8 SDK
- EF Core Tools (opcional para aplicar migraciones desde la CLI)
- Base de datos compatible (La utilizada es SQL Server). Ajustar `ConnectionStrings` en `appsettings.json`.

## Instalación y ejecución

1. Clona el repositorio:

   git clone <url-del-repo>

2. En la carpeta del proyecto, restaura paquetes:

   dotnet restore

3. Configura la cadena de conexión en `appsettings.json` o `appsettings.Development.json` (clave: `ConnectionStrings:DefaultConnection`).

4. Aplicar las migraciones a la base de datos (si queres crear la BD desde las migraciones):

   dotnet tool install --global dotnet-ef --version 8.*  # si no tienes dotnet-ef
   dotnet ef database update

5. Compilar y ejecutar la aplicación:

   dotnet build
   dotnet run

6. Abrir el navegador en `https://localhost:5001` o la URL que muestre la salida.

## Migraciones

Las migraciones están en la carpeta `Migrations/`. Si modificas el modelo:

- Crear nueva migración:

  dotnet ef migrations add NombreDeLaMigracion

- Aplicar migraciones:

  dotnet ef database update

## Estructura del proyecto

- `Controllers/` - controladores MVC
- `Models/` - modelos de dominio y vistas-modelo
- `Data/` - `AppDbContext` y configuración del EF Core
- `Views/` - vistas Razor
- `wwwroot/` - assets estáticos (css, js, imágenes)
- `Migrations/` - migraciones EF Core

## Desarrollo

- Recomendado usar Visual Studio 2022+ o VS Code con soporte para .NET 8.
- Ejecuta la app en el perfil de desarrollo para usar `appsettings.Development.json`.



