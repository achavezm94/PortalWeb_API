using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion Acreditar Transacciones.
    /// </summary>
    [Route("api/TransAcreditada")]
    [ApiController]
    public class TransaccionesAcreditarController : ControllerBase
    {
        private readonly PortalWebContext _context;

        /// <summary>
        /// Extraer el context de EF.
        /// </summary>
        public TransaccionesAcreditarController(PortalWebContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Acreditación de Transacciones en el sistema.
        /// </summary>        
        /// <response code="200">Se acredito correctamente todas las transacciones.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPost("GuardarTransacciones")]
        public async Task<IActionResult> GuardarTransacciones([FromBody] List<TransaccionesAcreditadas> model)
        {
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            DateTime timeUtc = DateTime.UtcNow;
            DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
            if (ModelState.IsValid)
            {
                foreach (var item in model)
                {
                    item.FechaRegistro = cstTime;
                }
                await _context.TransaccionesAcreditadas.AddRangeAsync(model).ConfigureAwait(false);
                return (await _context.SaveChangesAsync().ConfigureAwait(false) > 0) ? Ok() : BadRequest();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}