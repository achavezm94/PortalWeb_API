using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    [Route("api/CalculoTotal")]
    [ApiController]
    public class CalculoTotalController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public CalculoTotalController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpGet("Calculo/{machine_Sn}")]
        public async Task<IEnumerable<SP_CalculoTotalResult>> ResultadoCalculoTotal([FromRoute] string machine_Sn )
        {
            return await _context.GetProcedures().SP_CalculoTotalAsync(machine_Sn);
        }
    }
}
