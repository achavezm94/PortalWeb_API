`using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Methods_Token;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion requisito Comisariato.
    /// </summary>
    [Route("api/Comisariato")]
    [ApiController]
    public class FiltroPorFechaComisariatoController : ControllerBase
    {
        private readonly PortalWebContext _context;
        private readonly TokenValidator _tokenValidator;

        /// <summary>
        /// Extraer el context de EF y validador de token.
        /// </summary>
        public FiltroPorFechaComisariatoController(PortalWebContext context, IConfiguration configuration)
        {
            _context = context;
            _tokenValidator = new TokenValidator(configuration);
        }

        /// <summary>
        /// Obtiene las transacciones por filtro de fecha para cliente Comisariato.
        /// </summary>
        /// <returns>Lista de todas las transacciones del cliente Comisariato filtrado por fechas.</returns>
        /// <response code="200">Devuelve todas las transacciones del cliente Comisariato filtrado por fechas.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Restricciones de horario (00:00 - 05:00).</response> 
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Comi")]
        [HttpPost("Filtrar")]
        public async Task<IActionResult> ResultadoFiltroFechasTransaccionesAsync([FromBody] ModeloFiltroComisariato filtroFechas)
        {
            // Verificar horario
            if (!IsValidTime())
            {
                return BadRequest("El token solo puede utilizarse de 00:00 a 05:00.");
            }           
            bool esValido = _tokenValidator.GetJwtFromRequest(Request);
            //bool esValido = true;
            if (!esValido)
            {
                return BadRequest("No es válido el Token");
            }
            if (filtroFechas.Id_Local != "100")
            {
                return BadRequest("Piloto solo se puede acceder al establecimiento FC009");
            }
            if (filtroFechas.FechaInicio is null || filtroFechas.FechaFin is null)
            {
                return BadRequest("No puede enviar las fechas vacías");
            }
            if (filtroFechas.FechaInicio >= filtroFechas.FechaFin)
            {
                return BadRequest("La fecha de inicio no puede ser mayor o igual a la fecha de fin");
            }
            if (Math.Abs(((TimeSpan)(filtroFechas.FechaInicio - filtroFechas.FechaFin)).TotalDays) > 5)
            {
                return BadRequest("La diferencia entre las fechas no puede ser mayor a 5 días");
            }
            return Ok(await _context.GetProcedures().SP_FiltroPorFechaComisariatoAsync(filtroFechas.Id_Local, filtroFechas.FechaInicio, filtroFechas.FechaFin));
        }

        private static bool IsValidTime()
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            var horaActual = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);

            var start = new TimeSpan(0, 0, 0); // 00:00
            var end = new TimeSpan(5, 0, 0);   // 05:00

            return horaActual.TimeOfDay >= start && horaActual.TimeOfDay <= end;
        }
    }
}