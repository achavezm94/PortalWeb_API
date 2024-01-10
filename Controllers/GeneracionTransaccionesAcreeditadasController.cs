using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;
using System.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/Acreeditacion")]
    [ApiController]
    public class GeneracionTransaccionesAcreeditadasController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public GeneracionTransaccionesAcreeditadasController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GenerarCard")]
        public IActionResult GenerarCard()
        {
            string Sentencia = "SELECT [NombreArchivo] ,[FechaRegistro] ,[FechaIni] ,[FechaFin] ,[UsuarioRegistro]" +
                                "FROM [PortalWeb].[dbo].[TransaccionesAcreditadas]" +
                                "Group by [NombreArchivo] ,[FechaRegistro] ,[FechaIni] ,[FechaFin] ,[UsuarioRegistro] ";

            DataTable dt = new();
            using (SqlConnection connection = new(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new(Sentencia, connection);
                SqlDataAdapter adapter = new(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }
            return Ok(dt);
        }

        [HttpGet]
        [Route("GenerarTransacciones/{nombreArchivo}")]
        public IActionResult GenerarTransaccionesAcreeditadas([FromRoute] string nombreArchivo)
        {
            string Sentencia = "select [NoTransaction],[Machine_Sn]"+
                                "FROM [PortalWeb].[dbo].[TransaccionesAcreditadas]" +
                                "where NombreArchivo = @nomArchivo";

            DataTable dt = new();
            using (SqlConnection connection = new(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new(Sentencia, connection);
                SqlDataAdapter adapter = new(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@nomArchivo", nombreArchivo));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }
            return Ok(dt);
        }
    }
}
