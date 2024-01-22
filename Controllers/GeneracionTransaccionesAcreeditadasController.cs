using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;
using System.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/Acreeditacion")]
    [ApiController]
    public class GeneracionTransaccionesAcreeditadasController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public GeneracionTransaccionesAcreeditadasController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GenerarCard")]
        public IActionResult GenerarCard()
        {
            var Datos = from t in _context.TransaccionesAcreditadas
                        where t.Acreditada.Equals("E")
                        group t by new { t.NombreArchivo, t.FechaRegistro, t.FechaIni, t.FechaFin, t.UsuarioRegistro } into g
                        select new { g.Key.NombreArchivo, g.Key.FechaRegistro, g.Key.FechaIni, g.Key.FechaFin, g.Key.UsuarioRegistro, Total = g.Count() };
            return (Datos != null) ? Ok(Datos) : NotFound("No se pudo encontrar");
        }

        [HttpPost("GenerarCardAcreeditadasFiltro")]
        public IActionResult GenerarCardFiltro([FromBody] FechasIniFin model)
        {
            var Datos = from t in _context.TransaccionesAcreditadas
                        where t.FechaRegistro >= model.FechaIni && t.FechaRegistro <= model.FechaFin && t.Acreditada.Equals("A")
                        group t by new { t.NombreArchivo, t.FechaRegistro, t.FechaIni, t.FechaFin, t.UsuarioRegistro } into g
                        select new { g.Key.NombreArchivo, g.Key.FechaRegistro, g.Key.FechaIni, g.Key.FechaFin, g.Key.UsuarioRegistro, Total = g.Count() };
            return (Datos != null) ? Ok(Datos) : NotFound("No se pudo encontrar");
        }

        [HttpGet]
        [Route("GenerarTransacciones/{nombreArchivo}")]
        public IActionResult GenerarTransaccionesAcreeditadas([FromRoute] string nombreArchivo)
        {
            var Datos = from t in _context.TransaccionesAcreditadas
                        join m in _context.Equipos on t.MachineSn equals m.SerieEquipo
                        where t.NombreArchivo.Equals(nombreArchivo)
                        group t by new { t.MachineSn, m.IpEquipo } into g
                        select new { g.Key.MachineSn, g.Key.IpEquipo, Total = g.Count() };
            return (Datos != null) ? Ok(Datos) : NotFound("No se pudo encontrar");
        }

        [HttpGet]
        [Route("AprobacionTransacciones/{nombreArchivo}")]
        public IActionResult AprobarTransaccionesAcreeditadasAsync([FromRoute] string nombreArchivo)
        {
            var update = _context.TransaccionesAcreditadas
                            .Where(u => u.NombreArchivo.Equals(nombreArchivo))
                            .ExecuteUpdate(u => u.SetProperty(u => u.Acreditada, "A"));
            return (update != 0) ? Ok() : BadRequest();
        }


        [HttpDelete]
        [Route("BorrarTransacciones/{nombreArchivo}")]
        public IActionResult EliminarTransaccionesAcreeditadasAsync([FromRoute] string nombreArchivo)
        {
            var delete = _context.TransaccionesAcreditadas
                            .Where(b => b.NombreArchivo.Equals(nombreArchivo))
                            .ExecuteDelete();
            return (delete != 0) ? Ok() : BadRequest();
        }
    }
}