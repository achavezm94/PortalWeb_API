using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;
using System.Data;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion de datos Master.
    /// </summary>
    [Route("api/DataMaster")]
    [ApiController]
    public class MasterTableController : ControllerBase
    {
        private readonly PortalWebContext _context;

        /// <summary>
        /// Extraer el context de EF.
        /// </summary>
        public MasterTableController(PortalWebContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene datos necesarios Master.
        /// </summary>
        /// <returns>Lista de datos de la tabla Master.</returns>
        /// <response code="200">Devuelve la lista de datos de la tabla Master.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpGet("GetDataMaster/{mast}")]
        public IActionResult GetDataMaster([FromRoute] string mast)
        {
            try
            {
                var Datos = from m in _context.MasterTable.AsNoTracking()
                            where m.master.Equals(mast)
                            group m by new { m.master, m.codigo, m.nombre } into g
                            select new
                            {
                                master = g.Key.master.Trim(),
                                codigo = g.Key.codigo.Trim(),
                                nombre = g.Key.nombre.Trim()
                            };
                return (Datos != null) ? Ok(Datos) : NotFound();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Obtiene los datos de los tipos de equipos que existen para ingresar equipos.
        /// </summary>
        /// <returns>Lista de datos de los tipos de equipos existentes.</returns>
        /// <response code="200">Devuelve los tipos de equipos existentes.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel3")]
        [HttpGet("GetTipoEquipo")]
        public IActionResult GetTipoEquipo()
        {
            try
            {
                var Datos = from m in _context.MasterTable.AsNoTracking()
                            where m.master.Equals("MQT") && (m.codigo == "009" || m.codigo == "012")
                            group m by new { m.master, m.codigo, m.nombre } into g
                            select new
                            {
                                master = g.Key.master.Trim(),
                                codigo = g.Key.codigo.Trim(),
                                nombre = g.Key.nombre.Trim()
                            };
                return (Datos != null) ? Ok(Datos) : NotFound();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Obtiene las localidades de la tabla Master.
        /// </summary>
        /// <returns>Lista de localidades que existen en Master.</returns>
        /// <response code="200">Devuelve las localidades que existen en Master.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpGet("ObtenerDatamasterLocalidades/{codCliente}")]
        public IActionResult ObtenerDatamasterLocalidades([FromRoute] string codCliente)
        {
            try
            {
                var Datos = from t in _context.MasterTable.AsNoTracking()
                            where t.master.Equals("CCAN") && !(
                                                                from l in _context.ClienteSignaLocalidad
                                                                where l.codigoCiente == codCliente
                                                                select l.codigo
                                                               ).Contains(t.codigo)
                            select new { Master = t.master.Trim(), Codigo = t.codigo.Trim(), Nombre = t.nombre.Trim() };
                return (Datos != null) ? Ok(Datos) : NotFound();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Obtiene todas las localidades de la tabla Master para modificar el codigolocalidadbanco (campo2).
        /// </summary>
        /// <returns>Lista de localidades que existen en Master.</returns>
        /// <response code="200">Se devuelve todas las localidades existentes en dataMaster.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpGet("ObtenerLocalidades")]
        public IActionResult ObtenerLocalidades()
        {
            try
            {
                var Datos = from t in _context.MasterTable.AsNoTracking()
                            where t.master.Equals("CCAN")
                            select new { Codigo = t.codigo.Trim(), Nombre = t.nombre.Trim(), codigoLocalidadBanco =  t.codigoLocalidadBanco.Trim() };
                return (Datos != null) ? Ok(Datos) : NotFound();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Actualiza informacion de el codigo localidad del banco especifica.
        /// </summary>
        /// <response code="200">Actualizo correctamente el registro.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPut("ActualizarLocalidad")]
        public async Task<IActionResult> ActualizarLocalidadAsync([FromBody] ActualizarCodigoLocalidad actualizarCodigoLocalidad)
        {
            try
            {
                bool existeDuplicado = await _context.MasterTable
                    .AnyAsync(u =>
                        u.codigoLocalidadBanco == actualizarCodigoLocalidad.codigoLocalidad
                        && u.codigo != actualizarCodigoLocalidad.codigo
                    );

                if (existeDuplicado)
                {
                    return Ok(new {respuesta = "Repetido"});
                }

                var affectedRows = await _context.MasterTable
                    .Where(u => u.codigo == actualizarCodigoLocalidad.codigo)
                    .ExecuteUpdateAsync(u => u
                        .SetProperty(p => p.codigoLocalidadBanco, actualizarCodigoLocalidad.codigoLocalidad)
                    );

                return affectedRows > 0 ? Ok() : NotFound("Registro no encontrado");
            }
            catch (Exception ex)
            {
                return Problem("Error interno: " + ex.Message, statusCode: 500);
            }
        }
    }
}