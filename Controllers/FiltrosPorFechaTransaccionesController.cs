using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion transacciones.
    /// </summary>
    [Route("api/FiltroFechas")]
    [ApiController]
    public class FiltrosPorFechaTransaccionesController : ControllerBase
    {
        private readonly PortalWebContext _context;

        /// <summary>
        /// Extraer el context de EF.
        /// </summary>
        public FiltrosPorFechaTransaccionesController(PortalWebContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene las transacciones por filtro de fecha de todos los equipos.
        /// </summary>
        /// <returns>Lista de todas las transacciones de todos los equipos por filtro de fecha.</returns>
        /// <response code="200">Devuelve todas las transacciones de todos los equipos por filtro de fecha.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
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

        /// <summary>
        /// Obtiene los datos para generar el reporte de consolidado por localidad para generar el excel.
        /// </summary>
        /// <returns>Lista de datos para hacer consolidado por localidad.</returns>
        /// <response code="200">Devuelve todos los datos para realizar pdf de consolidado.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
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