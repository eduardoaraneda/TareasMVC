# \# TareasMVC

# 

# Proyecto ASP.NET Core (Razor/MVC) para gestión de tareas.

# 

# \## Descripción

# Aplicación web para crear, editar y listar tareas y usuarios. Usa ASP.NET Core con vistas Razor y una estructura MVC (controladores y vistas).

# 

# \## Requisitos

# \- .NET 10 SDK

# \- Visual Studio 2022/2026 o VS Code

# \- Node/npm (opcional para paquetes frontend)

# 

# \## Ejecutar

# 1\. Abrir la solución `TareasMVC.sln` en Visual Studio y ejecutar (F5) o:

# 2\. Desde terminal PowerShell en la carpeta del proyecto:

# 

# ```powershell

# dotnet restore

# dotnet build

# dotnet run --project TareasMVC

# ```

# 

# La aplicación por defecto se sirve en `https://localhost:5001` o la URL indicada en la salida.

# 

# \## Estructura relevante

# \- `Controllers/` - Controladores MVC (`HomeController`, `TareasController`, `UsuariosController`, ...)

# \- `Views/` - Vistas Razor por controlador

# &#x20; - `Views/Shared/\_Layout.cshtml` - Layout principal

# &#x20; - `Views/Shared/\_linkslogin.cshtml` - partial usado en el layout

# \- `wwwroot/` - archivos estáticos (JS, CSS, librerías)

# \- `wwwroot/js/Tareas.js` - scripts relacionados con las tareas

# 

# \## Endpoints importantes

# \- `Home/Index` - Página inicial

# \- `Usuarios/Registro` - Registro de usuarios

# \- `Tareas/\*` - Operaciones relacionadas con tareas (listado, editar, crear)

# 

# \## Desarrollo

# \- Seguir convenciones de MVC y usar `IActionResult`/`ActionResult<T>` en acciones de controladores.

# \- Las vistas usan Bootstrap y algunas librerías externas (Knockout, SweetAlert).

# 

# \## Tests

# Si hay proyecto de tests, ejecutar:

# 

# ```powershell

# dotnet test

# ```

# 

# \## Contribuciones

# Abrir un issue o enviar pull request con mejoras.

# 

# \## Licencia

# Este proyecto no incluye información de licencia; añadir `LICENSE` si es necesario.

# 

# \---

# Generado automáticamente — editar según necesidades del proyecto.



