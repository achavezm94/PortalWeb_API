﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Methods;
using PortalWeb_API.Models;
using System.Data;

namespace PortalWeb_API.Controllers
{
    /// <summary>
    /// ENDPOINT para seccion de equipos.
    /// </summary>
    [Route("api/Equipo")]
    [ApiController]
    public class EquipoController : ControllerBase
    {
        private readonly PortalWebContext _context;

        /// <summary>
        /// Extraer el context de EF.
        /// </summary>
        public EquipoController(PortalWebContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene los totales de un equipo para poder ser visualizados en el moneq.
        /// </summary>
        /// <returns>Lista de totales de la ultima transacción de un equipo.</returns>
        /// <response code="200">Devuelve la lista de datos de los totales de la ultima transaccion de un equipo.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Monitor")]
        [HttpGet("ObtenerTotalesMoneq/{Machine_Sn}")]
        public IActionResult ObtenerTotalesMoneq(string Machine_Sn)
        {
            try
            {
                MetodosTotales metodosTotales = new(_context);
                var resultado = metodosTotales.TotalesUltimaTransaccion(Machine_Sn);
                return Ok(resultado);
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Obtiene los datos de todos los equipos de un cliente para mostrar en el moneq.
        /// </summary>
        /// <returns>Lista de datos de todos los equipos de un cliente.</returns>
        /// <response code="200">Devuelve la lista de datos de los equipos pertenecientes a un cliente.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Monitor")]
        [HttpGet("ObtenerEquipoMoneq/{opcion}/{codigoCliente}")]
        public IActionResult ObtenerEquipoMoneq(int opcion, string codigoCliente)
        {
            try
            {
                var query = from ep in _context.Equipos.AsNoTracking()
                            join mt1 in _context.MasterTable.AsNoTracking()
                                on ep.tipo equals mt1.codigo into mt1Join
                            from mt1 in mt1Join.DefaultIfEmpty()
                            where mt1.master == "MQT"
                            join mc in _context.marca.AsNoTracking()
                                on new { ep.tipo, ep.marca } equals new { tipo = mc.codigotipomaq, marca = mc.codmarca } into mcJoin
                            from mc in mcJoin.DefaultIfEmpty()
                            join md in _context.modelo.AsNoTracking()
                                on new { ep.marca, ep.tipo, ep.modelo } equals new { marca = md.codmarca, tipo = md.codigotipomaq, modelo = md.codmodelo } into mdJoin
                            from md in mdJoin.DefaultIfEmpty()
                            join td in _context.Tiendas.AsNoTracking()
                                on ep.codigoTiendaidFk equals td.id into tdJoin
                            from td in tdJoin.DefaultIfEmpty()
                            join mt2 in _context.MasterTable.AsNoTracking()
                                on td.CodProv equals mt2.codigo into mt2Join
                            from mt2 in mt2Join.DefaultIfEmpty()
                            join t in _context.TotalesEquipos.AsNoTracking()
                                on ep.serieEquipo equals t.Equipo into tJoin
                            from t in tJoin.DefaultIfEmpty()
                            where ep.active == "A"
                            select new
                            {
                                idCliente = td.CodigoClienteidFk ?? "",
                                NombreTienda = td.NombreTienda ?? "",
                                tipoMaquinaria = mt1.nombre ?? "",
                                mc.nombremarca,
                                md.nombremodelo,
                                indicadorTotalMaxAsegurado = ep.capacidadAsegurada,
                                serieEquipo = ep.serieEquipo ?? "",
                                indicadorCapacidadBilletesMax = ep.capacidadIni ?? 0,
                                indicadorCapacidadMaxMonedas = ep.capacidadIniSobres ?? 0,
                                ep.estadoPing,
                                ep.tiempoSincronizacion,
                                ultimaRecoleccion = t.FechaUltimaRecoleccion,
                                provincia = mt2.nombre ?? "",
                                ep.IpEquipo,
                                UltimaNoTrans = t.UltimaTransaccion,
                                FechaUltimaTrans = t.FechaUltimaTransaccion,
                                TipoTrans = t.Tipo
                            };
                if (opcion == 1)
                {
                    query = query.OrderBy(x => x.serieEquipo);
                }
                else if (@opcion == 2)
                {
                    query = query.Where(x => x.idCliente == @codigoCliente).OrderBy(x => x.serieEquipo);
                }
                //query = query.OrderBy(x => x.serieEquipo);
                //var result = query.ToList();
                return Ok(query.ToList());
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Obtiene los datos de todos los equipos para modulo equipos.
        /// </summary>
        /// <returns>Lista de datos de todos los equipos.</returns>
        /// <response code="200">Devuelve la lista de datos de los equipos.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel2")]
        [HttpGet("ObtenerEquipo")]
        public IActionResult ObtenerEquipo()
        {
            try
            {
                var query = from ep in _context.Equipos.AsNoTracking()
                            join td in _context.Tiendas.AsNoTracking() on ep.codigoTiendaidFk equals td.id into tiendaGroup
                            from td in tiendaGroup.DefaultIfEmpty()
                            join cli in _context.Clientes.AsNoTracking() on td.CodigoClienteidFk equals cli.CodigoCliente into clienteGroup
                            from cli in clienteGroup.DefaultIfEmpty()
                            join mt1 in _context.MasterTable.AsNoTracking() on ep.tipo equals mt1.codigo into tipoGroup
                            from mt1 in tipoGroup.DefaultIfEmpty()
                            where mt1.master == "MQT"
                            join mc in _context.marca.AsNoTracking() on new { ep.tipo, ep.marca } equals new { tipo = mc.codigotipomaq, marca = mc.codmarca } into marcaGroup
                            from mc in marcaGroup.DefaultIfEmpty()
                            join md in _context.modelo.AsNoTracking() on new { ep.marca, ep.tipo, ep.modelo } equals new { marca = md.codmarca, tipo = md.codigotipomaq, modelo = md.codmodelo } into modeloGroup
                            from md in modeloGroup.DefaultIfEmpty()
                            join u in _context.Usuarios.AsNoTracking() on ep.IpEquipo equals u.IpMachine into usuarioGroup
                            from u in usuarioGroup.DefaultIfEmpty()
                            group new { ep, td, cli, mt1, mc, md, u } by new
                            {
                                td.CodigoClienteidFk,
                                cli.NombreCliente,
                                td.NombreTienda,
                                td.CodigoTienda,
                                mt1.nombre,
                                mc.nombremarca,
                                md.nombremodelo,
                                ep.id,
                                ep.codigoTiendaidFk,
                                ep.capacidadAsegurada,
                                ep.tipo,
                                ep.marca,
                                ep.modelo,
                                ep.serieEquipo,
                                ep.active,
                                ep.capacidadIni,
                                ep.capacidadIniSobres,
                                ep.fechaInstalacion,
                                ep.IpEquipo
                            } into g
                            select new
                            {
                                g.Key.CodigoClienteidFk,
                                g.Key.NombreCliente,
                                NombreTienda = g.Key.NombreTienda ?? "",
                                TipoMaquinaria = g.Key.nombre ?? "",
                                g.Key.nombremarca,
                                g.Key.nombremodelo,
                                g.Key.id,
                                g.Key.codigoTiendaidFk,
                                g.Key.CodigoTienda,
                                g.Key.capacidadAsegurada,
                                g.Key.tipo,
                                Marca = g.Key.marca ?? "",
                                Modelo = g.Key.modelo ?? "",
                                SerieEquipo = g.Key.serieEquipo ?? "",
                                g.Key.active,
                                g.Key.capacidadIni,
                                g.Key.capacidadIniSobres,
                                g.Key.fechaInstalacion,
                                CapacidadUsuariosTemporales = _context.UsuariosTemporales.Count(ut => ut.IpMachineSolicitud == g.Key.IpEquipo && ut.Active == "A"),
                                CapacidadUsuarios = g.Count(x => x.u != null),
                                g.Key.IpEquipo
                            };
                return Ok(query.OrderBy(x => x.SerieEquipo).ToList());
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Obtiene los datos de todos los equipos de un cliente para modulo equipos.
        /// </summary>
        /// <returns>Lista de datos de todos los equipos pertenecientes a un cliente.</returns>
        /// <response code="200">Devuelve la lista de datos de los equipos pertenecientes a un cliente.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Transaccional")]
        [HttpGet("EquipoLista/{codCliente}")]
        public IActionResult EquipoLista([FromRoute] string codCliente)
        {
            try
            {
                var Datos = from eq in _context.Equipos.AsNoTracking()
                            join ti in _context.Tiendas.AsNoTracking() on eq.codigoTiendaidFk equals ti.id into tiGroup
                            from ti in tiGroup.DefaultIfEmpty()
                            join cli in _context.Clientes.AsNoTracking() on ti.CodigoClienteidFk equals cli.CodigoCliente
                            where cli.CodigoCliente == codCliente
                            orderby eq.serieEquipo ascending
                            select new
                            {
                                eq.serieEquipo,
                                ti.NombreTienda
                            };
                return Ok(Datos);
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Obtiene los datos de todos los equipos nuevos para su ingreso a la plataforma.
        /// </summary>
        /// <returns>Lista de datos de todos los equipos nuevos.</returns>
        /// <response code="200">Devuelve la lista de todos los equipos nuevos.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel2")]
        [HttpGet("EquipoNuevo")]
        public IActionResult ObtenerEquipoTemporal()
        {
            try
            {
                var Datos = from eqt in _context.EquiposTemporales.AsNoTracking()
                            where eqt.Active.Equals("A")
                            select new { eqt.serieEquipo, eqt.IpEquipo };
                return (Datos != null) ? Ok(Datos) : NotFound();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Guarda un equipo nuevo.
        /// </summary>        
        /// <response code="200">Se registro el equipo a la plataforma.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPost("GuardarEquipo")]
        public async Task<IActionResult> GuardarEquipo([FromBody] Equipos model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool equipoExists = _context.Equipos.Any(u => u.serieEquipo.Equals(model.serieEquipo));
                    if (equipoExists)
                    {
                        return BadRequest("Equipo ya existe");
                    }
                    else
                    {
                        await _context.Equipos.AddAsync(model);

                        var nuevoEquipo = new TotalesEquipos
                        {
                            Equipo = model.serieEquipo,
                            TotalCuadreEquipo = 0
                        };

                        await _context.TotalesEquipos.AddAsync(nuevoEquipo);

                        if (await _context.SaveChangesAsync() > 0)
                        {
                            var update = _context.EquiposTemporales
                                        .Where(u => u.IpEquipo.Equals(model.IpEquipo))
                                        .ExecuteUpdate(u => u.SetProperty(u => u.Active, "F"));
                            return Ok();
                        }
                    }
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Actualiza informacion de un equipo especifico.
        /// </summary>
        /// <response code="200">Actualizo correctamente el registro.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPut("ActualizarEquipo/{id}")]
        public async Task<IActionResult> ActualizarEquipo([FromRoute] int id, [FromBody] Equipos model)
        {
            try
            {
                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Activa el equipo en caso de encontrarse desactivado.
        /// </summary>
        /// <response code="200">Activo correctamente el registro.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpPut("ActivarEquipo/{id}")]
        public IActionResult ActivarEquipo(int id)
        {
            try
            {
                var update = _context.Equipos
                              .Where(u => u.id.Equals(id))
                              .ExecuteUpdate(u => u.SetProperty(u => u.active, "A"));
                return Ok();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }

        /// <summary>
        /// Desactivar un equipo.
        /// </summary>
        /// <response code="200">Desactivo correctamente el registro.</response>
        /// <response code="401">Es necesario iniciar sesión.</response>
        /// <response code="403">Acceso denegado, permisos insuficientes.</response>
        /// <response code="500">Si ocurre un error en el servidor.</response>
        [Authorize(Policy = "Nivel1")]
        [HttpDelete("BorrarEquipo/{id}")]
        public IActionResult BorrarEquipo(int id)
        {
            try
            {
                var update = _context.Equipos
                               .Where(u => u.id.Equals(id))
                               .ExecuteUpdate(u => u.SetProperty(u => u.active, "F"));
                return (update != 0) ? Ok() : BadRequest();
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error interno", statusCode: 500);
            }
        }
    }
}