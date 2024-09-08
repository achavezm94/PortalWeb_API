using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    [Route("api/Cuenta")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public CuentaController(PortalWebContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Nivel1")]
        [HttpGet("NTransacciones/{id}")]
        public IActionResult GenerarTransaccionesAcreeditadas(int id)
        {
            var Datos = (from cb in _context.cuentas_bancarias
                        join cs in _context.cuentaSignaTienda on cb.id equals cs.idcuentabancaria
                        join t in _context.Tiendas on cs.idtienda equals t.CodigoTienda
                        join e in _context.Equipos on t.id equals e.codigoTiendaidFk
                        join d in _context.Depositos on e.serieEquipo equals d.Machine_Sn
                        where cb.id.Equals(id)
                        select d.Transaccion_No).IsNullOrEmpty();
            return Ok(!Datos);
        }

        [Authorize(Policy = "Nivel1")]
        [HttpPost("GuardarCuenta")]
        public async Task<IActionResult> GuardarCuenta([FromBody] cuentas_bancarias model)
        {
            if (ModelState.IsValid)
            {
                await _context.cuentas_bancarias.AddAsync(model);
                return (await _context.SaveChangesAsync() > 0) ? Ok() : BadRequest();
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize(Policy = "Nivel1")]
        [HttpPut("ActualizarCuenta")]
        public async Task<IActionResult> ActualizarCuentaAsync([FromBody] cuentas_bancarias model)
        {
            _context.Attach(model);
            _context.Entry(model).State = EntityState.Modified;
            _context.Entry(model).Property(nameof(model.id)).IsModified = false;
            return (await _context.SaveChangesAsync() > 0) ? Ok() : BadRequest();
        }

        [Authorize(Policy = "Nivel1")]
        [HttpDelete("BorrarCuenta/{id}")]
        public IActionResult BorrarCuenta(int id)
        {
            var delete = _context.cuentas_bancarias
                           .Where(b => b.id.Equals(id))
                           .ExecuteDelete();
            return (delete != 0) ? Ok() : BadRequest();
        }
    }
}