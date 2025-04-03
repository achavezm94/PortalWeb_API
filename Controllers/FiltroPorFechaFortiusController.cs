using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion requisito Transacciones Fortius.
    /// </summary>
    [Route("api/FiltroFechasFortius")]
    [ApiController]
    public class FiltroPorFechaFortiusController : ControllerBase
    {
        private readonly PortalWebContext _context;

        /// <summary>
        /// Extraer el context de EF.
        /// </summary>
        public FiltroPorFechaFortiusController(PortalWebContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene las transacciones por filtro de fecha de todos los equipos para Fortius.
        /// </summary>
        /// <returns>Lista de todas las transacciones de todos los equipos por filtro de fecha.</returns>
        /// <response code="200">Devuelve todas las transacciones de todos los equipos por filtro de fecha.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Transaccional")]
        [HttpPost("FiltrarFortius")]
        public async Task<IEnumerable<object>?> ResultadoFiltroFechasTransaccionesFortius([FromBody] FechasIniFin filtroFechas)
        {            
            try
            {
                List<SP_FiltroPorFechaTransaccionesResult> result = await _context.GetProcedures().SP_FiltroPorFechaTransaccionesAsync(4, "NULL", filtroFechas.FechaIni, filtroFechas.FechaFin);
                var resultadoFiltrado = result.Select(l => new 
                                            {
                                               Localidad = l.Acreditada.Trim(),
                                                l.Fecha,
                                                l.Hora,
                                                Cliente = l.NombreCliente,
                                                Tienda = l.NombreTienda,
                                                NTrans = l.Machine_Sn +"-"+l.Transaccion_No,
                                                SerieEquipo = l.Machine_Sn,
                                                Usuario = l.Usuarios_idFk,
                                                Establecimient0 = l.Establecimiento,
                                                Actividad = l.Observacion,
                                                l.CodigoEstablecimiento,
                                                l.nombanco,
                                                l.TipoCuenta,
                                                CuentaBancaria = l.numerocuenta,
                                                l.Deposito_Bill_1,
                                                l.Deposito_Bill_2,
                                                l.Deposito_Bill_5,
                                                l.Deposito_Bill_10,
                                                l.Deposito_Bill_20,
                                                l.Deposito_Bill_50,
                                                l.Deposito_Bill_100,
                                                l.Manual_Deposito_Coin_1,
                                                l.Manual_Deposito_Coin_5,
                                                l.Manual_Deposito_Coin_10,
                                                l.Manual_Deposito_Coin_25,
                                                l.Manual_Deposito_Coin_50,
                                                l.Manual_Deposito_Coin_100,
                                                l.Total,
                                                l.TipoTransaccion
                                            }).ToList();
                return resultadoFiltrado;                
            }
            catch (Exception)
            {
                return (IEnumerable<object>?)Problem("Ocurrió un error interno", statusCode: 500);
            }
        }
    }
}