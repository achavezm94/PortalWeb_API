using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Models;
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
        public async Task<IEnumerable<SP_DatosEquiposFrontResult>> ObtenerDetalleAsync(string id)
        {
            return await _context.GetProcedures().SP_DatosEquiposFrontAsync(id);
        }
    }
}
