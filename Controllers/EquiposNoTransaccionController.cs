using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    [Route("api/EquiposNoTransaccion")]
    [ApiController]
    public class EquiposNoTransaccionController : ControllerBase
    {
        private readonly PortalWebContext _context;
        public EquiposNoTransaccionController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpPost("Conteo/{opcion}")]
        public async Task<IEnumerable<SP_EquiposNoTransaccionesResult>> ResultadoConteoTransacciones([FromBody] FechasIniFin filtroFechas, [FromRoute] int opcion)
        {
            return await _context.GetProcedures().SP_EquiposNoTransaccionesAsync(filtroFechas.FechaIni, filtroFechas.FechaFin, opcion);
        }
    }
}
