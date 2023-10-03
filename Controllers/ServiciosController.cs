using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    [Route("api/Servicios")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private readonly PortalWebContext _context;
        private readonly IHubContext<PingHubEquipos> _pinghub;
        public ServiciosController(PortalWebContext context, IHubContext<PingHubEquipos> pingHub)
        {
            _context = context;
            _pinghub = pingHub;
        }

        [HttpGet]
        [Route("ObtenerIP")]
        public async Task<IActionResult> ObtenerIP()
        {
            if (ModelState.IsValid)
            {
                var ip = await _context.Equipos.ToListAsync();

                if (ip == null)
                {
                    return NotFound();
                }
                return Ok(ip);
            }
            else
            {
                return BadRequest("ERROR");
            }
        }

        [HttpPost]
        [Route("ActualizarEquipoIp")]
        public async Task<IActionResult> ActualizarEquipo([FromBody] List<EquiposDto> model)
        {
            foreach (var equipo in model)
            {
                var res = _context.Equipos.FirstOrDefault(x => x.IpEquipo == equipo.IpEquipo);
                if (res != null)
                {
                    if (equipo.EstadoPing == 0)
                    {
                        if (equipo.EstadoPing != res.EstadoPing)
                        {
                            res.EstadoPing = equipo.EstadoPing;
                            res.Active = "A";
                        }
                    }
                    else if (equipo.EstadoPing == 1)
                    {
                        res.EstadoPing = equipo.EstadoPing;
                        res.TiempoSincronizacion = DateTime.Now;
                        res.Active = "A";
                    }
                    _context.Entry(res).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    await _pinghub.Clients.All.SendAsync("SendPingEquipo", model);
                }
            }
            return Ok("actualizado");
        }
    }
}
