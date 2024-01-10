using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    [Route("api/TransAcreditada")]
    [ApiController]
    public class TransaccionesAcreditarController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public TransaccionesAcreditarController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("GuardarTransacciones")]
        public async Task<IActionResult> GuardarTransacciones([FromBody] TransaccionesAcreditadas model)
        {
            if (ModelState.IsValid)
            {                
                await _context.TransaccionesAcreditadas.AddAsync(model);                
                if (await _context.SaveChangesAsync() > 0)
                {
                    return Ok(model);
                }
                else
                {
                    return BadRequest("Datos incorrectos");
                }
            }
            else
            {
                return BadRequest("ERROR");
            }
        }
    }
}