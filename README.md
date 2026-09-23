# Tareas MVC

Aplicación web para gestionar tareas, pasos y archivos adjuntos, con usuarios autenticados y roles. Incluye controladores MVC, vistas Razor y una API de tareas en `/api/tareas`.

## Tecnologías

C# · .NET 10 · ASP.NET Core MVC · Entity Framework Core · SQL Server · ASP.NET Core Identity · AutoMapper. Incluye recursos de localización y autenticación externa con Microsoft.

## Estructura

- `Controllers/`: interfaz web, usuarios y API de tareas.
- `Entidades/` y `ApplicationDbContext.cs`: modelo de datos y persistencia.
- `Migrations/`: migraciones de SQL Server e Identity.
- `Views/`, `Recursos/` y `wwwroot/`: interfaz, traducciones y recursos estáticos.

## Ejecutar localmente

Requisitos: SDK de .NET 10, SQL Server y la herramienta `dotnet-ef` compatible con EF Core 10.

Desde la raíz:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=TareasMVC;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"
dotnet restore TareasMVC.csproj
dotnet build TareasMVC.csproj
dotnet ef database update --project TareasMVC.csproj
dotnet run --project TareasMVC.csproj
```

Aplica las migraciones únicamente a una base de desarrollo propia. El ejemplo de certificado es solo para desarrollo local. Abre la URL indicada por la terminal; la ruta inicial es `/Usuarios/Login`.

El arranque registra el proveedor Microsoft sin condición: configura `MicrosoftClientId` y `MicrosoftClientSecret` mediante User Secrets con una aplicación propia y su redirección OAuth correspondiente. No se incluyen credenciales de ese proveedor.

## Estado

Proyecto de portafolio. Las migraciones están incluidas; no hay una demo pública enlazada ni una suite de pruebas automatizadas incluida. La ejecución completa requiere configurar SQL Server y el proveedor de autenticación.

## Configuración y alcance

Usa una base de desarrollo y credenciales propias. Configura secretos mediante variables de entorno o User Secrets; no los incluyas en commits. La compilación no comprueba la disponibilidad de bases de datos, SMTP o APIs externas.

## Autor

[Eduardo Araneda](https://github.com/eduardoaraneda) · [Portafolio](https://eduardoaraneda.github.io/Portafolio/)
