using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using System.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/DataMaster")]
    [ApiController]
    public class MasterTableController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public MasterTableController(PortalWebContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Nivel1")]
        [HttpGet("GetDataMaster/{mast}")]
        public IActionResult GetDataMaster([FromRoute] string mast)
        {
            var Datos = from m in _context.MasterTable
                        where m.master.Equals(mast)
                        group m by new { m.master, m.codigo, m.nombre } into g
                        select new
                        {
                            master = g.Key.master.Trim(),
                            codigo = g.Key.codigo.Trim(),
                            nombre = g.Key.nombre.Trim()
                        };
            return (Datos != null) ? Ok(Datos) : NotFound();
        }

        [Authorize(Policy = "Nivel3")]
        [HttpGet("GetTipoEquipo")]
        public IActionResult GetTipoEquipo()
        {
            var Datos = from m in _context.MasterTable
                        where m.master.Equals("MQT") && (m.codigo == "009" || m.codigo == "012")
                        group m by new { m.master, m.codigo, m.nombre } into g
                        select new
                        {
                            master = g.Key.master.Trim(),
                            codigo = g.Key.codigo.Trim(),
                            nombre = g.Key.nombre.Trim()
                        };
            return (Datos != null) ? Ok(Datos) : NotFound();
        }

        [Authorize(Policy = "Nivel1")]
        [HttpGet("ObtenerDatamasterLocalidades/{codCliente}")]
        public  IActionResult ObtenerDatamasterLocalidades([FromRoute] string codCliente)
        {
            var Datos = from t in _context.MasterTable
                        where t.master.Equals("CCAN") && !(
                                                            from l in _context.ClienteSignaLocalidad
                                                            where l.codigoCiente == codCliente
                                                            select l.codigo
                                                           ).Contains(t.codigo)
                        select new {Master =  t.master.Trim(), Codigo = t.codigo.Trim(), Nombre =  t.nombre.Trim() };
            return (Datos != null) ? Ok(Datos) : NotFound();
        }
    }
}