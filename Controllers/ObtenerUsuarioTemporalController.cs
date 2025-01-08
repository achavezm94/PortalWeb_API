﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using System.Data;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion Usuarios Temporales.
    /// </summary>
    [Route("api/UsuarioTemporal")]
    [ApiController]
    public class ObtenerUsuarioTemporalController : ControllerBase
    {
        private readonly PortalWebContext _context;

        /// <summary>
        /// Extraer el context de EF.
        /// </summary>
        public ObtenerUsuarioTemporalController(PortalWebContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los usuarios temporales que aun no estan ingresados a la plataforma.
        /// </summary>
        /// <returns>Lista de usuarios temporales para ingresar a plataforma.</returns>
        /// <response code="200">Devuelve la lista de usuarios temporales.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel2")]
        [HttpGet("Usuario/{ip}")]
        public IActionResult ObtenerUsuarioTemporal(string ip)
        {
            var Datos = from ut in _context.UsuariosTemporales
                        where ut.IpMachineSolicitud.Equals(ip) && ut.Active.Equals("A")
                        select new { ut.id, ut.Usuario, ut.IpMachineSolicitud, ut.fecrea };
            return (Datos != null) ? Ok(Datos) : NotFound();
        }

        /// <summary>
        /// Borrar un usuario temporal.
        /// </summary>
        /// <response code="200">Borro correctamente el usuario temporal.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpGet("UsuarioDelete/{id}")]
        public IActionResult BorrarUsuarioTemporal([FromRoute] int id)
        {
            var update = _context.UsuariosTemporales
                           .Where(u => u.id.Equals(id))
                           .ExecuteUpdate(u => u
                           .SetProperty(u => u.Active, "F"));
            return (update != 0) ? Ok() : BadRequest();
        }
    }
}