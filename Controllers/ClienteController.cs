﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;
using System;
using System.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/Cliente")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public ClienteController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ObtenerCliente")]
        public IActionResult ObtenerCliente()
        {

            string Sentencia = "exec [ObtenerClientes]";

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

        [HttpGet]
        [Route("ObtenerCuentaCliente/{CodCliente}")]
        public IActionResult ObtenerCuentaCliente(string CodCliente)
        {
            var Datos = from cli in _context.Clientes
                        join cb in _context.CuentasBancarias on cli.CodigoCliente equals cb.CodigoCliente
                        where cli.CodigoCliente == CodCliente
                        select new
                        {
                            cb.Id,
                            ClienteID = cli.Id,
                            cb.CodigoCliente,
                            cb.Codcuentacontable,
                            cb.Nombanco,
                            cb.Numerocuenta,
                            cb.TipoCuenta,
                            cb.Observacion,
                            cb.Fecrea
                        };
            return (Datos != null) ? Ok(Datos) : NotFound("No existe cuenta bancaria");
        }

        [HttpPost]
        [Route("GuardarCliente")]
        public async Task<IActionResult> GuardarCliente([FromBody] Clientes model)
        {
            if (ModelState.IsValid)
            {
                await _context.Clientes.AddAsync(model);                
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
        [Route("ActualizarCliente")]
        public async Task<IActionResult> ActualizarCliente([FromBody] Clientes model)
        {
            var result = await _context.Clientes.FindAsync(model.CodigoCliente);

            if (result != null)
            {
                try
                {
                    result.NombreCliente = model.NombreCliente;
                    result.Ruc = model.Ruc;
                    result.Direccion = model.Direccion;
                    result.Telefcontacto = model.Telefcontacto;
                    result.Emailcontacto = model.Emailcontacto;
                    result.Nombrecontacto = model.Nombrecontacto;
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

        [HttpDelete]
        [Route("BorrarCliente/{id:int}")]
        public async Task<IActionResult> BorrarCliente(int id)
        {
            var result = await _context.Clientes.FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                _context.Clientes.Remove(result);
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