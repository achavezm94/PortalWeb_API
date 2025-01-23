using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using System.Data;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion de Cuentas asignadas a Tiendas.
    /// </summary>
    [Route("api/TiendaCuenta")]
    [ApiController]
    public class TiendaCuentaController : ControllerBase
    {
        private readonly PortalWebContext _context;

        /// <summary>
        /// Extraer el context de EF.
        /// </summary>
        public TiendaCuentaController(PortalWebContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todas las tiendas con sus respectivas cuentas asingadas.
        /// </summary>
        /// <returns>Lista de datos de las tiendas y cuentas asignadas que estan registradas en el sistema.</returns>
        /// <response code="200">Devuelve lista de datos de todas las tiendas y cuentas asignadas del sistema.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel2")]
        [HttpGet("ObtenerTiendaCuentas/{id}")]
        public IActionResult ObtenerTiendaCuentas([FromRoute] string id)
        {
            var Datos = from otc in _context.cuentaSignaTienda
                        join ct in _context.cuentas_bancarias on otc.idcuentabancaria equals ct.id
                        where otc.idtienda == id
                        select new { otc.idcuentabancaria, ct.nombanco, ct.numerocuenta, otc.id, ct.TipoCuenta, ct.Observacion };
            return (Datos != null) ? Ok(Datos) : NotFound();
        }

        /// <summary>
        /// Elimina una cuenta asignada a una tienda.
        /// </summary>
        /// <response code="200">Elimino correctamente el registro.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel2")]
        [HttpDelete("BorrarCuentaTienda/{id}")]
        public IActionResult BorrarCuentaTienda([FromRoute] int id)
        {
            var delete = _context.cuentaSignaTienda
                          .Where(b => b.id.Equals(id))
                          .ExecuteDelete();
            return (delete != 0) ? Ok() : BadRequest();
        }
    }
}
