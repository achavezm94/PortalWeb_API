using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using System.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/DataMaster")]
    [ApiController]
    public class MasterTableController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public MasterTableController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpGet("GetDataMaster/{mast}")]
        public IActionResult GetDataMaster([FromRoute] string mast)
        {

            string Sentencia = " select rtrim(ltrim(master)) as master, rtrim(ltrim( codigo )) as codigo," +
                               " rtrim(ltrim( nombre )) as nombre from MasterTable " +
                               " where master = @master group by master, codigo, nombre ";

            DataTable dt = new();
            using (SqlConnection connection = new(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@master", mast));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }
            return Ok(dt);
        }


        [HttpGet("ObtenerDatamasterGrupo/{grupo}")]
        public IActionResult ObtenerDatamasterGrupo([FromRoute] string grupo)
        {
            string Sentencia = " exec obtenerGruposMarcasMaq @codtipo, '', 1 ";
            DataTable dt = new();
            using (SqlConnection connection = new(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@codtipo", grupo));
                    adapter.Fill(dt);
                }
            }
            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }
            return Ok(dt);
        }

        [HttpGet("ObtenerDatamasterSubGrupos/{grupo}/{sgrupo}")]
        public IActionResult ObtenerDatamasterSubGrupos([FromRoute] string grupo, [FromRoute] string sgrupo)
        {
            string Sentencia = " exec obtenerGruposMarcasMaq @gr, @sgr, 2 ";
            DataTable dt = new();
            using (SqlConnection connection = new(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@gr", grupo));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@sgr", sgrupo));
                    adapter.Fill(dt);
                }
            }
            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }
            return Ok(dt);
        }
    }
}
