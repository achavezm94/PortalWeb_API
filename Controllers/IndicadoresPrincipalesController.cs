using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion de indicadores para pantalla Home.
    /// </summary>
    [Route("api/Indicadores")]
    [ApiController]
    public class IndicadoresPrincipalesController : ControllerBase
    {
        private readonly PortalWebContext _context;

        /// <summary>
        /// Extraer el context de EF.
        /// </summary>
        public IndicadoresPrincipalesController(PortalWebContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene los datos para mostrar en la pantalla Home.
        /// </summary>
        /// <returns>Lista de datos para poder mostrar en la pantalla principal.</returns>
        /// <response code="200">Devuelve la lista de datos a mostrar en la pantalla principal.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Monitor")]
        [HttpGet("ObtenerIndicadoresHome/{opcion}")]
        public IActionResult ObtenerHome(int opcion)
        {
            if (opcion == 1)
            {
                var NoClientes = _context.Clientes.AsNoTracking().Count(e => e.Active != "F");
                var NoTiendas = _context.Tiendas.AsNoTracking().Count(e => e.Active != "F");
                var NoEquipos = _context.Equipos.AsNoTracking().Count(e => e.active != "F");
                var NoUsuarios = _context.Usuarios_Portal.AsNoTracking().Count(e => e.Active != "F");
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