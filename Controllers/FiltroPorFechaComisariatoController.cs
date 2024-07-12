using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Methods_Token;
using PortalWeb_API.Models;
using System.Configuration;

namespace PortalWeb_API.Controllers
{
    [Route("api/Comisariato")]
    [ApiController]
    public class FiltroPorFechaComisariatoController : ControllerBase
    {
        private readonly PortalWebContext _context;
        private readonly TokenValidator _tokenValidator;
        private readonly IConfiguration _configuration;

        public FiltroPorFechaComisariatoController(PortalWebContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _tokenValidator = new TokenValidator(configuration);
        }

        [Authorize(Policy = "Comi")]
        [HttpPost("Filtrar")]
        public async Task<IActionResult> ResultadoFiltroFechasTransaccionesAsync([FromBody] ModeloFiltroComisariato filtroFechas)
        {
            bool esValido = _tokenValidator.GetJwtFromRequest(Request);
            //bool esValido = true; // Simulación de validación de token
            if (esValido)
            {
                if (filtroFechas.FechaInicio is not null && filtroFechas.FechaFin is not null)
                {
                    if (filtroFechas.FechaInicio < filtroFechas.FechaFin)
                    {
                        // Comprobar si la diferencia es menor o igual a 5 días
                        if (Math.Abs(((TimeSpan)(filtroFechas.FechaInicio - filtroFechas.FechaFin)).TotalDays) <= 5)
                        {
                            return Ok(await _context.GetProcedures().SP_FiltroPorFechaComisariatoAsync(filtroFechas.Id_Local, filtroFechas.FechaInicio, filtroFechas.FechaFin));
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
            else
            {
                return BadRequest("No es válido el Token"); 
            }
        }        
    }
}
