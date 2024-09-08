using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Methods_Token;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    [Route("api/Comisariato")]
    [ApiController]
    public class FiltroPorFechaComisariatoController : ControllerBase
    {
        private readonly PortalWebContext _context;
        private readonly TokenValidator _tokenValidator;

        public FiltroPorFechaComisariatoController(PortalWebContext context, IConfiguration configuration)
        {
            _context = context;
            _tokenValidator = new TokenValidator(configuration);
        }

        [Authorize(Policy = "Comi")]
        [HttpPost("Filtrar")]
        public async Task<IActionResult> ResultadoFiltroFechasTransaccionesAsync([FromBody] ModeloFiltroComisariato filtroFechas)
        {
            bool esValido = _tokenValidator.GetJwtFromRequest(Request);
            //bool esValido = true;
            if (!esValido)
            {
                return BadRequest("No es válido el Token");
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
    }
}