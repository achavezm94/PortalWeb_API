using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using System.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/UsuarioTemporal")]
    [ApiController]
    public class ObtenerUsuarioTemporalController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public ObtenerUsuarioTemporalController(PortalWebContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Nivel2")]
        [HttpGet("Usuario/{ip}")]
        public IActionResult ObtenerUsuarioTemporal(string ip)
        {
            var Datos = from ut in _context.UsuariosTemporales
                        where ut.IpMachineSolicitud.Equals(ip) && ut.Active.Equals("A")
                        select new { ut.id, ut.Usuario, ut.IpMachineSolicitud, ut.fecrea };
            return (Datos != null) ? Ok(Datos) : NotFound();
        }

        [Authorize(Policy = "Nivel1")]
        [HttpGet("UsuarioDelete/{id}")]
        public IActionResult BorrarUsuarioTemporal([FromRoute] int id)
        {
            var update = _context.UsuariosTemporales
                           .Where(u => u.id.Equals(id))
                           .ExecuteUpdate(u => u
                           .SetProperty(u => u.Active, "F"));
            return (update != 0) ? Ok() : BadRequest();
        }
    }
}