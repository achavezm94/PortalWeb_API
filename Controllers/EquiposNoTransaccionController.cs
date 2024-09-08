using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Methods;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    [Route("api/EquiposNoTransaccion")]
    [ApiController]
    public class EquiposNoTransaccionController : ControllerBase
    {
        private readonly PortalWebContext _context;
        public EquiposNoTransaccionController(PortalWebContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Nivel1")]
        [HttpGet("Cuadre/{machine_Sn}")]
        public IActionResult ResultadoCuadre([FromRoute] string machine_Sn)
        {
            MetodosTotales metodosTotales = new(_context);
            TotalTodoTransacciones todasTransacciones = metodosTotales.TotalesTodasTransacciones(machine_Sn);
            TotalUltimaTransaccion[] ultimaTransaccion = metodosTotales.TotalesUltimaTransaccion(machine_Sn);
            int resultado = (todasTransacciones.Total == ultimaTransaccion[0].TotalMont) ? 0 : // 0 = iguales
                (todasTransacciones.Total > ultimaTransaccion[0].TotalMont) ? 1 : 2; // 1 = duplicadas, 2 = faltante
            double? totalcuadre = (double?)_context.TotalesEquipos.Where(d => d.Equipo == machine_Sn).First().TotalCuadreEquipo;
            if (totalcuadre == 0 )
            {
                return Ok(new { machine_Sn, diferencia = (todasTransacciones.Total - ultimaTransaccion[0].TotalMont), resultado });
            }
            else
            {
                return Ok(new { machine_Sn, diferencia = (ultimaTransaccion[0].TotalMont - todasTransacciones.Total ) + totalcuadre, resultado = 2 });
            }
            
        }

        [Authorize(Policy = "Nivel1")]
        [HttpPost("Conteo/{opcion}")]
        public async Task<IEnumerable<SP_EquiposNoTransaccionesResult>> ResultadoConteoTransacciones([FromBody] FechasIniFin filtroFechas, [FromRoute] int opcion)=> 
            await _context.GetProcedures().SP_EquiposNoTransaccionesAsync(filtroFechas.FechaIni, filtroFechas.FechaFin, opcion);
    }
}