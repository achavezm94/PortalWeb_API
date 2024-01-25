using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;


namespace PortalWeb_API.Controllers
{
    [Route("api/EquipoDetalle")]
    [ApiController]
    public class EquipoTransaccionController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public EquipoTransaccionController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpGet("ObtenerDetalle/{id}")]
        public async Task<IActionResult> ObtenerDetalleAsync(string id)
        {
            var response = await _context.GetProcedures().SP_DatosEquiposFrontAsync(id);
            if (response == null)
            {
                return NotFound("No se ha podido crear...");
            }
            return Ok(response);
        }

        [HttpGet("ObtenerTotales/{machineSn}")]
        public async Task<IActionResult> ObtenerTotalesAsync(string machineSN) 
        {
            var response = await _context.GetProcedures().SP_ObtenerTotalesMachinAsync(machineSN);
            if (response == null)
            {
                return NotFound("No se ha podido crear...");
            }
            return Ok(response);
        }
    }
}