using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    [Route("api/ClienteSignaLocalidad")]
    [ApiController]
    public class ClienteSignaLocalidadController : ControllerBase
    {
        private readonly PortalWebContext _context;
        public ClienteSignaLocalidadController(PortalWebContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Nivel2")]
        [HttpGet("ObtenerLocalidades/{cliente}")]
        public IActionResult ObtenerDatamasterLocalidades(string cliente)
        {
            var Datos = from t in _context.ClienteSignaLocalidad
                        join l in _context.MasterTable on new { t.codigo, t.master } equals new { l.codigo, l.master }
                        join c in _context.Clientes on cliente equals c.CodigoCliente
                        where t.codigoCiente == cliente
                        select new { id = t.id, idCliente = c.id, t.master, t.codigo, l.nombre };
            return (Datos != null) ? Ok(Datos) : NotFound();
        }

        [Authorize(Policy = "Nivel1")]
        [HttpPost("GuardarClienteSignaTienda")]
        public async Task<IActionResult> GuardarCliente([FromBody] ObjClienteLocalidad model)
        {
            foreach (var item in model.Localidades)
            {
                await _context.ClienteSignaLocalidad.AddAsync(new ClienteSignaLocalidad
                {
                    codigoCiente = model.Cliente,
                    master = "CCAN",
                    codigo = item
                });
            }
            return (await _context.SaveChangesAsync() > 0) ? Ok() : BadRequest();            
        }        

        [Authorize(Policy = "Nivel1")]
        [HttpDelete("BorrarLocalidad/{id}")]
        public IActionResult EliminarTransaccionesAcreeditadasAsync(int id)
        {
            var delete = _context.ClienteSignaLocalidad
                            .Where(b => b.id.Equals(id))
                            .ExecuteDelete();
            return (delete != 0) ? Ok() : BadRequest();
        }
    }
}