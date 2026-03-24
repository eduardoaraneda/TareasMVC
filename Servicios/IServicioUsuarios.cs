using System.Security.Claims;

namespace TareasMVC.Servicios
{
    public interface IServicioUsuarios
    {
        string ObtenerUsuarioId();

    }

    public class ServicioUsuarios : IServicioUsuarios
    {
        private HttpContext _httpContext;
        public ServicioUsuarios(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public string ObtenerUsuarioId()
        {
            if (_httpContext.User.Identity.IsAuthenticated)
            {
                var idClaim = _httpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                return idClaim.Value;
            }
            else
            {
                throw new Exception("El usuario no esta autenticado");
            }
        }
    }
}
