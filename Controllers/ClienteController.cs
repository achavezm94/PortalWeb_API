using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;
using System.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/Cliente")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public ClienteController(PortalWebContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Nivel2")]
        [HttpGet("ObtenerCliente")]
        public IActionResult ObtenerCliente()
        {
            var query = from cl in _context.Clientes
                        join cb in _context.cuentas_bancarias on cl.CodigoCliente equals cb.CodigoCliente into cbGroup
                        from cbg in cbGroup.DefaultIfEmpty()
                        join csl in _context.ClienteSignaLocalidad on cl.CodigoCliente equals csl.codigoCiente into cslGroup
                        from cslg in cslGroup.DefaultIfEmpty()
                        group new { cl, cbg, cslg } by new
                        {
                            cl.id,
                            cl.CodigoCliente,
                            cl.NombreCliente,
                            cl.RUC,
                            cl.Direccion,
                            cl.Active,
                            cl.telefcontacto,
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
                            g.Key.telefcontacto,
                            g.Key.emailcontacto,
                            g.Key.nombrecontacto,
                            cantidadCuentasBancarias = g.Select(x => x.cbg.id).Distinct().Count(),
                            cantidadLocalidades = g.Select(x => x.cslg.id).Distinct().Count()
                        };
            var Datos = query.ToList();
            return (Datos != null) ? Ok(Datos) : NotFound();
        }

        [Authorize(Policy = "Monitor")]
        [HttpGet("ObtenerClienteSelect")]
        public IActionResult ObtenerClienteSelect()
        {
            var Datos = from cl in _context.Clientes
                        select new
                        {
                            cl.id,
                            cl.CodigoCliente,
                            cl.NombreCliente
                        };
            return (Datos != null) ? Ok(Datos) : NotFound();
        }

        [Authorize(Policy = "Nivel2")]
        [HttpGet("ObtenerCuentaCliente/{CodCliente}")]
        public IActionResult ObtenerCuentaCliente(string CodCliente)
        {
            var Datos = from cli in _context.Clientes
                        join cb in _context.cuentas_bancarias on cli.CodigoCliente equals cb.CodigoCliente
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

        [Authorize(Policy = "Nivel1")]
        [HttpPost("GuardarCliente")]
        public async Task<IActionResult> GuardarCliente([FromBody] Clientes model)
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

        [Authorize(Policy = "Nivel1")]
        [HttpPut("ActualizarCliente")]
        public async Task<IActionResult> ActualizarClienteAsync([FromBody] Clientes model)
        {
            _context.Attach(model);
            _context.Entry(model).State = EntityState.Modified;
            _context.Entry(model).Property(nameof(model.id)).IsModified = false;
            return (await _context.SaveChangesAsync() > 0) ? Ok() : BadRequest();
        }

        [Authorize(Policy = "Nivel1")]
        [HttpDelete("BorrarCliente/{id}")]
        public IActionResult BorrarCliente(int id)
        {
            var delete = _context.Clientes
                           .Where(b => b.id.Equals(id))
                           .ExecuteDelete();
            return (delete != 0) ? Ok() : BadRequest();
        }
    }
}