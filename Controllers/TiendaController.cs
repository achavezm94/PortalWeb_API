using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;
using System.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/tiendas")]
    [ApiController]
    public class TiendaController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public TiendaController(PortalWebContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Nivel2")]
        [HttpGet("ObtenerTienda")]
        public async Task<IActionResult> ObtenerTienda()
        {
            if(ModelState.IsValid)
            {
                var tiendas = await _context.Tiendas.ToListAsync();
                return (tiendas != null) ? Ok(tiendas) : NotFound();
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize(Policy = "Nivel1")]
        [HttpGet("ObtenerTiendaFiltroCliente/{codigoCliente}")]
        public async Task<IActionResult> ObtenerTiendaFiltroCliene(string codigoCliente)
        {
            if (ModelState.IsValid)
            {
                var tiendas = await _context.Tiendas
                                                    .Where(t => t.CodigoClienteidFk == codigoCliente)
                                                    .Select(t => new {t.id, t.NombreTienda })
                                                    .ToListAsync();
                return (tiendas != null) ? Ok(tiendas) : NotFound();
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize(Policy = "Nivel2")]
        [HttpGet("ObtenerTiendasCompletas")]
        public IActionResult ObtenerTiendas() {
            var query = from td in _context.Tiendas
                        join cli in _context.Clientes
                            on td.CodigoClienteidFk equals cli.CodigoCliente into cliGroup
                        from cli in cliGroup.DefaultIfEmpty()
                        join mt1 in _context.MasterTable
                            on td.CodProv equals mt1.codigo into mt1Group
                        from mt1 in mt1Group.Where(m => m.master == "CCAN").DefaultIfEmpty()
                        join ct in _context.cuentaSignaTienda
                            on td.CodigoTienda equals ct.idtienda into ctGroup
                        group new { td, cli, mt1, ctGroup } by new
                        {
                            cli.CodigoCliente,
                            cli.NombreCliente,
                            cli.RUC,
                            td.Telefono,
                            td.NombreAdmin,
                            td.TelfAdmin,
                            td.Direccion,
                            mt1.nombre,
                            td.id,
                            td.CodigoTienda,
                            td.CodigoClienteidFk,
                            td.NombreTienda,
                            td.EmailAdmin,
                            td.CodProv
                        } into grouped
                        select new
                        {
                            grouped.Key.CodigoCliente,
                            grouped.Key.NombreCliente,
                            grouped.Key.RUC,
                            grouped.Key.Telefono,
                            grouped.Key.NombreAdmin,
                            grouped.Key.TelfAdmin,
                            grouped.Key.Direccion,
                            NombreLocalidad = grouped.Key.nombre,
                            grouped.Key.id,
                            grouped.Key.CodigoTienda,
                            grouped.Key.CodigoClienteidFk,
                            grouped.Key.NombreTienda,
                            grouped.Key.EmailAdmin,
                            grouped.Key.CodProv,
                            CantidadCuentasAsign = grouped.SelectMany(g => g.ctGroup).Count()
                        };
            var result = query
                .OrderBy(x => x.NombreCliente)
                .ThenBy(x => x.NombreTienda)
                .ToList();
            return Ok(result);
        }


        [Authorize(Policy = "Nivel1")]
        [HttpPost("GuardarTienda")]
        public async Task<IActionResult> GuardarTienda([FromBody] Tiendas model)
        {
            if (ModelState.IsValid)
            {
                await _context.Tiendas.AddAsync(model);
                return (await _context.SaveChangesAsync() > 0) ? Ok(model) : BadRequest();
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize(Policy = "Nivel1")]
        [HttpPut("ActualizarTienda")]
        public async Task<IActionResult> ActualizarTiendaAsync([FromBody] Tiendas model)
        {
            _context.Attach(model);
            _context.Entry(model).State = EntityState.Modified;
            _context.Entry(model).Property(nameof(model.id)).IsModified = false;
            return (await _context.SaveChangesAsync() > 0) ? Ok() : BadRequest();
        }

        [Authorize(Policy = "Nivel1")]
        [HttpDelete("BorrarTienda/{id}")]
        public IActionResult BorrarTienda(int id)
        {
            var delete = _context.Tiendas
                            .Where(b => b.id.Equals(id))
                            .ExecuteDelete();
            return (delete != 0) ? Ok() : BadRequest();
        }
    }
}