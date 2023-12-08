using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    [Route("api/FiltroFechas")]
    [ApiController]
    public class FiltrosPorFechaTransaccionesController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public FiltrosPorFechaTransaccionesController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpGet("Filtrar")]
        public async Task<IEnumerable<SP_FiltroPorFechaTransaccionesResult>> ResultadoFiltroFechasTransacciones([FromBody] ModeloFiltroFechasTransacciones filtroFechas)
        {
            return await _context.GetProcedures().SP_FiltroPorFechaTransaccionesAsync(filtroFechas.Tipo, filtroFechas.Machine_Sn, filtroFechas.FechaInicio, filtroFechas.FechaFin);
        }
    }
}
