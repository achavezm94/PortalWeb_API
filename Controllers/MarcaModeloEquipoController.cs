using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion de Modelos y Marcas de los equipos.
    /// </summary>
    [Route("api/MarcaModeloEquipo")]
    [ApiController]
    public class MarcaModeloEquipoController : ControllerBase
    {
        private readonly PortalWebContext _context;

        /// <summary>
        /// Extraer el context de EF.
        /// </summary>
        public MarcaModeloEquipoController(PortalWebContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene los datos de las marcas para los equipos.
        /// </summary>
        /// <returns>Lista de datos de las marcas para los equipos.</returns>
        /// <response code="200">Devuelve la lista de datos de las marcas para los equipos.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
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

        /// <summary>
        /// Obtiene los datos de los modelos para los equipos.
        /// </summary>
        /// <returns>Lista de datos de los modelos para los equipos.</returns>
        /// <response code="200">Devuelve la lista de datos de los modelos para los equipos.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
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