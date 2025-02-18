using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion de asignacion de Localidad a cliente.
    /// </summary>
    [Route("api/ClienteSignaLocalidad")]
    [ApiController]
    public class ClienteSignaLocalidadController : ControllerBase
    {
        private readonly PortalWebContext _context;

        /// <summary>
        /// Extraer el context de EF.
        /// </summary>
        public ClienteSignaLocalidadController(PortalWebContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene localidad de un cliente.
        /// </summary>
        /// <returns>Lista localidades de un clientes.</returns>
        /// <response code="200">Devuelve litsa localidades de un cliente.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel2")]
        [HttpGet("ObtenerLocalidades/{cliente}")]
        public IActionResult ObtenerDatamasterLocalidades(string cliente)
        {
            var Datos = from t in _context.ClienteSignaLocalidad.AsNoTracking()
                        join l in _context.MasterTable.AsNoTracking() on new { t.codigo, t.master } equals new { l.codigo, l.master }
                        join c in _context.Clientes.AsNoTracking() on cliente equals c.CodigoCliente
                        where t.codigoCiente == cliente
                        select new { t.id, idCliente = c.id, t.master, t.codigo, l.nombre };
            return (Datos != null) ? Ok(Datos) : NotFound();
        }

        /// <summary>
        /// Guarda localidad de un cliente.
        /// </summary>        
        /// <response code="200">Se registro la localidad en el cliente.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPost("GuardarClienteSignaTienda")]
        public async Task<IActionResult> GuardarCliente([FromBody] ObjClienteLocalidad model)
        {
            foreach (var item in model.Localidades)
            {
                await _context.ClienteSignaLocalidad.AddAsync(new ClienteSignaLocalidad
                {
                    codigoCiente = model.Cliente,
                    master = "CCAN",
                    codigo = item
                });
            }
            return (await _context.SaveChangesAsync() > 0) ? Ok() : BadRequest();
        }

        /// <summary>
        /// Borra la localidad de un cliente.
        /// </summary>
        /// <response code="200">Borro correctamente la localidad.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpDelete("BorrarLocalidad/{id}")]
        public IActionResult EliminarLocalidadAsync(int id)
        {
            var delete = _context.ClienteSignaLocalidad
                            .Where(b => b.id.Equals(id))
                            .ExecuteDelete();
            return (delete != 0) ? Ok() : BadRequest();
        }
    }
}