using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;
using System.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/Indicadores")]
    [ApiController]
    public class IndicadoresPrincipalesController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public IndicadoresPrincipalesController(PortalWebContext context)
        {
            _context = context;
        }
        /*
        [HttpGet]
        [Route("ObtenerEquipo/{serieEquipo}")]
        public async Task<IEnumerable<SP_GraficoTotalesAñoResult>> ObtenerEquipoAsync([FromRoute] string serieEquipo)
        {
            return await _context.GetProcedures().SP_GraficoTotalesAñoAsync(serieEquipo);
        }
         */


        [HttpGet("ObtenerIndicadores/{id}/{tp}")]
        public async Task<IEnumerable<SP_IndicadoresLlenadoResult>> ObtenerIndicadores([FromRoute] string id, [FromRoute] int tp)
        {
            return await _context.GetProcedures().SP_IndicadoresLlenadoAsync(id, tp);
        }
    }
}
