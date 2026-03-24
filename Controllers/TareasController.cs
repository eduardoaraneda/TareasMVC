using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TareasMVC.Entidades;
using TareasMVC.Servicios;

namespace TareasMVC.Controllers
{
    [Route("api/tareas")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicioUsuarios servicioUsuarios;
        private readonly IMapper mapper;

        public TareasController(
            ApplicationDbContext context,
            IServicioUsuarios servicioUsuarios,
            IMapper mapper)
        {
            this.context = context;
            this.servicioUsuarios = servicioUsuarios;
            this.mapper = mapper;
        }

        // ===============================
        // DTO
        // ===============================
        public class CrearTareaDTO
        {
            public int Id { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
        }

        // ===============================
        // CREAR / EDITAR
        // ===============================
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CrearTareaDTO modelo)
        {
            if (string.IsNullOrWhiteSpace(modelo.Titulo))
                return BadRequest("El título es obligatorio");

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            Tarea tarea;

            // ===============================
            // CREAR
            // ===============================
            if (modelo.Id == 0)
            {
                int ordenMayor = await context.Tareas
                    .Where(t => t.UsuarioCreacion == usuarioId)
                    .Select(t => (int?)t.Orden)
                    .MaxAsync() ?? 0;

                tarea = new Tarea
                {
                    Titulo = modelo.Titulo,
                    Descripcion = modelo.Descripcion,
                    UsuarioCreacion = usuarioId,
                    FechaCreacion = DateTime.UtcNow,
                    Orden = ordenMayor + 1
                };

                context.Add(tarea);
            }
            // ===============================
            // EDITAR
            // ===============================
            else
            {
                tarea = await context.Tareas
                    .FirstOrDefaultAsync(t =>
                        t.Id == modelo.Id &&
                        t.UsuarioCreacion == usuarioId);

                if (tarea == null)
                    return NotFound();

                tarea.Titulo = modelo.Titulo;
                tarea.Descripcion = modelo.Descripcion;
            }

            await context.SaveChangesAsync();

            return Ok(new
            {
                exito = true,
                id = tarea.Id
            });
        }

        // ===============================
        // OBTENER TAREAS
        // ===============================
        [HttpGet]
        public async Task<ActionResult<List<TareaDTO>>> Get()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();

            var tareas = await context.Tareas
                .Where(t => t.UsuarioCreacion == usuarioId)
                .OrderBy(t => t.Orden)
                .ProjectTo<TareaDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(tareas);
        }

        // ===============================
        // ELIMINAR
        // ===============================
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();

            var tarea = await context.Tareas
                .FirstOrDefaultAsync(t =>
                    t.Id == id &&
                    t.UsuarioCreacion == usuarioId);

            if (tarea == null)
                return NotFound(new { exito = false, mensaje = "Tarea no encontrada" });

            context.Tareas.Remove(tarea);
            await context.SaveChangesAsync();

            return Ok(new { exito = true });
        }

    }
}
