using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
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
        [HttpGet("ObtenerIndicadores/{id}")]
        public IActionResult ObtenerIndicadores([FromRoute] int id)
        {
            string Sentencia = " EXEC SP_IndicadoresLlenado " +id;
            DataTable dt = new();
            using (SqlConnection connection = new(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
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
