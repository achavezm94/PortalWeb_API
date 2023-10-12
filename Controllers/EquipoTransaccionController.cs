using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using System.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/EquipoDetalle")]
    [ApiController]
    public class EquipoTransaccionController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public EquipoTransaccionController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpGet("ObtenerDetalle/{id}")]
        public IActionResult ObtenerDetalle(string id)
        {
            string Sentencia = " exec SP_DatosEquiposFront @id_equipo";
            DataTable dt = new();
            using (SqlConnection connection = new(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@id_equipo", id));
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
