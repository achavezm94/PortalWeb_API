using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    [Route("api/ClienteSignaLocalidad")]
    [ApiController]
    public class ClienteSignaLocalidadController : ControllerBase
    {
        private readonly PortalWebContext _context;
        public ClienteSignaLocalidadController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("GuardarClienteSignaTienda")]
        public async Task<IActionResult> GuardarCliente([FromBody] ObjClienteLocalidad model)
        {
            foreach (var item in model.Localidades)
            {
                await _context.ClienteSignaLocalidad.AddAsync(new ClienteSignaLocalidad
                {
                    CodigoCiente = model.Cliente,
                    Master = "CCAN",
                    Codigo = item
                });
            }
            return (await _context.SaveChangesAsync() > 0) ? Ok() : BadRequest();            
        }
    }
}
