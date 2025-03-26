using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;
using System.Data;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion modulo de acreditación.
    /// </summary>
    [Route("api/Acreeditacion")]
    [ApiController]
    public class GeneracionTransaccionesAcreeditadasController : ControllerBase
    {
        private readonly PortalWebContext _context;

        /// <summary>
        /// Extraer el context de EF.
        /// </summary>
        public GeneracionTransaccionesAcreeditadasController(PortalWebContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene los datos para generar card para aprobar transacciones acreditadas.
        /// </summary>
        /// <returns>Lista de los datos para generar vista para aprobación de transacciones acreditadas.</returns>
        /// <response code="200">Devuelve todos los datos para generar vista para aprobación de transacciones acreditadas.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpGet("GenerarCard")]
        public IActionResult GenerarCard()
        {
            try
            {
                var Datos = from t in _context.TransaccionesAcreditadas.AsNoTracking()
                            where t.Acreditada.Equals("E")
                            group t by new { t.NombreArchivo, t.FechaRegistro, t.FechaIni, t.FechaFin, t.NombreConsolidado } into g
                            select new { g.Key.NombreArchivo, g.Key.FechaRegistro, g.Key.FechaIni, g.Key.FechaFin, g.Key.NombreConsolidado, Total = g.Count() };
                return (Datos != null) ? Ok(Datos) : NotFound();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Obtiene los datos para generar card para aprobar transacciones acreditadas segun filtros de fechas.
        /// </summary>
        /// <returns>Lista de los datos para generar vista para aprobación de transacciones acreditadas segun filtros de fechas.</returns>
        /// <response code="200">Devuelve todos los datos para generar vista para aprobación de transacciones acreditadas segun filtro de fechas.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPost("GenerarCardAcreeditadasFiltro")]
        public IActionResult GenerarCardFiltro([FromBody] FechasIniFin model)
        {
            try
            {
                var Datos = from t in _context.TransaccionesAcreditadas.AsNoTracking()
                            where t.FechaRegistro >= model.FechaIni && t.FechaRegistro <= model.FechaFin && t.Acreditada.Equals("A")
                            group t by new { t.NombreArchivo, t.FechaRegistro, t.FechaIni, t.FechaFin, t.NombreConsolidado } into g
                            orderby g.Key.FechaRegistro descending
                            select new { g.Key.NombreArchivo, g.Key.FechaRegistro, g.Key.FechaIni, g.Key.FechaFin, g.Key.NombreConsolidado, Total = g.Count() };
                return (Datos != null) ? Ok(Datos) : NotFound();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Obtiene los datos para obtener el numero de transacciones que ya han sido acreditadas segun el nombre del archivo que se registro.
        /// </summary>
        /// <returns>Lista de los datos de transacciones acreditadas filtro nombre del archivo registrado.</returns>
        /// <response code="200">Devuelve los datos de transacciones acreditadas segun el filtro del nombre del archivo.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpGet("GenerarTransacciones/{nombreArchivo}")]
        public IActionResult GenerarTransaccionesAcreeditadas([FromRoute] string nombreArchivo)
        {
            try
            {
                var Datos = from t in _context.TransaccionesAcreditadas.AsNoTracking()
                            join m in _context.Equipos.AsNoTracking() on t.Machine_Sn equals m.serieEquipo
                            where t.NombreArchivo.Equals(nombreArchivo)
                            group t by new { t.Machine_Sn, m.IpEquipo } into g
                            select new { g.Key.Machine_Sn, g.Key.IpEquipo, Total = g.Count() };
                return (Datos != null) ? Ok(Datos) : NotFound();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Aprobar todas las transacciones escogidas.
        /// </summary>        
        /// <response code="200">Se registro la aprobacion de todas las transacciones.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpGet("AprobacionTransacciones/{nombreArchivo}")]
        public IActionResult AprobarTransaccionesAcreeditadas([FromRoute] string nombreArchivo)
        {
            try
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
                            }).ToList();

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
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Borrar una pre acreditacion.
        /// </summary>
        /// <response code="200">Borro correctamente la pre acreditacion.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpDelete("BorrarTransacciones/{nombreArchivo}")]
        public async Task<IActionResult> EliminarTransaccionesAcreeditadasAsync([FromRoute] string nombreArchivo)
        {
            try
            {
                // Validar parámetro de entrada
                if (string.IsNullOrEmpty(nombreArchivo))
                {
                    return BadRequest("Nombre de archivo inválido");
                }

                var fecha = await _context.TransaccionesAcreditadas
                 .Where(b => b.NombreArchivo == nombreArchivo)
                 .Select(b => b.FechaRegistro)
                 .FirstOrDefaultAsync();

                if (fecha == default)
                {
                    return NotFound("No se encontraron transacciones con el nombre de archivo especificado");
                }

                // Eliminar registros
                var registrosEliminados = await _context.TransaccionesAcreditadas
                    .Where(b => b.NombreArchivo.Equals(nombreArchivo))
                    .ExecuteDeleteAsync();

                if (registrosEliminados > 0)
                {
                    var fechaFormateada = fecha.ToString("yyyyMMdd");
                    var numeroCorte = await _context.NumeroCortesDias
                        .FirstOrDefaultAsync(b => b.Fecha.Equals(fechaFormateada));

                    if (numeroCorte != null)
                    {
                        numeroCorte.NumCorte = Math.Max(0, numeroCorte.NumCorte - 1);
                        await _context.SaveChangesAsync();
                    }
                }
                return Ok(new { RegistrosEliminados = registrosEliminados });
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }
    }
}