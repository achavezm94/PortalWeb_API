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

        [HttpPost]
        [Route("GuardarClienteSignaTienda")]
        public async Task<IActionResult> GuardarCliente([FromBody] ObjClienteLocalidad model)
        {
            foreach (var item in model.Localidades)
            {
                await _context.ClienteSignaLocalidad.AddAsync(new ClienteSignaLocalidad
                {
                    CodigoCiente = model.Cliente,
                    Master = "CCAN",
                    Codigo = item
                });
            }
            return (await _context.SaveChangesAsync() > 0) ? Ok() : BadRequest();            
        }

        [HttpGet("ObtenerLocalidades/{cliente}")]
        public IActionResult ObtenerDatamasterLocalidades([FromRoute] string cliente)
        {
            var Datos = from t in _context.ClienteSignaLocalidad
                        join l in _context.MasterTable on new { t.Codigo, t.Master } equals new { l.Codigo, l.Master }
                        join c in _context.Clientes on cliente equals c.CodigoCliente
                        where t.CodigoCiente == cliente
                        select new {id = t.Id, idCliente = c.Id, t.Master, t.Codigo, l.Nombre};
            return (Datos != null) ? Ok(Datos) : NotFound("No se pudo encontrar");
        }

        [HttpDelete]
        [Route("BorrarLocalidad/{id}")]
        public IActionResult EliminarTransaccionesAcreeditadasAsync([FromRoute] int id)
        {
            var delete = _context.ClienteSignaLocalidad
                            .Where(b => b.Id.Equals(id))
                            .ExecuteDelete();
            return (delete != 0) ? Ok() : BadRequest();
        }
    }
}