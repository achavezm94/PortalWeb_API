using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;
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
        public async Task<IEnumerable<ObtenerClientesResult>> ObtenerCliente()
        {
            return await _context.GetProcedures().ObtenerClientesAsync();
        }

        [HttpGet]
        [Route("ObtenerCuentaCliente/{id:int}")]
        public IActionResult ObtenerCuentaCliente(int id)
        {
            var Datos = from cli in _context.Clientes
                        join cb in _context.CuentasBancarias on cli.CodigoCliente equals cb.CodigoCliente
                        where cli.Id == id
                        select new
                        {
                            id = cb.Id,
                            clienteID = cli.Id,
                            codigoCliente = cb.CodigoCliente,
                            codcuentacontable = cb.Codcuentacontable,
                            nombanco = cb.Nombanco,
                            numerocuenta = cb.Numerocuenta,
                            tipoCuenta = cb.TipoCuenta,
                            observacion = cb.Observacion,
                            fecrea = cb.Fecrea
                        };
            return (Datos != null) ? Ok(Datos) : NotFound("No existe cuenta bancaria");
            /*
            List<CuentasBancariasIDCliente> _cuentasBancariasIDcliente = new();
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
            */
        }

        [HttpPost]
        [Route("GuardarCliente")]
        public async Task<IActionResult> GuardarCliente([FromBody] Clientes model)
        {
            if (ModelState.IsValid)
            {
                await _context.Clientes.AddAsync(model);
                return (await _context.SaveChangesAsync() > 0) ?  Ok("Se guardo") : BadRequest("Datos incorrectos");
                /*
                if (await _context.SaveChangesAsync() > 0)
                {                    
                    return Ok(model);
                }
                else
                {
                    return BadRequest("Datos incorrectos");
                }*/
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpPut]
        [Route("ActualizarCliente")]
        public IActionResult ActualizarCliente([FromBody] Clientes model)
        {
            var update = _context.Clientes
                            .Where(u => u.NombreCliente.Equals(model.NombreCliente))
                            .ExecuteUpdate(u => u
                            .SetProperty(u => u.NombreCliente, model.NombreCliente)
                            .SetProperty(u => u.Ruc, model.Ruc)
                            .SetProperty(u => u.Direccion, model.Direccion)
                            .SetProperty(u => u.Telefcontacto, model.Telefcontacto)
                            .SetProperty(u => u.Emailcontacto, model.Emailcontacto)
                            .SetProperty(u => u.Nombrecontacto, model.Nombrecontacto));
            return (update != 0) ? Ok("Se actualizo") : BadRequest("No se pudo actualizar");
            /*
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
            }*/
        }

        [HttpDelete]
        [Route("BorrarCliente/{id:int}")]
        public IActionResult BorrarCliente(int id)
        {
            var delete = _context.Clientes
                            .Where(b => b.Id.Equals(id))
                            .ExecuteDelete();
            return (delete != 0) ? Ok("Se borro") : BadRequest("No se pudo eliminar");
            /*
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
            }*/
        }
    }
}
