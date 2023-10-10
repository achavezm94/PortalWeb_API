using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Collection;
using PortalWeb_API.Data;
using PortalWeb_API.Models;
using PortalWeb_APIs;
using System.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/Servicios")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private readonly PortalWebContext _context;
        private readonly IHubContext<PingHubEquipos> _pinghub;
        private readonly IHubContext<AutomaticoTransaHUb> _autoTranhub;
        private readonly IHubContext<ManualesHub> _manualesHub;
        private readonly IHubContext<RecoleccionHub> _recoleccionHub;
        private readonly IHubContext<UsuarioHub> _usuarioHub;
        public ServiciosController(PortalWebContext context, IHubContext<PingHubEquipos> pingHub, IHubContext<AutomaticoTransaHUb> autoTranhub, IHubContext<ManualesHub> manualesHub, IHubContext<RecoleccionHub> recoleccionHub, IHubContext<UsuarioHub> usuarioHub)
        {
            _context = context;
            _pinghub = pingHub;
            _autoTranhub = autoTranhub;
            _manualesHub = manualesHub;
            _recoleccionHub = recoleccionHub;
            _usuarioHub = usuarioHub;
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
                    else if (equipo.EstadoPing == 2)
                    {
                        res.EstadoPing = equipo.EstadoPing;
                        res.Active = "A";
                    }
                    _context.Entry(res).State = EntityState.Modified;
                    if (await _context.SaveChangesAsync() > 0) 
                    {
                        await _pinghub.Clients.All.SendAsync("SendPingEquipo", model);
                    }
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
                ",@Usuario = '" + model.Usuario +
                "',@UserName = '" + model.UserName +
                "',@IPMachineSolicitud = '" + model.IpMachineSolicitud + "'";
            var response = await _context.RespuestaSentencia.FromSqlRaw(Sentencia).ToArrayAsync();
            if (response.Length > 0)
            {
                var ultimoUsuario = _context.UsuariosTemporales.FirstOrDefault(d => d.Usuario == model.Usuario && d.IpMachineSolicitud == model.IpMachineSolicitud);
                await _usuarioHub.Clients.All.SendAsync("SendUsuarioTemporal", ultimoUsuario);
                return Ok(response);
            }
            else 
            {
                return BadRequest("No se registro");
            }
            
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
            if (response.Length > 0)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("No se registro");
            }
        }

        [HttpPost]
        [Route("ManualIngresar")]
        public async Task<IActionResult> IngresarManualAsync([FromBody] OManual model)
        {
            ManualDepositosCollection manual_depositos = new()
            {
                model
            };

            SqlParameter param = new()
            {
                ParameterName = "Manual",
                SqlDbType = SqlDbType.Structured,
                Value = manual_depositos,
                Direction = ParameterDirection.Input
            };
            try
            {
                using (SqlConnection conn = new(_context.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand sqlCmd = new("dbo.SP_IngresoManualDepositos");
                    conn.Open();
                    sqlCmd.Connection = conn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.Add(param);
                    var returnParameter = sqlCmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    int result = sqlCmd.ExecuteNonQuery();
                    if (returnParameter.Value.ToString() == "1")
                    {
                        var ultimoDeposito = _context.ManualDepositos.FirstOrDefault(d => d.MachineSn == model.Machine_Sn && d.TransaccionNo == model.Transaction_no && d.UsuariosIdFk == model.User_id);
                        await _manualesHub.Clients.All.SendAsync("SendTransaccionManual", ultimoDeposito);
                        return Ok(1);
                        
                    }
                }
                return Ok(0);
            }
            catch (Exception)
            {
                return Ok(0);
            }
        }

        [HttpPost]
        [Route("DepositoIngresar")]
        public async Task<IActionResult> IngresarDepositoAsync([FromBody] ODeposito model)
        {
            DepositosCollection depositos = new()
            {
                model
            };

            SqlParameter param = new()
            {
                ParameterName = "Depositos",
                SqlDbType = SqlDbType.Structured,
                Value = depositos,
                Direction = ParameterDirection.Input
            };
            try
            {
                using (SqlConnection conn = new(_context.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand sqlCmd = new("dbo.SP_IngresoDepositos");
                    conn.Open();
                    sqlCmd.Connection = conn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.Add(param);
                    var returnParameter = sqlCmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    int result = sqlCmd.ExecuteNonQuery();
                    if (returnParameter.Value.ToString() == "1")
                    {
                        var ultimoDeposito = _context.Depositos.FirstOrDefault(d => d.MachineSn == model.Machine_Sn && d.TransaccionNo == model.Transaction_no && d.UsuariosIdFk == model.User_id);
                        await _autoTranhub.Clients.All.SendAsync("SendTransaccionAuto", ultimoDeposito);
                        
                        return Ok(1);
                    }
                }
                return Ok(0);
            }
            catch (Exception)
            {
                return Ok(0);
            }
            
        }

        [HttpPost]
        [Route("RecoleccionIngresar")]
        public async Task<IActionResult> IngresarRecoleccionAsync([FromBody] ORecoleccion model)
        {
            RecoleccionCollection recolecciones = new ()
            {
                model
            };

            SqlParameter param = new()
            {
                ParameterName = "Recoleccion",
                SqlDbType = SqlDbType.Structured,
                Value = recolecciones,
                Direction = ParameterDirection.Input
            };
            try
            {
                using (SqlConnection conn = new(_context.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand sqlCmd = new("dbo.SP_IngresoRecolecciones");
                    conn.Open();
                    sqlCmd.Connection = conn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.Add(param);
                    var returnParameter = sqlCmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    int result = sqlCmd.ExecuteNonQuery();
                    if (returnParameter.Value.ToString() == "1")
                    {
                        var ultimoDeposito = _context.Recolecciones.FirstOrDefault(d => d.MachineSn == model.Machine_Sn && d.TransaccionNo == model.Transaction_no && d.UsuariosIdFk == model.User_id);
                        await _recoleccionHub.Clients.All.SendAsync("SendTransaccionRecoleccion", ultimoDeposito);
                        return Ok(1);
                    }
                }
                return Ok(0);
            }
            catch (Exception)
            {
                return Ok(0);
            }            
        }
    }
}
