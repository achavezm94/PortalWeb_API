using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterEquiposController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public MasterEquiposController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ObtenerEquipos")]
        public async Task<IActionResult> ObtenerEquipos()
        {
            if (ModelState.IsValid)
            {
                var equipos = await _context.MasterEquipos.ToListAsync();
                return (equipos != null) ? Ok(equipos) : NotFound();
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpPost]
        [Route("GuardarEquipo")]
        public async Task<IActionResult> GuardarEquipo([FromBody] MasterEquipos model)
        {
            if (ModelState.IsValid)
            {
                await _context.MasterEquipos.AddAsync(model);
                return (await _context.SaveChangesAsync() > 0) ? Ok("Se guardo") : BadRequest("Datos incorrectos");
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpPut]
        [Route("ActualizarEquipo")]
        public IActionResult ActualizarEquipo([FromBody] MasterEquipos model)
        {
            if (ModelState.IsValid)
            {
                var update = _context.MasterEquipos
                            .Where(u => u.CodigoEquipo.Equals(model.CodigoEquipo))
                            .ExecuteUpdate(u => u
                            .SetProperty(u => u.NombreEquipo, model.NombreEquipo)
                            .SetProperty(u => u.CapacidadBolsillo, model.CapacidadBolsillo)
                            .SetProperty(u => u.VelocidadConteo, model.VelocidadConteo));
                return (update != 0) ? Ok("Se actualizo") : BadRequest("No se pudo actualizar");
            }
            else
            {
                return BadRequest("Error");
            }
            /*
            var result = await _context.MasterEquipos.FindAsync(model.CodigoEquipo);
            if (result != null)
            {
                try
                {               
                    result.NombreEquipo = model.NombreEquipo;
                    result.CapacidadBolsillo = model.CapacidadBolsillo;
                    result.VelocidadConteo = model.VelocidadConteo;
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
        [Route("BorrarEquipo/{id:int}")]
        public IActionResult BorrarEquipo(int id)
        {
            var delete = _context.MasterEquipos
                            .Where(b => b.Id.Equals(id))
                            .ExecuteDelete();
            return (delete != 0) ? Ok("Se borro") : BadRequest("No se pudo eliminar");

            /*
            var result = await _context.MasterEquipos.FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                _context.MasterEquipos.Remove(result);
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
            */
        }
    }
}
