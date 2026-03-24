using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TareasMVC.Models;
using TareasMVC.Servicios;

namespace TareasMVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public UsuariosController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext applicationDbContext)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._context = applicationDbContext;
        }
        [AllowAnonymous]
        public IActionResult Registro()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }
            var usuario = new IdentityUser { UserName = modelo.Email, Email = modelo.Email };
            var resultado = await _userManager.CreateAsync(usuario, modelo.Password);
            if (resultado.Succeeded)
            {
                await _signInManager.SignInAsync(usuario, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(modelo);
        }
        [AllowAnonymous]
        public IActionResult Login(string mensaje = null)
        {
            if (mensaje != null)
            {
                ViewData["Mensaje"] = mensaje;
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }
            var resultado = await _signInManager.PasswordSignInAsync(modelo.Email, modelo.Password, modelo.Recuerdame, lockoutOnFailure: false);
            if (resultado.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido.");
            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Usuarios");
        }
        [AllowAnonymous]
        [HttpGet]
        public ChallengeResult LoginConProveedor(string proveedor, string urlRetorno = null)
        {
            var redireccionUrl = Url.Action("RegistrarUsuarioExterno", new { ReturnUrl = urlRetorno });
            var propiedades = _signInManager.ConfigureExternalAuthenticationProperties(proveedor, redireccionUrl);
            return new ChallengeResult(proveedor, propiedades);
        }
        [AllowAnonymous]
        public async Task<IActionResult> RegistrarUsuarioExterno(string returnUrl = null, string remoteError = null)
        {
            // Si no vienen returnUrl, enviar al Home
            returnUrl ??= Url.Action("Index", "Home");

            // Error que viene del proveedor (Microsoft)
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error de proveedor externo: {remoteError}");
                return RedirectToAction("Login");
            }

            // Obtener los datos del login externo
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("Login");
            }

            // Intentar iniciar sesión con el proveedor externo
            var resultadoInicioSesion = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: true,
                bypassTwoFactor: true);

            if (resultadoInicioSesion.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            // Si no existía login asociado, se crea un usuario nuevo
            var email = info.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.Email);
            if (email != null)
            {
                var usuario = new IdentityUser { UserName = email, Email = email };
                var resultadoCreacion = await _userManager.CreateAsync(usuario);

                if (resultadoCreacion.Succeeded)
                {
                    // Asociar login externo al usuario
                    resultadoCreacion = await _userManager.AddLoginAsync(usuario, info);

                    if (resultadoCreacion.Succeeded)
                    {
                        // Login exitoso
                        await _signInManager.SignInAsync(usuario, isPersistent: true, info.LoginProvider);
                        return LocalRedirect(returnUrl);
                    }
                }

                // Manejo de errores de creación
                foreach (var error in resultadoCreacion.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Si algo falla → volver al Login
            return RedirectToAction("Login");
        }
        [HttpGet]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Listado(string mensaje = null)
        {
            var usuarios = await _context.Users.Select(u => new UsuarioViewModel
            {
                email = u.Email
            }).ToListAsync();
            var modelo = new UsuarioListadoViewModel();
            modelo.usuarios = usuarios;
            modelo.mensaje = mensaje;
            return View(modelo);

        }
        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> HacerAdmin(string email)
        {
            var usuaario = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (usuaario == null)
            {
                return RedirectToAction("Listado", new { mensaje = "Usuario no encontrado" });
            }
            await _userManager.AddToRoleAsync(usuaario, Constantes.RolAdmin);
            return RedirectToAction("Listado", new { mensaje = "Usuario actualizado a admin" });
        }
        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> RemoverAdmin(string email)
        {
            var usuaario = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (usuaario == null)
            {
                return RedirectToAction("Listado", new { mensaje = "Usuario no encontrado" });
            }
            await _userManager.RemoveFromRoleAsync(usuaario, Constantes.RolAdmin);
            return RedirectToAction("Listado", new { mensaje = "Usuario Removido de admin" });
        }
    }
}
