using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcaModeloEquipoController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public MarcaModeloEquipoController(PortalWebContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        [Route("ObtenerMarca/{codigotipomaq}")]
        public IActionResult ObtenerMarca(string codigotipomaq)
        {
            if (ModelState.IsValid)
            {
                var marcas = from c in _context.Marca
                            where c.Codigotipomaq == codigotipomaq
                            select c;
                return (marcas != null) ? Ok(marcas) : NotFound("No se encontro");
            }
            else
            {
                return BadRequest("ERROR");
            }
        }

        [HttpGet]
        [Route("ObtenerModelo/{codigotipomaq}/{codmodelo}")]
        public IActionResult ObtenerMarca(string codigotipomaq, string codmodelo)
        {
            if (ModelState.IsValid)
            {
                var modelos = from c in _context.Modelo
                              where c.Codigotipomaq == codigotipomaq && c.Codmarca == codmodelo
                              select c;
                return (modelos != null) ? Ok(modelos) : NotFound("No se encontro");
            }
            else
            {
                return BadRequest("ERROR");
            }
        }
    }
}
