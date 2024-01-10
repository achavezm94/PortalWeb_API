using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using System.Data;

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
    }
}
