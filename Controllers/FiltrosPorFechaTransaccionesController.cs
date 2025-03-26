using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion transacciones.
    /// </summary>
    [Route("api/FiltroFechas")]
    [ApiController]
    public class FiltrosPorFechaTransaccionesController : ControllerBase
    {
        private readonly PortalWebContext _context;

        /// <summary>
        /// Extraer el context de EF.
        /// </summary>
        public FiltrosPorFechaTransaccionesController(PortalWebContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene las transacciones por filtro de fecha de todos los equipos.
        /// </summary>
        /// <returns>Lista de todas las transacciones de todos los equipos por filtro de fecha.</returns>
        /// <response code="200">Devuelve todas las transacciones de todos los equipos por filtro de fecha.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Transaccional")]
        [HttpPost("Filtrar")]
        public async Task<IEnumerable<object>?> ResultadoFiltroFechasTransacciones([FromBody] ModeloFiltroFechasTransacciones filtroFechas)
        {
            try
            {
                if (filtroFechas.Tipo == 2)
                {
                    var fechaAtras = filtroFechas.FechaInicio.AddDays(-5).Date;
                    var resultado = _context.TransaccionesExcel.AsNoTracking()
                                           .Where(te => te.Machine_Sn == filtroFechas.Machine_Sn
                                                     && te.FechaTransaccion >= fechaAtras
                                                     && te.FechaTransaccion <= filtroFechas.FechaFin)
                                           .OrderByDescending(te => te.FechaTransaccion)
                                           .ToList();
                    return resultado;
                }
                else
                {
                    return await _context.GetProcedures().SP_FiltroPorFechaTransaccionesAsync(filtroFechas.Tipo, filtroFechas.Machine_Sn, filtroFechas.FechaInicio, filtroFechas.FechaFin);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene los datos para generar el reporte de consolidado por localidad para generar el excel.
        /// </summary>
        /// <returns>Lista de datos para hacer consolidado por localidad.</returns>
        /// <response code="200">Devuelve todos los datos para realizar pdf de consolidado.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPost("Consolidado")]
        public IActionResult ResultadoConsolidado([FromBody] ModeloConsolidado modelo)
        {
            try
            {
                List<object> result = new();
                string fechaHoy = DateTime.Now.ToString("yyyyMMdd"),
                hora = DateTime.Now.ToString("HHmm");
                DateTime fechaAtras = modelo.FechaIni.AddDays(-5).Date;

                if (modelo?.Equipos == null || !modelo.Equipos.Any())
                    return BadRequest("Lista de equipos vacía");
                foreach (var equipo in modelo.Equipos)
                {
                    var query =
                    (from te in _context.TransaccionesExcel.AsNoTracking()
                     join ep in _context.Equipos.AsNoTracking() on te.Machine_Sn equals ep.serieEquipo into epJoin
                     from ep in epJoin.DefaultIfEmpty()
                     join td in _context.Tiendas.AsNoTracking() on ep.codigoTiendaidFk equals td.id into tdJoin
                     from td in tdJoin.DefaultIfEmpty()
                     join cli in _context.Clientes.AsNoTracking() on td.CodigoClienteidFk equals cli.CodigoCliente into cliJoin
                     from cli in cliJoin.DefaultIfEmpty()
                     join mt in _context.MasterTable.AsNoTracking() on new { cod = td.CodProv, master = "CCAN" } equals new { cod = mt.codigo, master = mt.master } into mtJoin
                     from mt in mtJoin.DefaultIfEmpty()
                     join da in _context.Datos_Personales.AsNoTracking() on te.Usuarios_idFk equals da.UsuarioidFk into daJoin
                     from da in daJoin.DefaultIfEmpty()
                     join u in _context.Usuarios on new { usuario = te.Usuarios_idFk, ip = ep.IpEquipo } equals new { usuario = u.Usuario, ip = u.IpMachine } into uJoin
                     from u in uJoin.DefaultIfEmpty()
                     join ctb in _context.cuentas_bancarias.AsNoTracking() on u.CuentasidFk equals ctb.id into ctbJoin
                     from ctb in ctbJoin.DefaultIfEmpty()
                     where ep.serieEquipo == equipo && te.FechaTransaccion >= fechaAtras && te.FechaTransaccion <= modelo.FechaFin
                     let TotalOriginal = (te.FechaTransaccion >= modelo.FechaIni && te.FechaTransaccion <= modelo.FechaFin) ?
                                         (te.Deposito_Bill_1 * 1) + (te.Deposito_Bill_2 * 2) + (te.Deposito_Bill_5 * 5) +
                                         (te.Deposito_Bill_10 * 10) + (te.Deposito_Bill_20 * 20) + (te.Deposito_Bill_50 * 50) +
                                         (te.Deposito_Bill_100 * 100) + (te.Manual_Deposito_Coin_1 * 0.01m) +
                                         (te.Manual_Deposito_Coin_5 * 0.05m) + (te.Manual_Deposito_Coin_10 * 0.10m) +
                                         (te.Manual_Deposito_Coin_25 * 0.25m) + (te.Manual_Deposito_Coin_50 * 0.50m) +
                                         (te.Manual_Deposito_Coin_100 * 1m) : 0m
                     let Total5DiasAntes = (te.FechaTransaccion >= fechaAtras && te.FechaTransaccion < modelo.FechaIni) ?
                                           (te.Deposito_Bill_1 * 1) + (te.Deposito_Bill_2 * 2) + (te.Deposito_Bill_5 * 5) +
                                           (te.Deposito_Bill_10 * 10) + (te.Deposito_Bill_20 * 20) + (te.Deposito_Bill_50 * 50) +
                                           (te.Deposito_Bill_100 * 100) + (te.Manual_Deposito_Coin_1 * 0.01m) +
                                           (te.Manual_Deposito_Coin_5 * 0.05m) + (te.Manual_Deposito_Coin_10 * 0.10m) +
                                           (te.Manual_Deposito_Coin_25 * 0.25m) + (te.Manual_Deposito_Coin_50 * 0.50m) +
                                           (te.Manual_Deposito_Coin_100 * 1m) : 0m
                     group new { te, td, cli, mt, da, u, ctb, TotalOriginal, Total5DiasAntes } by new
                     {
                         CodigoCliente = cli.CodigoClienteBanco,
                         cli.NombreCliente,
                         td.NombreTienda,
                         codigoLocalidad = mt.codigoLocalidadBanco.Trim(),
                         Localidad = mt.nombre.Trim(),
                         te.Machine_Sn,
                         Establecimiento = da.Nombres,
                         CodigoEstablecimiento = da.Cedula,
                         ctb.numerocuenta,
                         ctb.nombanco,
                         u.Observacion
                     } into g
                     select new
                     {
                         Fecha = fechaHoy,
                         g.Key.CodigoCliente,
                         g.Key.NombreCliente,
                         g.Key.NombreTienda,
                         g.Key.codigoLocalidad,
                         g.Key.Localidad,
                         g.Key.Machine_Sn,
                         g.Key.Establecimiento,
                         g.Key.CodigoEstablecimiento,
                         g.Key.numerocuenta,
                         g.Key.nombanco,
                         g.Key.Observacion,
                         Total = g.Sum(x => x.TotalOriginal),
                         TotalRezagadas = g.Sum(x => x.Total5DiasAntes)
                     });
                    //var queryResult = await query.AsNoTracking().ToListAsync();
                    result.AddRange(query);
                }
                int numeroCorte = _context.NumeroCortesDias.AsNoTracking().Where(x => x.Fecha == fechaHoy).Select(x => x.NumCorte).FirstOrDefault();
                string nombreArchivoDetalle = $"Fr{fechaHoy}{hora}{numeroCorte}";
                var update = _context.TransaccionesAcreditadas
                             .Where(u => u.NombreArchivo.Equals(modelo.NombreArchivo))
                             .ExecuteUpdate(u => u.SetProperty(u => u.NombreConsolidado, nombreArchivoDetalle));
                return Ok(new { result, nombreArchivoDetalle });
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }
    }
}