using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion de cuenta bancarias.
    /// </summary>
    [Route("api/Cuenta")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        private readonly PortalWebContext _context;

        /// <summary>
        /// Extraer el context de EF.
        /// </summary>
        public CuentaController(PortalWebContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene toda la informacion correspondiente de una cuenta bancaria y sus asignaciones.
        /// </summary>
        /// <returns>Lista de datos de las cuentas bancarias y sus asignaciones.</returns>
        /// <response code="200">Devuelve la lista de datos de las cuentas bancarias y sus asignaciones.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpGet("NTransacciones/{id}")]
        public IActionResult ObtenerCuentasBancarias(int id)
        {
            var Datos = (from cb in _context.cuentas_bancarias.AsNoTracking()
                         join cs in _context.cuentaSignaTienda.AsNoTracking() on cb.id equals cs.idcuentabancaria
                         join t in _context.Tiendas.AsNoTracking() on cs.idtienda equals t.CodigoTienda
                         join e in _context.Equipos.AsNoTracking() on t.id equals e.codigoTiendaidFk
                         join d in _context.Depositos.AsNoTracking() on e.serieEquipo equals d.Machine_Sn
                         where cb.id.Equals(id)
                         select d.Transaccion_No).IsNullOrEmpty();
            return Ok(!Datos);
        }

        /// <summary>
        /// Guarda una cuenta bancaria.
        /// </summary>        
        /// <response code="200">Se registro la cuenta bancaria.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
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

        /// <summary>
        /// Actualiza información de una cuenta bancaria.
        /// </summary>
        /// <response code="200">Actualizo correctamente el registro.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPut("ActualizarCuenta")]
        public async Task<IActionResult> ActualizarCuentaAsync([FromBody] cuentas_bancarias model)
        {
            _context.Attach(model);
            _context.Entry(model).State = EntityState.Modified;
            _context.Entry(model).Property(nameof(model.id)).IsModified = false;
            return (await _context.SaveChangesAsync() > 0) ? Ok() : BadRequest();
        }

        /// <summary>
        /// Borra información de una cuenta bancaria.
        /// </summary>
        /// <response code="200">Borro correctamente el registro.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
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