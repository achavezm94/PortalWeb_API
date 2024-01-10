using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    [Route("api/Grafico")]
    [ApiController]
    public class GraficoTotalesAnualController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public GraficoTotalesAnualController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ObtenerEquipo/{serieEquipo}")]
        public async Task<IEnumerable<SP_GraficoTotalesAñoResult>> ObtenerEquipoAsync([FromRoute] string serieEquipo)
        {
            return await _context.GetProcedures().SP_GraficoTotalesAñoAsync(serieEquipo);
        }
    }
}
