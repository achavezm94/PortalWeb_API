using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion modulo de consolidado de cajas generacion de txt.
    /// </summary>
    [Route("api/ConsolidadoCajas")]
    [ApiController]
    public class ConsolidadoCajasController : Controller
    {
        private readonly PortalWebContext _context;

        /// <summary>
        /// Extraer el context de EF.
        /// </summary>
        public ConsolidadoCajasController(PortalWebContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene los datos para generar vista para volver a descargar el txt.
        /// </summary>
        /// <returns>Lista de los datos para generar vista para volver a descargar los txt de consolidado de cajas.</returns>
        /// <response code="200">Devuelve todos los datos para generar vista para volver a descargar los txt de consolidado de cajas.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPost("FiltroConsolidado")]
        public IActionResult FiltroConsolidados([FromBody] FechasIniFin model)
        {
            try
            {
                string fechaIniStr = model.FechaIni.ToString("yyyyMMdd");
                string fechaFinStr = model.FechaFin.ToString("yyyyMMdd");
                var Datos = _context.ConsolidadoSaldoCajas
                    .Where(s => s.Fecha.CompareTo(fechaIniStr) >= 0 && s.Fecha.CompareTo(fechaFinStr) <= 0)
                    .GroupBy(c => c.NomArchivo)
                    .Select(g => new
                    {
                        NomArchivo = g.Key,
                        CantidadEquipos = g.Count(),
                        g.First().Fecha,
                        g.First().Generado
                    })
                    .OrderByDescending(te => te.Fecha)
                    .ToList();
                return (Datos != null) ? Ok(Datos) : NotFound();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Obtiene los datos para generar los txt de consolidado de cada caja.
        /// </summary>
        /// <returns>Lista de los datos para descargar los txt de consolidado de cajas.</returns>
        /// <response code="200">Devuelve todos los datos para descargar los txt de consolidado de cajas.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpGet("ObtenerConsolidado/{nombreArchivo}")]
        public IActionResult ObtenerConsolidados([FromRoute] string nombreArchivo)
        {
            try
            {
                var Datos = _context.ConsolidadoSaldoCajas
                    .Where(s => s.NomArchivo == nombreArchivo)
                    .Select(s => new
                    {
                        s.Fecha,
                        CodLocalidad = s.CodLocalidad.Trim(),
                        s.Localidad,
                        s.CodCliente,
                        s.NomCliente,
                        s.CodEstablecimiento,
                        s.NomEstablecimiento,
                        s.Equipo,
                        s.SaldoInicial,
                        s.DepositosProcesados,
                        s.Retiros,
                        s.SaldoFinal
                    });
                return (Datos != null) ? Ok(Datos) : NotFound();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Genera los datos para el txt consolidado de cajas de forma manual.
        /// </summary>
        /// <returns>Ejecuta el sp que genera los datos para el txt consolidado de cajas de forma manual.</returns>
        /// <response code="200">Ejecuta el sp que genera los datos para el txt consolidado de cajas de forma manual.</response>
        /// <response code="204">Ejecuta el sp pero no se actualiza ningun dato debido a la falta de transacciones.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpGet("GenerarConsolidadoManual/{tipo}")]
        public async Task<IActionResult> GenerarConsolidadosManualAsync([FromRoute] string tipo)
        {
            try
            {
                return Ok(await _context.GetProcedures().SP_ConsolidadoCajasAsync(tipo.ToUpper()));
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }
    }
}