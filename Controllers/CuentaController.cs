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
        public async Task<IActionResult> ObtenerCuenta()
        {
            if (ModelState.IsValid)
            {
                var cuentasBancarias = await _context.CuentasBancarias.ToListAsync();
                if (cuentasBancarias == null)
                {
                    return NotFound();
                }
                return Ok(cuentasBancarias);
            }
            else
            {
                return BadRequest("ERROR");
            }
        }

        [HttpGet]
        [Route("NTransacciones/{id}")]
        public IActionResult GenerarTransaccionesAcreeditadas([FromRoute] int id)
        {
            var Datos = (from cb in _context.CuentasBancarias
                        join cs in _context.CuentaSignaTienda on cb.Id equals cs.Idcuentabancaria
                        join t in _context.Tiendas on cs.Idtienda equals t.CodigoTienda
                        join e in _context.Equipos on t.Id equals e.CodigoTiendaidFk
                        join d in _context.Depositos on e.SerieEquipo equals d.MachineSn
                        where cb.Id.Equals(id)
                        select d.TransaccionNo).FirstOrDefault().ToString();
            return (Datos is not null) ? Ok(Datos) : NotFound("No se pudo encontrar");
        }

        [HttpPost]
        [Route("GuardarCuenta")]
        public async Task<IActionResult> GuardarCuenta([FromBody] CuentasBancarias model)
        {
            if (ModelState.IsValid)
            {
                await _context.CuentasBancarias.AddAsync(model);

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
        [Route("ActualizarCuenta")]
        public async Task<IActionResult> ActualizarCuenta([FromBody] CuentasBancarias model)
        {
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
            }
        }

        [HttpDelete]
        [Route("BorrarCuenta/{id:int}")]
        public async Task<IActionResult> BorrarCuenta(int id)
        {
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
            }
        }
    }
}
