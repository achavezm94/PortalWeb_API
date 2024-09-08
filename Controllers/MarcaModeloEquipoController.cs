using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/MarcaModeloEquipo")]
    [ApiController]
    public class MarcaModeloEquipoController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public MarcaModeloEquipoController(PortalWebContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Nivel1")]
        [HttpGet("ObtenerMarca/{codigotipomaq}")]
        public IActionResult ObtenerMarca(string codigotipomaq)
        {
            if (ModelState.IsValid)
            {
                var marcas = from m in _context.marca where m.codigotipomaq.Equals(codigotipomaq) select m;
                return (marcas != null) ? Ok(marcas) : NotFound();
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize(Policy = "Nivel1")]
        [HttpGet("ObtenerModelo/{codigotipomaq}/{codmodelo}")]
        public IActionResult ObtenerMarca(string codigotipomaq, string codmodelo)
        {
            if (ModelState.IsValid)
            {
                var modelos = from mo in _context.modelo where mo.codigotipomaq.Equals(codigotipomaq) && mo.codmarca.Equals(codmodelo) select mo;
                return (modelos != null) ? Ok(modelos) : NotFound();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}