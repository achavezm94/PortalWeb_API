using Microsoft.AspNetCore.Http;
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

        [HttpPost("Conteo")]
        public async Task<IEnumerable<SP_EquiposNoTransaccionesResult>> ResultadoConteoTransacciones([FromBody] FechasIniFin filtroFechas)
        {
            return await _context.GetProcedures().SP_EquiposNoTransaccionesAsync(filtroFechas.fechaIni, filtroFechas.fechaFin);
        }
    }
}
