using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;
using System.Data;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion Usuarios de los Equipos.
    /// </summary>
    [Route("api/Usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly PortalWebContext _context;

        /// <summary>
        /// Extraer el context de EF.
        /// </summary>
        public UsuarioController(PortalWebContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los usuarios de los equipos registrados.
        /// </summary>
        /// <returns>Lista de datos de los usuarios de los equipos registrados.</returns>
        /// <response code="200">Devuelve lista de datos de los usuarios de los equipos registrados.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel2")]
        [HttpGet("ObtenerUsuario")]
        public IActionResult ObtenerUsuario()
        {
            try
            {
                var Datos = (from usm in _context.Usuarios.AsNoTracking()
                             join td in _context.Tiendas.AsNoTracking() on new { codigo = usm.TiendasidFk } equals new { codigo = (td.id).ToString() }
                             join mt1 in _context.MasterTable.AsNoTracking() on td.CodProv equals mt1.codigo
                             where mt1.master == "PRV00"
                             join dp in _context.Datos_Personales.AsNoTracking() on usm.Usuario equals dp.UsuarioidFk
                             join cli in _context.Clientes.AsNoTracking() on td.CodigoClienteidFk equals cli.CodigoCliente
                             select new
                             {
                                 usm.id,
                                 usm.Usuario,
                                 usm.TiendasidFk,
                                 usm.Active,
                                 usm.IpMachine,
                                 usm.CuentasidFk,
                                 usm.Observacion,
                                 cli.NombreCliente,
                                 td.NombreTienda,
                                 td.CodProv,
                                 nombreProvincia = mt1.nombre,
                                 idDatosPersonales = dp.id,
                                 dp.UsuarioidFk,
                                 dp.Nombres,
                                 dp.Cedula
                             });
                return Ok(Datos);
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Obtiene todos los usuarios de un equipo mediante su ip.
        /// </summary>
        /// <returns>Lista de datos de los usuarios de un equipo mediante ip.</returns>
        /// <response code="200">Devuelve lista de datos de los usuarios de un equipo mediante ip.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel2")]
        [HttpGet("ObtenerUsuarioIP/{ip}")]
        public IActionResult ObtenerUsuarioIp([FromRoute] string ip)
        {
            try
            {
                var Datos = (from usm in _context.Usuarios.AsNoTracking()
                             join td in _context.Tiendas.AsNoTracking() on new { codigo = usm.TiendasidFk } equals new { codigo = (td.id).ToString() }
                             join mt1 in _context.MasterTable.AsNoTracking() on td.CodProv equals mt1.codigo
                             where mt1.master == "CCAN"
                             join dp in _context.Datos_Personales.AsNoTracking() on usm.Usuario equals dp.UsuarioidFk
                             join cli in _context.Clientes.AsNoTracking() on td.CodigoClienteidFk equals cli.CodigoCliente
                             join cb in _context.cuentas_bancarias.AsNoTracking() on usm.CuentasidFk equals cb.id
                             where ip == usm.IpMachine
                             select new
                             {
                                 usm.id,
                                 usm.Usuario,
                                 usm.TiendasidFk,
                                 usm.Active,
                                 usm.IpMachine,
                                 usm.CuentasidFk,
                                 usm.Observacion,
                                 cli.NombreCliente,
                                 td.NombreTienda,
                                 td.CodProv,
                                 nombreProvincia = mt1.nombre,
                                 idDatosPersonales = dp.id,
                                 dp.UsuarioidFk,
                                 dp.Nombres,
                                 cb.numerocuenta,
                                 cb.nombanco,
                                 cb.TipoCuenta,
                                 dp.Cedula,
                             });
                return Ok(Datos);
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Guarda un usuario nuevo a la plataforma.
        /// </summary>        
        /// <response code="200">Se registro el usuario a la plataforma.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPost]
        [Route("GuardarUsuario")]
        public async Task<IActionResult> GuardarUsuario([FromBody] UserPortal_DatosPersonales model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _context.Usuarios.AddAsync(new Usuarios { Usuario = model.Usuario, Contrasenia = model.Contrasenia, IpMachine = model.IpMachine, TiendasidFk = model.Rol, CuentasidFk = model.CuentasidFk, Observacion = model.Observacion });
                    int total = _context.Datos_Personales.Count(t => t.UsuarioidFk == model.Usuario);
                    if (total == 0)
                    {
                        await _context.Datos_Personales.AddAsync(new Datos_Personales { Nombres = model.Nombres, Apellidos = model.Apellidos, Telefono = model.Telefono, Cedula = model.Cedula, UsuarioPortaidFk = "NULL", UsuarioidFk = model.Usuario });
                    }
                    return (await _context.SaveChangesAsync() > 0) ? Ok(model) : BadRequest();
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
        /// Actualiza información de un usuario.
        /// </summary>
        /// <response code="200">Actualizo correctamente el registro.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPut]
        [Route("ActualizarUsuario/{id}")]
        public async Task<IActionResult> ActualizarUsuario([FromRoute] int id, [FromBody] Usuarios model)
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
        /// Actualiza información de los datos personales de un usuario.
        /// </summary>
        /// <response code="200">Actualizo correctamente el registro.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPut]
        [Route("ActualizarDatosPersonales/{id}")]
        public async Task<IActionResult> ActualizarDatosPersonales([FromRoute] int id, [FromBody] Datos_Personales model)
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
    }
}