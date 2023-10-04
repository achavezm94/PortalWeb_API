using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    [Route("api/Servicios")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private readonly PortalWebContext _context;
        private readonly IHubContext<PingHubEquipos> _pinghub;
        public ServiciosController(PortalWebContext context, IHubContext<PingHubEquipos> pingHub)
        {
            _context = context;
            _pinghub = pingHub;
        }

        [HttpGet]
        [Route("ObtenerIP")]
        public async Task<IActionResult> ObtenerIP()
        {
            if (ModelState.IsValid)
            {
                var ip = await _context.Database.SqlQuery<string>($"select [IpEquipo] From [Equipos] WHERE Active = 'A'").ToListAsync();

                if (ip == null)
                {
                    return NotFound();
                }
                return Ok(ip);
            }
            else
            {
                return BadRequest("ERROR");
            }
        }

        [HttpPut]
        [Route("ActualizarEquipoIp")]
        public async Task<IActionResult> ActualizarEquipo([FromBody] List<EquiposDto> model)
        {
            foreach (var equipo in model)
            {
                var res = _context.Equipos.FirstOrDefault(x => x.IpEquipo == equipo.IpEquipo);
                if (res != null)
                {
                    if (equipo.EstadoPing == 0)
                    {
                        if (equipo.EstadoPing != res.EstadoPing)
                        {
                            res.EstadoPing = equipo.EstadoPing;
                            res.Active = "A";
                        }
                    }
                    else if (equipo.EstadoPing == 1)
                    {
                        res.EstadoPing = equipo.EstadoPing;
                        res.TiempoSincronizacion = DateTime.Now;
                        res.Active = "A";
                    }
                    _context.Entry(res).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    await _pinghub.Clients.All.SendAsync("SendPingEquipo", model);
                }
            }
            return Ok("Actualizado");
        }

        [HttpPost]
        [Route("UsuarioTempIngresar")]
        public async Task<IActionResult> IngresarUsuario([FromBody] UsuariosTemporales model)
        {
            string Sentencia = "exec SP_Servicios " +
                "@id_SP = 1" +
                ",@Usuario = '" +model.Usuario +
                "',@UserName = '" + model.UserName +
                "',@IPMachineSolicitud = '" + model.IpMachineSolicitud + "'";
            var response = await _context.RespuestaSentencia.FromSqlRaw(Sentencia).ToArrayAsync();
            return Ok(response);
        }

        [HttpPost]
        [Route("EquipoTempIngresar")]
        public async Task<IActionResult> IngresarEquipo([FromBody] EquiposTemporalesResponse model)
        {
            string Sentencia = "exec SP_Servicios " +
                "@id_SP = 2" +
                ",@serieEquipo = '" + model.SerieEquipo +
                "',@serieEquiponew = '" + model.SerieEquiponew +
                "',@IPMachineSolicitud = '" + model.IPMachineSolicitud + "'";
            var response = await _context.RespuestaSentencia.FromSqlRaw(Sentencia).ToArrayAsync();
            return Ok(response);
        }

        [HttpPost]
        [Route("ManualIngresar")]
        public async Task<IActionResult> IngresarManual([FromBody] ManualDepositos model)
        {
            string Sentencia = "exec SP_IngresoManualDepositos "+model;
            var response = await _context.RespuestaSentencia.FromSqlRaw(Sentencia).ToArrayAsync();
            //
            //var response = await _context.Database.ExecuteStoredProcedure(Sentencia).ToArrayAsync();
            return Ok(response);
        }
    }
}
