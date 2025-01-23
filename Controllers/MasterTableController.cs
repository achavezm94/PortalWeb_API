using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
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
            var Datos = from m in _context.MasterTable
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
            var Datos = from m in _context.MasterTable
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
            var Datos = from t in _context.MasterTable
                        where t.master.Equals("CCAN") && !(
                                                            from l in _context.ClienteSignaLocalidad
                                                            where l.codigoCiente == codCliente
                                                            select l.codigo
                                                           ).Contains(t.codigo)
                        select new { Master = t.master.Trim(), Codigo = t.codigo.Trim(), Nombre = t.nombre.Trim() };
            return (Datos != null) ? Ok(Datos) : NotFound();
        }
    }
}