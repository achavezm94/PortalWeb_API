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
        
        [HttpPost("Filtrar")]
        public async Task<IEnumerable<SP_FiltroPorFechaTransaccionesResult>> ResultadoFiltroFechasTransacciones([FromBody] ModeloFiltroFechasTransacciones filtroFechas)
        {
            return await _context.GetProcedures().SP_FiltroPorFechaTransaccionesAsync(filtroFechas.Tipo, filtroFechas.Machine_Sn, filtroFechas.FechaInicio, filtroFechas.FechaFin);
        }

        [HttpPost("Consolidado")]
        public async Task<List<SP_ConsolidadoLocalidadResult>> ResultadoConsolidado([FromBody] ModeloConsolidado modelo)
        {
            List<SP_ConsolidadoLocalidadResult> resultados = new();
            if (modelo.equipos is not null)
            {
                foreach (var equipo in modelo.equipos)
                {
                    var resultado = await _context.GetProcedures().SP_ConsolidadoLocalidadAsync(modelo.Tipo, equipo, modelo.FechaIni, modelo.FechaFin);
                    resultados.AddRange(resultado);
                }
                resultados = resultados.OrderBy(x => x.NombreTienda).ToList();
                return resultados;
            }
            else
            {
                return resultados;
            }
        }
    }
}