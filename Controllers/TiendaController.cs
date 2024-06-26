﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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

        [HttpGet]
        [Route("ObtenerTienda")]
        public async Task<IActionResult> ObtenerTienda()
        {
            if (ModelState.IsValid)
            {
                var tiendas = await _context.Tiendas.ToListAsync();

                if (tiendas == null)
                {
                    return NotFound();
                }
                return Ok(tiendas);
            }
            else
            {
                return BadRequest("ERROR");
            }
        }

        [HttpGet("ObtenerTiendasCompletas")]
        public async Task<IEnumerable<ObtenerTiendasResult>> ObtenerTiendasAsync()
        {
            return await _context.GetProcedures().ObtenerTiendasAsync();            
        }

        [HttpPost]
        [Route("GuardarTienda")]
        public async Task<IActionResult> GuardarTienda([FromBody] Tiendas model)
        {
            if (ModelState.IsValid)
            {
                await _context.Tiendas.AddAsync(model);
                return (await _context.SaveChangesAsync() > 0) ? Ok(model) : BadRequest("No se guardo");
            }
            else
            {
                return BadRequest("ERROR");
            }
        }

        [HttpPut]
        [Route("ActualizarTienda")]
        public async Task<IActionResult> ActualizarTienda([FromBody] Tiendas model)
        {
            var result = await _context.Tiendas.FindAsync(model.CodigoTienda);
            if (result != null)
            {
                try
                {
                    result.NombreTienda = model.NombreTienda;
                    result.Telefono = model.Telefono;
                    result.Direccion = model.Direccion;
                    result.NombreAdmin = model.NombreAdmin;
                    result.TelfAdmin = model.TelfAdmin;
                    result.EmailAdmin = model.EmailAdmin;
                    result.CodProv = model.CodProv;
                    result.IdCentroProceso = model.IdCentroProceso;
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest();
                }
                finally
                {
                    await _context.SaveChangesAsync();
                }
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("BorrarTienda/{id:int}")]
        public async Task<IActionResult> BorrarTienda(int id)
        {
            var result = await _context.Tiendas
                 .FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                _context.Tiendas.Remove(result);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return NoContent();
                }
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
