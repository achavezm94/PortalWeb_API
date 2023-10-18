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
        readonly double moneda1 = 2.50, 
                        moneda5 = 5.00, 
                        moneda10 = 2.27, 
                        moneda25 = 5.67, 
                        moneda50 = 11.34, 
                        moneda100 = 8.10;
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

        [HttpPut]
        [Route("ActualizarEquipoIp")]
        public async Task<IActionResult> ActualizarEquipo([FromBody] string ip)
        {
            List<MonitoreoModel> resultado = new List<MonitoreoModel>();
            int estadoPingActual = 0;
            var res = await _context.Equipos.FirstOrDefaultAsync(x => x.IpEquipo == ip);
            if (res != null)
            {
                res.EstadoPing = 1;
                res.TiempoSincronizacion = DateTime.Now;
                res.Active = "A";
                _context.Entry(res).State = EntityState.Modified;
                if (await _context.SaveChangesAsync() > 0)
                {
                    var data = (from x in _context.Equipos
                                where x.Active == "A"
                                select new { ipEquipo = x.IpEquipo, tiempoSincronizacion = x.TiempoSincronizacion }).ToList();
                    foreach (var item in data)
                    {
                        if (item.tiempoSincronizacion is not null)
                        {
                            TimeSpan ts = DateTime.Now.Subtract((DateTime)item.tiempoSincronizacion);
                            if (ts.TotalMinutes < 0.2) estadoPingActual = 1;
                            resultado.Add(new MonitoreoModel()
                            {
                                ip = item.ipEquipo,
                                estadoPing = estadoPingActual
                            });
                        }
                        estadoPingActual = 0;
                    }
                    await _pinghub.Clients.All.SendAsync("SendPingEquipo", resultado);
                    return Ok("Actualizado");
                }
                return BadRequest("No se guardó");
            }
            return NoContent();
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

                        var sp_response = await _context.GetProcedures().SP_TablaTransaccionalPrincipalAsync(ultimoDeposito?.MachineSn, 4);

                        var sp_responseDatos = await _context.GetProcedures().SP_DatosEquiposFrontAsync(ultimoDeposito?.MachineSn);

                        double? peso = ((ultimoDeposito?.TotalManualDepositoCoin1 * moneda1) +
                                       (ultimoDeposito?.TotalManualDepositoCoin5 * moneda5) +
                                       (ultimoDeposito?.TotalManualDepositoCoin10 * moneda10)+
                                       (ultimoDeposito?.TotalManualDepositoCoin25 * moneda25)+
                                       (ultimoDeposito?.TotalManualDepositoCoin25 * moneda50)+
                                       (ultimoDeposito?.TotalManualDepositoCoin100 * moneda100)) * 0.0022;

                        if (ultimoDeposito is not null && peso is not null && sp_response is not null)
                        {
                            List<object> list = new()
                            {
                                ultimoDeposito,
                                peso,
                                sp_response,
                                sp_responseDatos
                            };
                            await _manualesHub.Clients.All.SendAsync("SendTransaccionManual", list);
                        }
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
                        var sp_response = await _context.GetProcedures().SP_TablaTransaccionalPrincipalAsync(ultimoDeposito?.MachineSn, 3);
                        var sp_responseDatos = await _context.GetProcedures().SP_DatosEquiposFrontAsync(ultimoDeposito?.MachineSn);
                        if (ultimoDeposito is not null && sp_response is not null)
                        {
                            List<object> list = new()
                            {
                                ultimoDeposito,
                                sp_response,
                                sp_responseDatos
                            };
                            await _autoTranhub.Clients.All.SendAsync("SendTransaccionAuto", list);
                        }
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

                        var sp_response = await _context.GetProcedures().SP_TablaTransaccionalPrincipalAsync(ultimoDeposito?.MachineSn, 5);
                        var sp_responseDatos = await _context.GetProcedures().SP_DatosEquiposFrontAsync(ultimoDeposito?.MachineSn);
                        if (ultimoDeposito is not null && sp_response is not null)
                        {
                            List<object> list = new()
                            {
                                ultimoDeposito,
                                sp_response,
                                sp_responseDatos
                            };
                            await _recoleccionHub.Clients.All.SendAsync("SendTransaccionRecoleccion", list);
                        }
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
