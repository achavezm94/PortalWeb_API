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

                if (equipos == null)
                {
                    return NotFound();
                }
                return Ok(equipos);
            }
            else
            {
                return BadRequest("ERROR");
            }
        }

        [HttpPost]
        [Route("GuardarEquipo")]
        public async Task<IActionResult> GuardarEquipo([FromBody] MasterEquipos model)
        {
            if (ModelState.IsValid)
            {
                await _context.MasterEquipos.AddAsync(model);

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
        [Route("ActualizarEquipo")]
        public async Task<IActionResult> ActualizarEquipo([FromBody] MasterEquipos model)
        {
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
            }
        }

        [HttpDelete]
        [Route("BorrarEquipo/{id:int}")]
        public async Task<IActionResult> BorrarEquipo(int id)
        {
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
        }
    }
}
