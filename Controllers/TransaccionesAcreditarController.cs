using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            try
            {
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstZone);
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                foreach (var item in model)
                {
                    item.FechaRegistro = cstTime;
                }
                await _context.TransaccionesAcreditadas.AddRangeAsync(model).ConfigureAwait(false);
                if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
                {
                    string fechaHoy = cstTime.ToString("yyyyMMdd");
                    var ultimoRegistro = await _context.NumeroCortesDias
                                           .OrderByDescending(x => x.id)
                                           .FirstOrDefaultAsync()
                                           .ConfigureAwait(false);
                    if (ultimoRegistro == null || fechaHoy != ultimoRegistro.Fecha)
                    {
                        var nuevoCorte = new NumeroCortesDias
                        {
                            Fecha = fechaHoy,
                            NumCorte = 1
                        };
                        await _context.NumeroCortesDias.AddAsync(nuevoCorte).ConfigureAwait(false);
                    }
                    else
                    {
                        ultimoRegistro.NumCorte += 1;
                    }
                    return (await _context.SaveChangesAsync().ConfigureAwait(false) > 0) ? Ok() : BadRequest();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }
    }
}