using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Policy = "Nivel1")]
        [HttpGet("GenerarCard")]
        public IActionResult GenerarCard()
        {
            var Datos = from t in _context.TransaccionesAcreditadas
                        where t.Acreditada.Equals("E")
                        group t by new { t.NombreArchivo, t.FechaRegistro, t.FechaIni, t.FechaFin, t.UsuarioRegistro } into g
                        select new { g.Key.NombreArchivo, g.Key.FechaRegistro, g.Key.FechaIni, g.Key.FechaFin, g.Key.UsuarioRegistro, Total = g.Count() };
            return (Datos != null) ? Ok(Datos) : NotFound();
        }

        [Authorize(Policy = "Nivel1")]
        [HttpPost("GenerarCardAcreeditadasFiltro")]
        public IActionResult GenerarCardFiltro([FromBody] FechasIniFin model)
        {
            var Datos = from t in _context.TransaccionesAcreditadas
                        where t.FechaRegistro >= model.FechaIni && t.FechaRegistro <= model.FechaFin && t.Acreditada.Equals("A")
                        group t by new { t.NombreArchivo, t.FechaRegistro, t.FechaIni, t.FechaFin, t.UsuarioRegistro } into g
                        orderby g.Key.FechaRegistro descending
                        select new { g.Key.NombreArchivo, g.Key.FechaRegistro, g.Key.FechaIni, g.Key.FechaFin, g.Key.UsuarioRegistro, Total = g.Count() };
            return (Datos != null) ? Ok(Datos) : NotFound();
        }

        [Authorize(Policy = "Nivel1")]
        [HttpGet("GenerarTransacciones/{nombreArchivo}")]
        public IActionResult GenerarTransaccionesAcreeditadas([FromRoute] string nombreArchivo)
        {
            var Datos = from t in _context.TransaccionesAcreditadas
                        join m in _context.Equipos on t.Machine_Sn equals m.serieEquipo
                        where t.NombreArchivo.Equals(nombreArchivo)
                        group t by new { t.Machine_Sn, m.IpEquipo } into g
                        select new { g.Key.Machine_Sn, g.Key.IpEquipo, Total = g.Count() };
            return (Datos != null) ? Ok(Datos) : NotFound();
        }

        [Authorize(Policy = "Nivel1")]
        [HttpGet("AprobacionTransacciones/{nombreArchivo}")]
        public IActionResult AprobarTransaccionesAcreeditadas([FromRoute] string nombreArchivo)
        {
            int registrosBorrados = 0;
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var update = _context.TransaccionesAcreditadas
                           .Where(u => u.NombreArchivo.Equals(nombreArchivo))
                           .ExecuteUpdate(u => u.SetProperty(u => u.Acreditada, "A"));

                if (update != 0)
                {
                    var listaDeDatos = _context.TransaccionesAcreditadas
                        .Where(t => t.NombreArchivo == nombreArchivo)
                        .Select(t => new
                        {
                            t.NoTransaction,
                            t.Machine_Sn
                        })
                        .ToList();
                    
                    if (listaDeDatos.Any())
                    {
                        var registrosAEliminar = _context.TransaccionesExcel
                                                .AsEnumerable()
                                                .Where(te => listaDeDatos.Any(d =>
                                                    d.NoTransaction == te.Transaccion_No &&
                                                    d.Machine_Sn == te.Machine_Sn))
                                                .ToList();
                        registrosBorrados = registrosAEliminar.Count;
                        _context.TransaccionesExcel.RemoveRange(registrosAEliminar);
                    }
                }
                if (_context.SaveChanges() > 0)
                {
                    transaction.CommitAsync();
                    return Ok();
                }
            }
            catch (Exception)
            {
                transaction.RollbackAsync();
            }
            transaction.RollbackAsync();
            return BadRequest();
        }

        [Authorize(Policy = "Nivel1")]
        [HttpDelete("BorrarTransacciones/{nombreArchivo}")]
        public IActionResult EliminarTransaccionesAcreeditadasAsync([FromRoute] string nombreArchivo)
        {
            var delete = _context.TransaccionesAcreditadas
                            .Where(b => b.NombreArchivo.Equals(nombreArchivo))
                            .ExecuteDelete();
            return (delete != 0) ? Ok() : BadRequest();
        }
    }
}