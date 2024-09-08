using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using System.Data;

namespace PortalWeb_API.Controllers
{

    [Route("api/TiendaCuenta")]
    [ApiController]
    public class TiendaCuentaController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public TiendaCuentaController(PortalWebContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Nivel2")]
        [HttpGet("ObtenerTiendaCuentas/{id}")]
        public IActionResult ObtenerTiendaCuentas([FromRoute] string id)
        {
            var Datos = from otc in _context.cuentaSignaTienda
                        join ct in _context.cuentas_bancarias on otc.idcuentabancaria equals ct.id
                        where otc.idtienda == id
                        select new { otc.idcuentabancaria, ct.nombanco, ct.numerocuenta, otc.id, ct.TipoCuenta, ct.Observacion };
            return (Datos != null) ? Ok(Datos) : NotFound();
        }

        [Authorize(Policy = "Nivel2")]
        [HttpDelete("BorrarCuentaTienda/{id}")]
        public IActionResult BorrarCuentaTienda([FromRoute] int id)
        {
            var delete = _context.cuentaSignaTienda
                          .Where(b => b.id.Equals(id))
                          .ExecuteDelete();
            return (delete != 0) ? Ok() : BadRequest();
        }
    }
}
