﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;
using System.Data;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion de Tienda.
    /// </summary>
    [Route("api/tiendas")]
    [ApiController]
    public class TiendaController : ControllerBase
    {
        private readonly PortalWebContext _context;

        /// <summary>
        /// Extraer el context de EF.
        /// </summary>
        public TiendaController(PortalWebContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todas las tiendas disponibles en el sistema.
        /// </summary>
        /// <returns>Lista de datos de las tiendas que estan registradas en el sistema.</returns>
        /// <response code="200">Devuelve lista de datos de todas las tiendas del sistema.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel2")]
        [HttpGet("ObtenerTienda")]
        public async Task<IActionResult> ObtenerTienda()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tiendas = await _context.Tiendas.AsNoTracking().ToListAsync();
                    return (tiendas != null) ? Ok(tiendas) : NotFound();
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
        /// Obtiene todas las tiendas disponibles en el sistema de un cliene.
        /// </summary>
        /// <returns>Lista de datos de las tiendas que estan registradas en el sistema de un cliente.</returns>
        /// <response code="200">Devuelve lista de datos de todas las tiendas del sistema de un cliente.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpGet("ObtenerTiendaFiltroCliente/{codigoCliente}")]
        public async Task<IActionResult> ObtenerTiendaFiltroCliene(string codigoCliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tiendas = await _context.Tiendas.AsNoTracking()
                                                        .Where(t => t.CodigoClienteidFk == codigoCliente)
                                                        .Select(t => new { t.id, t.NombreTienda })
                                                        .ToListAsync();
                    return (tiendas != null) ? Ok(tiendas) : NotFound();
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
        /// Obtiene todas las tiendas con datos de clientes, cuentas disponibles en el sistema.
        /// </summary>
        /// <returns>Lista de datos de las tiendas con información de clientes, cuentas que estan registradas en el sistema.</returns>
        /// <response code="200">Devuelve lista de datos de todas las tiendas con información de clientes, cuentas del sistema.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel2")]
        [HttpGet("ObtenerTiendasCompletas")]
        public IActionResult ObtenerTiendas()
        {
            try
            {
                var query = from td in _context.Tiendas.AsNoTracking()
                            join cli in _context.Clientes.AsNoTracking()
                                on td.CodigoClienteidFk equals cli.CodigoCliente into cliGroup
                            from cli in cliGroup.DefaultIfEmpty()
                            join mt1 in _context.MasterTable.AsNoTracking()
                                on td.CodProv equals mt1.codigo into mt1Group
                            from mt1 in mt1Group.Where(m => m.master == "CCAN").DefaultIfEmpty()
                            join ct in _context.cuentaSignaTienda.AsNoTracking()
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
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Guarda una tienda nueva.
        /// </summary>        
        /// <response code="200">Se registro la tienda a la plataforma.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPost("GuardarTienda")]
        public async Task<IActionResult> GuardarTienda([FromBody] Tiendas model)
        {
            try
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
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Actualiza informacion de una tienda especifica.
        /// </summary>
        /// <response code="200">Actualizo correctamente el registro.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPut("ActualizarTienda")]
        public async Task<IActionResult> ActualizarTiendaAsync([FromBody] Tiendas model)
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
        /// Elimina una tienda.
        /// </summary>
        /// <response code="200">Elimino correctamente el registro.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpDelete("BorrarTienda/{id}")]
        public IActionResult BorrarTienda(int id)
        {
            try
            {
                var delete = _context.Tiendas
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