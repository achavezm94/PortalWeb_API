using Microsoft.AspNetCore.Mvc;
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
        [Route("ObtenerCuentaCliente/{id:int}")]
        public async Task<IActionResult> ObtenerCuentaCliente(int id)
        {
            List<CuentasBancariasIDCliente> _cuentasBancariasIDcliente = new List<CuentasBancariasIDCliente>();
            if (ModelState.IsValid)
            {
                var result = await _context.Clientes.FirstOrDefaultAsync(e => e.Id == id);

                if (result == null)
                {
                    return NotFound("No existe cliente");
                }
                else
                {
                    var cuentasBancarias = _context.CuentasBancarias.Where(x => x.CodigoCliente == result.CodigoCliente).ToList();
                    foreach (var cuenta in cuentasBancarias)
                    {
                        _cuentasBancariasIDcliente.Add(new CuentasBancariasIDCliente{
                            Id = cuenta.Id,
                            TipoCuenta = cuenta.TipoCuenta,
                            ClienteID = id,
                            Numerocuenta = cuenta.Numerocuenta,
                            Codcuentacontable = cuenta.Codcuentacontable,
                            CodigoCliente = cuenta.CodigoCliente,
                            Nombanco = cuenta.Nombanco,
                            Observacion = cuenta.Observacion,
                            Fecrea = cuenta.Fecrea
                        });
                    }
                    if (cuentasBancarias == null)
                    {
                        return NotFound("No existe cuenta bancaria");
                    }
                    else
                    {
                        return Ok(_cuentasBancariasIDcliente);
                    }
                    
                };    
            }
            else
            {
                return BadRequest("ERROR");
            }
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
