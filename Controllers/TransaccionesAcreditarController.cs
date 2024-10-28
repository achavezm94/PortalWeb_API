using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    [Route("api/TransAcreditada")]
    [ApiController]
    public class TransaccionesAcreditarController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public TransaccionesAcreditarController(PortalWebContext context)
        {
            _context = context;
        }

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