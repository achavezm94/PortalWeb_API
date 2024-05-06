using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;
using System.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/Equipo")]
    [ApiController]
    public class EquipoController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public EquipoController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ObtenerEquipo/{tp}/{codTienda}")]
        public IActionResult ObtenerEquipo([FromRoute] int tp, [FromRoute] string codTienda )
        {
            string Sentencia = "exec sp_obtener_maquinaria @type, @ctienda";
            DataTable dt = new();
            using (SqlConnection connection = new(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new(Sentencia, connection);
                SqlDataAdapter adapter = new(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@type", tp));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@ctienda", codTienda));
                adapter.Fill(dt);
            }
            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }
            return Ok(dt);
        }

        [HttpPost]
        [Route("GuardarEquipo")]
        public async Task<IActionResult> GuardarEquipo([FromBody] Equipos model)
        {
            if (ModelState.IsValid)
            {
                if (model.CapacidadIni is not null && model.CapacidadIniSobres is not null && model.CapacidadAsegurada is not null && model.CapacidadIni !=0 && model.CapacidadIniSobres != 0 && model.CapacidadAsegurada != 0)
                {
                    var equipo = _context.Equipos.FirstOrDefault(x => x.SerieEquipo == model.SerieEquipo);
                    if (equipo == null)
                    {
                        await _context.Equipos.AddAsync(model);

                        if (await _context.SaveChangesAsync() > 0)
                        {
                            var res = _context.EquiposTemporales.FirstOrDefault(x => x.IpEquipo == model.IpEquipo);
                            if (res != null)
                            {
                                res.Active = "F";
                                await _context.SaveChangesAsync();
                            }
                            return Ok(model);
                        }
                        else
                        {
                            return BadRequest("Datos incorrectos");
                        }
                    }
                    return BadRequest("Ya existe el Equipo");
                }
                return BadRequest("Campos NULL");
            }
            else
            {
                return BadRequest("ERROR");
            }
        }

        [HttpPut]
        [Route("ActualizarEquipo/{id}")]
        public async Task<IActionResult> ActualizarEquipo([FromRoute] int id, [FromBody] Equipos model)
        {            
            if (id != model.Id)
            {
                return BadRequest("No existe el equipo");
            }
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(model);
        }

        [HttpDelete]
        [Route("BorrarEquipo/{id}")]
        public IActionResult BorrarEquipo(int id)
        {
            var update = _context.Equipos
                           .Where(u => u.Id.Equals(id))
                           .ExecuteUpdate(u => u.SetProperty(u => u.Active, "F"));
            return (update != 0) ? Ok() : BadRequest();
        }

        [HttpGet]
        [Route("EquipoNuevo")]
        public IActionResult ObtenerEquipoTemporal()
        {
            string Sentencia = "exec SP_ObtenerEquipoTemp";
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
    }
}