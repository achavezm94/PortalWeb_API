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

        [HttpPost]
        [Route("GuardarCuentAsigna")]
        public async Task<IActionResult> GuardarCuentAsigna([FromBody] CuentaSignaTienda model)
        {
            if (ModelState.IsValid)
            {
                await _context.CuentaSignaTienda.AddAsync(model);
                return (await _context.SaveChangesAsync() > 0) ? Ok("Se guardo") : BadRequest("Datos incorrectos");
            }
            else
            {
                return BadRequest("Error");
            }
        }
    }
}
