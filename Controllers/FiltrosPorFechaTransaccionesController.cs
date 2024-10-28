using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Policy = "Transaccional")]
        [HttpPost("Filtrar")]
        public async Task<IEnumerable<object>> ResultadoFiltroFechasTransacciones([FromBody] ModeloFiltroFechasTransacciones filtroFechas) 
        {
            //return await _context.GetProcedures().SP_FiltroPorFechaTransaccionesAsync(filtroFechas.Tipo, filtroFechas.Machine_Sn, filtroFechas.FechaInicio, filtroFechas.FechaFin);
            
            if (filtroFechas.Tipo == 2)
            {                
                var fechaAtras = (filtroFechas.FechaInicio.AddDays(-5));            
                var resultado = _context.TransaccionesExcel
                                       .Where(te => te.Machine_Sn == filtroFechas.Machine_Sn
                                                 && te.FechaTransaccion >= fechaAtras
                                                 && te.FechaTransaccion <= filtroFechas.FechaFin)
                                       .OrderByDescending(te => te.FechaTransaccion)
                                       .ToList();
                return resultado;
            }
            else
            {
                return await _context.GetProcedures().SP_FiltroPorFechaTransaccionesAsync(filtroFechas.Tipo, filtroFechas.Machine_Sn, filtroFechas.FechaInicio, filtroFechas.FechaFin);
            }
        }

        [Authorize(Policy = "Nivel1")]
        [HttpPost("Consolidado")]
        public List<SP_ConsolidadoLocalidadResult> ResultadoConsolidado([FromBody] ModeloConsolidado modelo)
        {
            if (modelo.Equipos.Any())
            {
                var resultados = modelo.Equipos.SelectMany(equipo =>
                    _context.GetProcedures().SP_ConsolidadoLocalidadAsync(modelo.Tipo, equipo, modelo.FechaIni, modelo.FechaFin).Result
                ).OrderBy(x => x.NombreTienda).ToList();

                return resultados;
            }
            else
            {
                return new List<SP_ConsolidadoLocalidadResult>();
            }
        }
    }
}