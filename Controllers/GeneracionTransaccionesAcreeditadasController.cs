using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
        [Route("GenerarCard/{Opcion}")]
        public IActionResult GenerarCard([FromRoute] string opcion)
        {
            var Datos = from t in _context.TransaccionesAcreditadas
                        where t.Acreditada.Equals(opcion)
                        group t by new { t.NombreArchivo, t.FechaRegistro, t.FechaIni, t.FechaFin, t.UsuarioRegistro, t.NoTransaction } into g
                        select new { g.Key.NombreArchivo, g.Key.FechaRegistro, g.Key.FechaIni, g.Key.FechaFin, g.Key.UsuarioRegistro, Total = g.Count() };
            return (Datos != null) ? Ok(Datos) : NotFound("No se pudo encontrar");
        }

        [HttpGet]
        [Route("GenerarTransacciones/{nombreArchivo}")]
        public IActionResult GenerarTransaccionesAcreeditadas([FromRoute] string nombreArchivo)
        {
            var Datos = from t in _context.TransaccionesAcreditadas
                        where t.NombreArchivo.Equals(nombreArchivo)
                        group t by new { t.MachineSn } into g
                        select new { g.Key.MachineSn, Total = g.Count() };
            return (Datos != null) ? Ok(Datos) : NotFound("No se pudo encontrar");
        }

        [HttpGet]
        [Route("AprobacionTransacciones/{nombreArchivo}")]
        public IActionResult AprobarTransaccionesAcreeditadasAsync([FromRoute] string nombreArchivo)
        {
            var update = _context.TransaccionesAcreditadas
                            .Where(u => u.NombreArchivo.Equals(nombreArchivo))
                            .ExecuteUpdate(u => u.SetProperty(u => u.Acreditada, "A"));
            return (update != 0) ? Ok("Se actualizo") : BadRequest("No se pudo actualizar");
        }


        [HttpDelete]
        [Route("BorrarTransacciones/{nombreArchivo}")]
        public IActionResult EliminarTransaccionesAcreeditadasAsync([FromRoute] string nombreArchivo)
        {
            var delete = _context.TransaccionesAcreditadas
                            .Where(b => b.NombreArchivo.Equals(nombreArchivo))
                            .ExecuteDelete();
            return (delete != 0) ? Ok("Se borro") : BadRequest("No se pudo eliminar");
        }
    }
}
