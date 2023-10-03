using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    [Route("api/AlertError")]
    [ApiController]
    public class AlertErrorController : ControllerBase
    {

        private readonly PortalWebContext _context;

        public AlertErrorController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("GuardarErrorAlert")]
        public async Task<IActionResult> GuardarErrorAlert([FromBody] ErrorAlerts model)
        {
            if (ModelState.IsValid)
            {
                await _context.ErrorAlerts.AddAsync(model);
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
