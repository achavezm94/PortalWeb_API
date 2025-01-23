using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion de asignacion de una cuenta a un cliente.
    /// </summary>
    [Route("api/CuentAsigna")]
    [ApiController]
    public class CuentaAsignaController : ControllerBase
    {

        private readonly PortalWebContext _context;

        /// <summary>
        /// Extraer el context de EF.
        /// </summary>
        public CuentaAsignaController(PortalWebContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Guarda cuenta asignada a un cliente.
        /// </summary>        
        /// <response code="200">Se registro la cuenta asignada en el cliente.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPost("GuardarCuentAsigna")]
        public async Task<IActionResult> GuardarCuentAsigna([FromBody] cuentaSignaTienda model)
        {
            if (ModelState.IsValid)
            {
                await _context.cuentaSignaTienda.AddAsync(model);
                return (await _context.SaveChangesAsync() > 0) ? Ok() : BadRequest();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}