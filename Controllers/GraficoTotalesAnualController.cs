using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using System.Data;

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
        public IActionResult ObtenerEquipo([FromRoute] string serieEquipo)
        {
            string Sentencia = "exec SP_GraficoTotalesAño @serieEquipo";

            DataTable dt = new();
            using (SqlConnection connection = new(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new(Sentencia, connection);
                SqlDataAdapter adapter = new(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@serieEquipo", serieEquipo));
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
