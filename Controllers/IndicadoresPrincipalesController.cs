using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/Indicadores")]
    [ApiController]
    public class IndicadoresPrincipalesController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public IndicadoresPrincipalesController(PortalWebContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Monitor")]
        [HttpGet("ObtenerIndicadoresHome/{opcion}")]
        public IActionResult ObtenerHome(int opcion)
        {
            if (opcion == 1)
            {
                var NoClientes = _context.Clientes.Count(e => e.Active != "F");
                var NoTiendas = _context.Tiendas.Count(e => e.Active != "F");
                var NoEquipos = _context.Equipos.Count(e => e.active != "F");
                var NoUsuarios = _context.Usuarios_Portal.Count(e => e.Active != "F");
                var Datos = new List<object>
                {
                    new { tipo = "clientes", Cantidad = NoClientes },
                    new { tipo = "tiendas", Cantidad = NoTiendas },
                    new { tipo = "equipos", Cantidad = NoEquipos },
                    new { tipo = "usuarios", Cantidad = NoUsuarios }
                };
                return Ok(Datos);
            }
            else
            {
                return Ok(null);
            }
        }
    }
}