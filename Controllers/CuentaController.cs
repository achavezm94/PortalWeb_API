using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public CuentaController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ObtenerCuenta")]
        public IActionResult ObtenerCuenta()
        {
            var Datos = from cb in _context.CuentasBancarias select cb;
            return (Datos != null) ? Ok(Datos) : NotFound("No existe cuenta bancaria");
            /*
            var cuentasBancarias = await _context.CuentasBancarias.ToListAsync();
            if (cuentasBancarias == null)
            {
                return NotFound();
            }
            return Ok(cuentasBancarias);
            */
        }

        [HttpPost]
        [Route("GuardarCuenta")]
        public async Task<IActionResult> GuardarCuenta([FromBody] CuentasBancarias model)
        {
            if (ModelState.IsValid)
            {
                await _context.CuentasBancarias.AddAsync(model);
                return (await _context.SaveChangesAsync() > 0) ? Ok("Se guardo") : BadRequest("Datos incorrectos");
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpPut]
        [Route("ActualizarCuenta")]
        public IActionResult ActualizarCuenta([FromBody] CuentasBancarias model)
        {

            var update = _context.CuentasBancarias
                           .Where(u => u.Codcuentacontable.Equals(model.Codcuentacontable))
                           .ExecuteUpdate(u => u
                           .SetProperty(u => u.CodigoCliente, model.CodigoCliente)
                           .SetProperty(u => u.Nombanco, model.Nombanco)
                           .SetProperty(u => u.Numerocuenta, model.Numerocuenta)
                           .SetProperty(u => u.Observacion, model.Observacion)
                           .SetProperty(u => u.TipoCuenta, model.TipoCuenta));
            return (update != 0) ? Ok("Se actualizo") : BadRequest("No se pudo actualizar");

            /*
            var result = await _context.CuentasBancarias.FindAsync(model.Codcuentacontable);

            if (result != null)
            {
                try
                {
                    result.CodigoCliente = model.CodigoCliente;
                    result.Nombanco = model.Nombanco;
                    result.Numerocuenta = model.Numerocuenta;
                    result.Observacion = model.Observacion;
                    result.TipoCuenta = model.TipoCuenta;
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
        [Route("BorrarCuenta/{id:int}")]
        public IActionResult BorrarCuenta(int id)
        {
            var delete = _context.CuentasBancarias
                           .Where(b => b.Id.Equals(id))
                           .ExecuteDelete();
            return (delete != 0) ? Ok("Se borro") : BadRequest("No se pudo eliminar");

            /*
            var result = await _context.CuentasBancarias
                 .FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                _context.CuentasBancarias.Remove(result);
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
