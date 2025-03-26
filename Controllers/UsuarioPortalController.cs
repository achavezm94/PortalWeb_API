using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;
using System.Data;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion Usuarios del Portal.
    /// </summary>
    [Route("api/UsuarioPortal")]
    [ApiController]
    public class UsuarioPortalController : ControllerBase
    {
        private readonly PortalWebContext _context;

        /// <summary>
        /// Extraer el context de EF.
        /// </summary>
        public UsuarioPortalController(PortalWebContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los usuarios del Portal registrados.
        /// </summary>
        /// <returns>Lista de datos de los usuarios del Portal registrados.</returns>
        /// <response code="200">Devuelve lista de datos de los usuarios del Portal registrados.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel2")]
        [HttpGet("ObtenerUsuario")]
        public IActionResult ObtenerUsuario()
        {
            try
            {
                var Datos = (from usp in _context.Usuarios_Portal.AsNoTracking()
                             join mt1 in _context.MasterTable.AsNoTracking() on usp.Rol equals mt1.codigo
                             join dp in _context.Datos_Personales.AsNoTracking() on usp.Usuario equals dp.UsuarioPortaidFk
                             select new
                             {
                                 usp.id,
                                 usp.Usuario,
                                 usp.Contrasenia,
                                 usp.Rol,
                                 usp.Active,
                                 rolNombre = mt1.nombre,
                                 idDatosPersonales = dp.id,
                                 dp.UsuarioPortaidFk,
                                 dp.Nombres,
                                 dp.Apellidos,
                                 dp.Cedula,
                                 dp.Telefono
                             });
                return Ok(Datos);
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Guarda un usuario nuevo del portal a la plataforma.
        /// </summary>        
        /// <response code="200">Se registro el usuario a la plataforma.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPost("GuardarUsuario")]
        public async Task<IActionResult> GuardarUsuario([FromBody] UserPortal_DatosPersonales model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _context.Usuarios_Portal.AddAsync(new Usuarios_Portal { Usuario = model.Usuario, Contrasenia = model.Contrasenia, Rol = model.Rol });
                    await _context.Datos_Personales.AddAsync(new Datos_Personales { Nombres = model.Nombres, Apellidos = model.Apellidos, Telefono = model.Telefono, Cedula = model.Cedula, UsuarioPortaidFk = model.Usuario, UsuarioidFk = "NULL" });
                    return (await _context.SaveChangesAsync() > 0) ? Ok() : BadRequest();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Actualiza información de un usuario del portal.
        /// </summary>
        /// <response code="200">Actualizo correctamente el registro.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPut("ActualizarUsuario/{id}")]
        public async Task<IActionResult> ActualizarUsuario([FromRoute] int id, [FromBody] Usuarios_Portal model)
        {
            try
            {
                if (id != model.id)
                {
                    return BadRequest();
                }
                _context.Entry(model).State = EntityState.Modified;
                return (await _context.SaveChangesAsync() > 0) ? Ok() : BadRequest();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Elimina un usuario del portal.
        /// </summary>
        /// <response code="200">Elimino correctamente el registro.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpDelete("BorrarUsuario/{id}")]
        public async Task<IActionResult> BorrarUsuario([FromRoute] int id)
        {
            try
            {
                var result = await _context.Usuarios_Portal.FirstOrDefaultAsync(e => e.id == id);
                var delete = _context.Usuarios_Portal
                               .Where(b => b.id.Equals(id))
                               .ExecuteDelete();
                if (delete != 0 && result is not null)
                {
                    var deleteDatos = _context.Datos_Personales
                              .Where(b => b.UsuarioPortaidFk.Equals(result.Usuario))
                              .ExecuteDelete();
                    return (deleteDatos != 0) ? Ok() : BadRequest();
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }
    }
}