using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using System.Data;

namespace PortalWeb_API.Controllers
{

    [Route("api/TiendaCuenta")]
    [ApiController]
    public class TiendaCuentaController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public TiendaCuentaController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ObtenerTiendaCuentas/{id}")]
        public IActionResult ObtenerTiendaCuentas([FromRoute] string id)
        {

            string Sentencia = "EXEC SP_ObtenerTiendasCuentas @Ids";

            DataTable dt = new();
            using (SqlConnection connection = new(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new(Sentencia, connection);
                SqlDataAdapter adapter = new(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@IDS", id));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("No se ha podido obtener...");
            }

            return Ok(dt);

        }

        [HttpDelete]
        [Route("BorrarCuentaTienda/{id}")]
        public async Task<IActionResult> BorrarCuentaTienda([FromRoute] int id)
        {
            var result = await _context.CuentaSignaTienda.FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                _context.CuentaSignaTienda.Remove(result);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return NoContent();
                }
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
