using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;
using System.Data;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion de clientes.
    /// </summary>
    [Route("api/Cliente")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly PortalWebContext _context;

        /// <summary>
        /// Extraer el context de EF.
        /// </summary>
        public ClienteController(PortalWebContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene clientes con datos para cuentas bancarias y localidades.
        /// </summary>
        /// <returns>Lista de datos de los clientes.</returns>
        /// <response code="200">Devuelve la lista de clientes.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel2")]
        [HttpGet("ObtenerCliente")]
        public IActionResult ObtenerCliente()
        {
            try
            {
                var query = from cl in _context.Clientes.AsNoTracking()
                        join cb in _context.cuentas_bancarias.AsNoTracking() on cl.CodigoCliente equals cb.CodigoCliente into cbGroup
                        from cbg in cbGroup.DefaultIfEmpty()
                        join csl in _context.ClienteSignaLocalidad.AsNoTracking() on cl.CodigoCliente equals csl.codigoCiente into cslGroup
                        from cslg in cslGroup.DefaultIfEmpty()
                        group new { cl, cbg, cslg } by new
                        {
                            cl.id,
                            cl.CodigoCliente,
                            cl.NombreCliente,
                            cl.RUC,
                            cl.Direccion,
                            cl.Active,
                            cl.CodigoClienteBanco,
                            cl.emailcontacto,
                            cl.nombrecontacto
                        } into g
                        select new
                        {
                            g.Key.id,
                            g.Key.CodigoCliente,
                            g.Key.NombreCliente,
                            g.Key.RUC,
                            g.Key.Direccion,
                            g.Key.Active,
                            g.Key.CodigoClienteBanco,
                            g.Key.emailcontacto,
                            g.Key.nombrecontacto,
                            cantidadCuentasBancarias = g.Select(x => x.cbg.id).Distinct().Count(),
                            cantidadLocalidades = g.Select(x => x.cslg.id).Distinct().Count()
                        };
            var Datos = query.ToList();
            return (Datos != null) ? Ok(Datos) : NotFound();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Obtiene codigo y nombre de clientes.
        /// </summary>
        /// <returns>Lista de datos de los clientes.</returns>
        /// <response code="200">Devuelve la lista de clientes.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Monitor")]
        [HttpGet("ObtenerClienteSelect")]
        public IActionResult ObtenerClienteSelect()
        {
            try
            {
                var Datos = from cl in _context.Clientes.AsNoTracking()
                        select new
                        {
                            cl.id,
                            cl.CodigoCliente,
                            cl.NombreCliente
                        };
            return (Datos != null) ? Ok(Datos) : NotFound();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Obtiene cuenta de los clientes.
        /// </summary>
        /// <returns>Lista de datos de las cuentas de los clientes.</returns>
        /// <response code="200">Devuelve lista con datos de cuentas de clientes.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel2")]
        [HttpGet("ObtenerCuentaCliente/{CodCliente}")]
        public IActionResult ObtenerCuentaCliente(string CodCliente)
        {
            try
            {
                var Datos = from cli in _context.Clientes.AsNoTracking()
                            join cb in _context.cuentas_bancarias.AsNoTracking() on cli.CodigoCliente equals cb.CodigoCliente
                            where cli.CodigoCliente == CodCliente
                            select new
                            {
                                cb.id,
                                ClienteID = cli.id,
                                cb.CodigoCliente,
                                cb.codcuentacontable,
                                cb.nombanco,
                                cb.numerocuenta,
                                cb.TipoCuenta,
                                cb.Observacion,
                                cb.fecrea
                            };
                return (Datos != null) ? Ok(Datos) : NotFound();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Guarda un cliente.
        /// </summary>
        /// <response code="200">Se registro el cliente.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPost("GuardarCliente")]
        public async Task<IActionResult> GuardarCliente([FromBody] Clientes model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _context.Clientes.AddAsync(model);
                    return (await _context.SaveChangesAsync() > 0) ? Ok() : BadRequest();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Actualiza información de un cliente.
        /// </summary>
        /// <response code="200">Actualizo correctamente el registro.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPut("ActualizarCliente")]
        public async Task<IActionResult> ActualizarClienteAsync([FromBody] Clientes model)
        {
            try
            {
                _context.Attach(model);
                _context.Entry(model).State = EntityState.Modified;
                _context.Entry(model).Property(nameof(model.id)).IsModified = false;
                return (await _context.SaveChangesAsync() > 0) ? Ok() : BadRequest();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Borra información de un cliente.
        /// </summary>
        /// <response code="200">Borro correctamente el registro.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpDelete("BorrarCliente/{id}")]
        public IActionResult BorrarCliente(int id)
        {
            try
            {
                var delete = _context.Clientes
                               .Where(b => b.id.Equals(id))
                               .ExecuteDelete();
                return (delete != 0) ? Ok() : BadRequest();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }
    }
}