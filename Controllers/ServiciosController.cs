using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Collection;
using PortalWeb_API.Data;
using PortalWeb_API.Models;
using System.Data;

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
        public IActionResult IngresarManual([FromBody] OManual model)
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
                if (returnParameter.Value.ToString() == "0")
                {
                    return Ok(0);
                }
            }
            return Ok(1);
        }

        [HttpPost]
        [Route("DepositoIngresar")]
        public IActionResult IngresarDeposito([FromBody] ODeposito model)
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
            using (SqlConnection conn = new(_context.Database.GetDbConnection().ConnectionString))
            {
                SqlCommand sqlCmd = new SqlCommand("dbo.SP_IngresoDepositos");
                conn.Open();
                sqlCmd.Connection = conn;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add(param);
                var returnParameter = sqlCmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                int result = sqlCmd.ExecuteNonQuery();
                if (returnParameter.Value.ToString() == "0")
                {
                    return Ok(0);
                }
            }
            return Ok(1);
        }

        [HttpPost]
        [Route("RecoleccionIngresar")]
        public IActionResult IngresarRecoleccion([FromBody] ORecoleccion model)
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
            using (SqlConnection conn = new(_context.Database.GetDbConnection().ConnectionString))
            {
                SqlCommand sqlCmd = new SqlCommand("dbo.SP_IngresoRecolecciones");
                conn.Open();
                sqlCmd.Connection = conn;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add(param);
                var returnParameter = sqlCmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                int result = sqlCmd.ExecuteNonQuery();
                if (returnParameter.Value.ToString() == "0")
                {
                    return Ok(0);
                }
            }
            return Ok(1);
        }
    }
}
