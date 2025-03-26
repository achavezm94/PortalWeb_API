using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Methods;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion cuadre y conteo de transacciones.
    /// </summary>
    [Route("api/EquiposNoTransaccion")]
    [ApiController]
    public class EquiposNoTransaccionController : ControllerBase
    {
        private readonly PortalWebContext _context;

        /// <summary>
        /// Extraer el context de EF.
        /// </summary>
        public EquiposNoTransaccionController(PortalWebContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene los datos de un equipo si esta cuadrado.
        /// </summary>
        /// <returns>Lista de los datos necesarios para saber si esta cuadrado el equipo.</returns>
        /// <response code="200">Devuelve la lista de datos de cuadre de un equipo.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpGet("Cuadre/{machine_Sn}")]
        public IActionResult ResultadoCuadre([FromRoute] string machine_Sn)
        {
            try
            {
                MetodosTotales metodosTotales = new(_context);
                TotalTodoTransacciones todasTransacciones = metodosTotales.TotalesTodasTransacciones(machine_Sn);
                TotalUltimaTransaccion[] ultimaTransaccion = metodosTotales.TotalesUltimaTransaccion(machine_Sn);
                int resultado = (todasTransacciones.Total == ultimaTransaccion[0].TotalMont) ? 0 : // 0 = iguales
                    (todasTransacciones.Total > ultimaTransaccion[0].TotalMont) ? 1 : 2; // 1 = duplicadas, 2 = faltante
                double? totalcuadre = (double?)_context.TotalesEquipos.Where(d => d.Equipo == machine_Sn).First().TotalCuadreEquipo;
                if (totalcuadre == 0)
                {
                    return Ok(new { machine_Sn, diferencia = (todasTransacciones.Total - ultimaTransaccion[0].TotalMont), resultado });
                }
                else
                {
                    return Ok(new { machine_Sn, diferencia = (ultimaTransaccion[0].TotalMont - todasTransacciones.Total) + totalcuadre, resultado = 2 });// revisar aqui
                }
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Obtiene el numero de transacciones que tiene todos los equipos.
        /// </summary>
        /// <returns>Lista de los conteo de transacciones de todos los equipos.</returns>
        /// <response code="200">Devuelve la lista de conteo de transacciones de todos los equipos.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPost("Conteo/{opcion}")]
        public async Task<IEnumerable<SP_EquiposNoTransaccionesResult>> ResultadoConteoTransacciones([FromBody] FechasIniFin filtroFechas, [FromRoute] int opcion) =>
            await _context.GetProcedures().SP_EquiposNoTransaccionesAsync(filtroFechas.FechaIni, filtroFechas.FechaFin, opcion);
    }
}