using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    [Route("api/Comisariato")]
    [ApiController]
    public class FiltroPorFechaComisariatoController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public FiltroPorFechaComisariatoController(PortalWebContext context)
        {
            _context = context;
        }
        //[Authorize(Policy = "Comi")]
        [HttpPost("Filtrar")]
        public async Task<IActionResult> ResultadoFiltroFechasTransaccionesAsync([FromBody] ModeloFiltroComisariato filtroFechas)
        {
            if (filtroFechas.FechaInicio is not null && filtroFechas.FechaFin is not null)
            {
                if (filtroFechas.FechaInicio < filtroFechas.FechaFin)
                {
                    // Comprobar si la diferencia es menor o igual a 5 días
                    if (Math.Abs(((TimeSpan)(filtroFechas.FechaInicio - filtroFechas.FechaFin)).TotalDays) <= 5)
                    {
                        return Ok(await _context.GetProcedures().SP_FiltroPorFechaComisariatoAsync(filtroFechas.Machine_Sn, filtroFechas.FechaInicio, filtroFechas.FechaFin));
                    }
                    else
                    {
                        return BadRequest("La diferencia entre las fechas no puede ser mayor a 5 días");
                    }
                }
                else
                {
                    return BadRequest("La fecha de inicio no puede ser mayor a la fecha de fin");
                }
            }
            else
            {
                return BadRequest("No puede enviar las fechas vacías");
            }       
        }

        //Comentar si es produccion
        [HttpGet("Establecimientos")]
        public IActionResult ObtenerEstablecimiento()
        {
            var Datos = _context.DatosPersonales
                                .Where(dp => dp.Cedula.Length <= 3)
                                .Where(dp => dp.Cedula != "000" && dp.Cedula != "0000")
                                .GroupBy(dp => new { dp.Cedula, dp.Nombres })
                                .Select(g => new { g.Key.Cedula, g.Key.Nombres })
                                .ToList();
            return (Datos != null) ? Ok(Datos) : NotFound("No existe cuenta bancaria");
        }
        //
    }
}
