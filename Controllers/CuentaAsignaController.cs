using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    [Route("api/CuentAsigna")]
    [ApiController]
    public class CuentaAsignaController : ControllerBase
    {

          private readonly PortalWebContext _context;

          public CuentaAsignaController ( PortalWebContext context )
          {
              _context = context;
          }

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