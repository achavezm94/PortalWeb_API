﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;
using System.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/Usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public UsuarioController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpGet("ObtenerUsuario")]
        public IActionResult ObtenerUsuario()
        {

            string Sentencia = "exec sp_obtenerUsuarios 2";

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
                return NotFound("No existe busqueda...");
            }

            return Ok(dt);
        }

        [HttpGet("ObtenerUsuarioIP/{ip}")]
        public IActionResult ObtenerUsuarioIp([FromRoute] string ip)
        {
            string Sentencia = " exec sp_obtenerUsuarios 3, @IPMachineSolicitud";

            DataTable dt = new();
            using (SqlConnection connection = new(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new(Sentencia, connection);
                SqlDataAdapter adapter = new(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@IPMachineSolicitud", ip));

                adapter.Fill(dt);
            }
            if (dt == null)
            {
                return NotFound("No existe busqueda...");
            }
            return Ok(dt);

        }

        [HttpPost]
        [Route("GuardarUsuario")]
        public async Task<IActionResult> GuardarUsuario([FromBody] UserPortal_DatosPersonales model)
        {
            if (ModelState.IsValid)
            {
                Usuarios usuarios = new()
                {
                    Usuario = model.Usuario,
                    Contrasenia = model.Contrasenia,
                    IpMachine = model.IpMachine,
                    TiendasidFk = model.Rol,
                    CuentasidFk = model.CuentasidFk,
                    Observacion = model.Observacion
                };
                DatosPersonales datosPersonales = new()
                {
                    Nombres = model.Nombres,
                    Apellidos = model.Apellidos,
                    Telefono = model.Telefono,
                    Cedula = model.Cedula,
                    UsuarioPortaidFk = "NULL",
                    UsuarioidFk = model.Usuario,
                };
                await _context.Usuarios.AddAsync(usuarios);
                int total = _context.DatosPersonales.Count(t => t.UsuarioidFk == datosPersonales.UsuarioidFk && t.Nombres == datosPersonales.Nombres && t.Cedula == datosPersonales.Cedula);
                if (total == 0)
                {
                    await _context.DatosPersonales.AddAsync(datosPersonales);
                }
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

        [HttpPut]
        [Route("ActualizarUsuario/{id}")]
        public async Task<IActionResult> ActualizarUsuario([FromRoute] int id, [FromBody] Usuarios model)
        {
            if (id != model.Id)
            {
                return BadRequest("No existe el usuario");
            }
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(model);
        }


        [HttpPut]
        [Route("ActualizarDatosPersonales/{id}")]
        public async Task<IActionResult> ActualizarDatosPersonales([FromRoute] int id, [FromBody] DatosPersonales model)
        {
            if (id != model.Id)
            {
                return BadRequest("No existe el usuario");
            }
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(model);
        }
    }
}
