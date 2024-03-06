using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using System.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/UsuarioTemporal")]
    [ApiController]
    public class ObtenerUsuarioTemporalController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public ObtenerUsuarioTemporalController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Usuario/{ip}")]
        public IActionResult ObtenerUsuarioTemporal([FromRoute] string ip)
        {
            string Sentencia = "exec SP_ObtenerUsuarioTemp @IP ";

            DataTable dt = new();
            using (SqlConnection connection = new(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new(Sentencia, connection);
                SqlDataAdapter adapter = new(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@IP", ip));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear");
            }
            return Ok(dt);
        }

        [HttpGet]
        [Route("UsuarioDelete/{id}")]
        public async Task<IActionResult> BorrarUsuarioTemporal([FromRoute] int id)
        {
            var result = await _context.UsuariosTemporales.FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                result.Active = "F";
                _context.UsuariosTemporales.Update(result);
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
