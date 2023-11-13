using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;
using System.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/UsuarioPortal")]
    //[Authorize]
    [ApiController]
    public class UsuarioPortalController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public UsuarioPortalController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpGet("ObtenerUsuario")]
        public IActionResult ObtenerUsuario()
        {

            string Sentencia = "exec sp_obtenerUsuarios 1";

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

        [HttpPut]
        [Route("ActualizarUsuario/{id}")]
        public async Task<IActionResult> ActualizarUsuario([FromRoute] int id, [FromBody] UsuariosPortal model)
        {
            /*if (id != model.Id)
            {
                return BadRequest("No existe el usuario portal");
            }
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(model);*/

            var result = await _context.UsuariosPortal.FindAsync(model.Id);
            if (result != null)
            {
                try
                {
                    result.Usuario = model.Usuario;
                    result.Contrasenia = model.Contrasenia;
                    result.Rol = model.Rol;
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest();
                }
                finally
                {
                    await _context.SaveChangesAsync();
                }
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("GuardarUsuario")]
        public async Task<IActionResult> GuardarUsuario([FromBody] UserPortal_DatosPersonales model)
        {
            if (ModelState.IsValid)
            {
                UsuariosPortal usuariosPortal = new(){
                    Usuario = model.Usuario,
                    Contrasenia = model.Contrasenia,
                    Rol = model.Rol
                };
                await _context.UsuariosPortal.AddAsync(usuariosPortal);
                DatosPersonales datosPersonales = new()
                {
                    Nombres = model.Nombres,
                    Apellidos = model.Apellidos,
                    Telefono = model.Telefono, 
                    Cedula = model.Cedula,
                    UsuarioPortaidFk = model.Usuario,
                    UsuarioidFk = "NULL",
                };
                await _context.DatosPersonales.AddAsync(datosPersonales);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return Ok(model);
                }
                else
                {
                    return BadRequest("Datos incorrectos");
                }
            }
            else
            {
                return BadRequest("ERROR");
            }
        }


        [HttpDelete]
        [Route("BorrarUsuario/{id}")]
        public async Task<IActionResult> BorrarUsuario([FromRoute] int id)
        {
            var result = await _context.UsuariosPortal.FirstOrDefaultAsync(e => e.Id == id);            
            if (result != null)
            {
                var resultDatos = await _context.DatosPersonales.FirstOrDefaultAsync(e => e.UsuarioPortaidFk == result.Usuario);
                _context.UsuariosPortal.Remove(result);
                if (resultDatos != null)
                {
                    _context.DatosPersonales.Remove(resultDatos);
                }
                else
                {
                    return NotFound();
                }
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
