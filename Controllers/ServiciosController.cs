using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using PortalWeb_API.Methods;
using PortalWeb_API.Models;
using PortalWeb_APIs;
using System.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/Servicios")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private readonly PortalWebContext _context;
        private readonly IHubContext<PingHubEquipos> _pinghub;
        private readonly IHubContext<ManualesHub> _manualesHub;
        private readonly IHubContext<UsuarioHub> _usuarioHub;
        public ServiciosController(PortalWebContext context, IHubContext<PingHubEquipos> pingHub, IHubContext<ManualesHub> manualesHub, IHubContext<UsuarioHub> usuarioHub)
        {
            _context = context;
            _pinghub = pingHub;
            _manualesHub = manualesHub;
            _usuarioHub = usuarioHub;
        }

        [Authorize(Policy = "Monitor")]
        [HttpGet("HoraActual")]
        public IActionResult HoraActual()
        {               
            HoraActual horaActual = new();            
            return Ok(horaActual.HoraActualProceso().ToString("yyyy-MM-ddTHH:mm:ss.ffffffzzz"));                    
        }

        [HttpPut("ActualizarEquipoIp")]
        public async Task<IActionResult> ActualizarEquipo([FromBody] string ip)
        {
            MonitoreoModel monitoreoModel = new();
            HoraActual horaActual = new();            
            DateTime cstTime = horaActual.HoraActualProceso();
            var res = await _context.Equipos.FirstOrDefaultAsync(x => x.IpEquipo == ip);
            if (res is not null)
            {
                res.estadoPing = 1;
                res.tiempoSincronizacion = cstTime;
                res.active = res.active;
                _context.Entry(res).State = EntityState.Modified;
                if (await _context.SaveChangesAsync() > 0)
                {
                    monitoreoModel.Ip = res.IpEquipo;
                    monitoreoModel.TiempoSincronizacion = res.tiempoSincronizacion;
                    monitoreoModel.EstadoPing = 1;
                    await _pinghub.Clients.All.SendAsync("SendPingEquipo", monitoreoModel);
                    return Ok();
                }
            }
            return NoContent();
        }

        [HttpPost("UsuarioTempIngresar")]
        public async Task<IActionResult> IngresarUsuario([FromBody] UsuariosTemporales model)
        {
            if (ModelState.IsValid)
            {
                bool usuarioExists = _context.Usuarios.Any(u => u.Usuario == model.Usuario && model.IpMachineSolicitud ==u.IpMachine);
                if (!usuarioExists)
                {
                    bool usuarioTemporalExists = _context.UsuariosTemporales.Any(ut => ut.Usuario == model.Usuario && model.IpMachineSolicitud == ut.IpMachineSolicitud);
                    if (!usuarioTemporalExists)
                    {
                        _context.UsuariosTemporales.Add(new UsuariosTemporales
                        {
                            Usuario = model.Usuario,
                            UserName = model.UserName,
                            IpMachineSolicitud = model.IpMachineSolicitud
                        });
                        if (await _context.SaveChangesAsync() > 0) 
                        {
                            await _usuarioHub.Clients.All.SendAsync("SendUsuarioTemporal", model);
                            return Ok("Ok");
                        }                                               
                    }
                    else
                    {
                        var usuarioTemporal = _context.UsuariosTemporales
                            .FirstOrDefault(ut => ut.Usuario == model.Usuario && ut.IpMachineSolicitud == model.IpMachineSolicitud);
                        if (usuarioTemporal != null && usuarioTemporal.Active == "F")
                        {
                            usuarioTemporal.Active = "A";
                            //_context.SaveChanges();
                            if (await _context.SaveChangesAsync() > 0)
                            {
                                await _usuarioHub.Clients.All.SendAsync("SendUsuarioTemporal", model);
                                return Ok("Ok");
                            }
                        }
                    }
                }
                else
                {
                    return BadRequest("Usuario ya Existe");
                }
            }
            return BadRequest("No se registro");
        }

        [HttpPost("EquipoTempIngresar")]
        public async Task<IActionResult> IngresarEquipo([FromBody] EquiposTemporalesResponse model)
        {
            bool equipoExists = _context.Equipos.Any(u => u.serieEquipo == model.SerieEquipo);
            if (!equipoExists)
            {
                bool IPMachineSolicitudExists = _context.EquiposTemporales.Any(ut => ut.IpEquipo == model.IPMachineSolicitud);
                if (!IPMachineSolicitudExists)
                {
                    _context.EquiposTemporales.Add(new EquiposTemporales
                    {
                        serieEquipo = model.SerieEquiponew,
                        IpEquipo = model.IPMachineSolicitud
                    });
                    return (await _context.SaveChangesAsync() > 0) ? Ok("Ok") : BadRequest();
                }
                else
                {
                    var equipoTemporal = _context.EquiposTemporales
                        .FirstOrDefault(ut => ut.IpEquipo == model.IPMachineSolicitud);
                    if (equipoTemporal != null)
                    {
                        equipoTemporal.Active = "A";
                        equipoTemporal.serieEquipo = model.SerieEquiponew;
                        return (await _context.SaveChangesAsync() > 0) ? Ok("Ok") : BadRequest();
                    }
                }
            }
            else
            {
                return BadRequest("Equipo ya Existe");
            }
            return BadRequest("No se registro");            
        }

        [HttpDelete("UsuarioTempEliminar/{usuario}/{ip}")]
        public async Task<IActionResult> EliminarUsuario([FromRoute] string usuario, [FromRoute] string ip)
        {
            var usuarioExistente = _context.Usuarios.FirstOrDefault(ut => ut.Usuario == usuario);
            var usuarioTemporal = _context.UsuariosTemporales.FirstOrDefault(ut => ut.Usuario == usuario);
            if (usuarioExistente != null)
            {
                var deleteUsuario = _context.Usuarios
                          .Where(b => b.Usuario.Equals(usuario))
                          .ExecuteDelete();

                var deleteDatosPersonales = _context.Datos_Personales
                          .Where(b => b.UsuarioidFk.Equals(usuario))
                          .ExecuteDelete();
                return (await _context.SaveChangesAsync() > 0) ? Ok("Ok") : BadRequest();
            }
            if (usuarioTemporal != null)
            {
                var deleteUsuario = _context.UsuariosTemporales
                          .Where(b => b.Usuario.Equals(usuario))
                          .ExecuteDelete();
                return (await _context.SaveChangesAsync() > 0) ? Ok("Ok") : BadRequest();
            }
            return BadRequest("No se registro");
        }

        [HttpPost("DepositoIngresar")]
        public async Task<IActionResult> IngresarDepositoAsync([FromBody] ODeposito model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                bool verificacion = VerificacionTransaccion(model.User_id, model.Machine_Sn);
                if (verificacion)
                {
                    bool transactionExists = _context.Depositos
                                            .Where(d => d.FechaTransaccion >= model.Time_generated.AddDays(-5) && d.Machine_Sn == model.Machine_Sn)
                                            .Any(d => d.Transaccion_No == model.Transaction_no &&
                                                        d.Usuarios_idFk == model.User_id &&
                                                        d.Machine_Sn == model.Machine_Sn);
                    if (!transactionExists)
                    {
                        await _context.Depositos.AddAsync(new Depositos
                        {
                            Usuarios_idFk = model.User_id,
                            Machine_Sn = model.Machine_Sn,
                            Transaccion_No = model.Transaction_no,
                            FechaTransaccion = model.Time_generated,
                            DivisaTransaccion = "USD",
                            Deposito_Bill_1 = model.Deposit_denom_1,
                            Deposito_Bill_2 = model.Deposit_denom_2,
                            Deposito_Bill_5 = model.Deposit_denom_5,
                            Deposito_Bill_10 = model.Deposit_denom_10,
                            Deposito_Bill_20 = model.Deposit_denom_20,
                            Deposito_Bill_50 = model.Deposit_denom_50,
                            Deposito_Bill_100 = model.Deposit_denom_100,
                            Total_Deposito_Bill_1 = model.Total_deposit_denom_1,
                            Total_Deposito_Bill_2 = model.Total_deposit_denom_2,
                            Total_Deposito_Bill_5 = model.Total_deposit_denom_5,
                            Total_Deposito_Bill_10 = model.Total_deposit_denom_10,
                            Total_Deposito_Bill_20 = model.Total_deposit_denom_20,
                            Total_Deposito_Bill_50 = model.Total_deposit_denom_50,
                            Total_Deposito_Bill_100 = model.Total_deposit_denom_100,
                            Total_Manual_Deposito_Bill_1 = model.Total_manual_deposit_denom_1,
                            Total_Manual_Deposito_Bill_2 = model.Total_manual_deposit_denom_2,
                            Total_Manual_Deposito_Bill_5 = model.Total_manual_deposit_denom_5,
                            Total_Manual_Deposito_Bill_10 = model.Total_manual_deposit_denom_10,
                            Total_Manual_Deposito_Bill_20 = model.Total_manual_deposit_denom_20,
                            Total_Manual_Deposito_Bill_50 = model.Total_manual_deposit_denom_50,
                            Total_Manual_Deposito_Bill_100 = model.Total_manual_deposit_denom_100,
                            Total_Manual_Deposito_Coin_1 = model.Total_manual_deposit_coin_1,
                            Total_Manual_Deposito_Coin_5 = model.Total_manual_deposit_coin_5,
                            Total_Manual_Deposito_Coin_10 = model.Total_manual_deposit_coin_10,
                            Total_Manual_Deposito_Coin_25 = model.Total_manual_deposit_coin_25,
                            Total_Manual_Deposito_Coin_50 = model.Total_manual_deposit_coin_50,
                            Total_Manual_Deposito_Coin_100 = model.Total_manual_deposit_coin_100,
                            Active = "A"
                        });
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            var equipoExistente = await _context.TotalesEquipos.FirstOrDefaultAsync(t => t.Equipo == model.Machine_Sn);

                            if (equipoExistente != null)
                            {
                                if (model.Time_generated < equipoExistente.FechaUltimaRecoleccion)
                                {
                                    equipoExistente.TotalCuadreEquipo -= ((model.Deposit_denom_1 * 1) +
                                                                          (model.Deposit_denom_2 * 2) +
                                                                          (model.Deposit_denom_5 * 5) +
                                                                          (model.Deposit_denom_10 * 10) +
                                                                          (model.Deposit_denom_20 * 20) +
                                                                          (model.Deposit_denom_50 * 50) +
                                                                          (model.Deposit_denom_100 * 100));
                                }
                                else if (equipoExistente.FechaUltimaTransaccion > model.Time_generated)
                                {
                                    equipoExistente.EquipoDepositoBill1 += model.Deposit_denom_1;
                                    equipoExistente.EquipoDepositoBill2 += model.Deposit_denom_2;
                                    equipoExistente.EquipoDepositoBill5 += model.Deposit_denom_5;
                                    equipoExistente.EquipoDepositoBill10 += model.Deposit_denom_10;
                                    equipoExistente.EquipoDepositoBill20 += model.Deposit_denom_20;
                                    equipoExistente.EquipoDepositoBill50 += model.Deposit_denom_50;
                                    equipoExistente.EquipoDepositoBill100 += model.Deposit_denom_100;
                                }
                                else
                                {
                                    equipoExistente.UltimaTransaccion = model.Transaction_no;
                                    equipoExistente.FechaUltimaTransaccion = model.Time_generated;
                                    equipoExistente.Tipo = "A";
                                    equipoExistente.EquipoDepositoBill1 += model.Deposit_denom_1;
                                    equipoExistente.EquipoDepositoBill2 += model.Deposit_denom_2;
                                    equipoExistente.EquipoDepositoBill5 += model.Deposit_denom_5;
                                    equipoExistente.EquipoDepositoBill10 += model.Deposit_denom_10;
                                    equipoExistente.EquipoDepositoBill20 += model.Deposit_denom_20;
                                    equipoExistente.EquipoDepositoBill50 += model.Deposit_denom_50;
                                    equipoExistente.EquipoDepositoBill100 += model.Deposit_denom_100;
                                    equipoExistente.TotalEquipoDepositoBill1 = model.Total_deposit_denom_1;
                                    equipoExistente.TotalEquipoDepositoBill2 = model.Total_deposit_denom_2;
                                    equipoExistente.TotalEquipoDepositoBill5 = model.Total_deposit_denom_5;
                                    equipoExistente.TotalEquipoDepositoBill10 = model.Total_deposit_denom_10;
                                    equipoExistente.TotalEquipoDepositoBill20 = model.Total_deposit_denom_20;
                                    equipoExistente.TotalEquipoDepositoBill50 = model.Total_deposit_denom_50;
                                    equipoExistente.TotalEquipoDepositoBill100 = model.Total_deposit_denom_100;
                                    equipoExistente.TotalEquipoManualBill1 = model.Total_manual_deposit_denom_1;
                                    equipoExistente.TotalEquipoManualBill2 = model.Total_manual_deposit_denom_2;
                                    equipoExistente.TotalEquipoManualBill5 = model.Total_manual_deposit_denom_5;
                                    equipoExistente.TotalEquipoManualBill10 = model.Total_manual_deposit_denom_10;
                                    equipoExistente.TotalEquipoManualBill20 = model.Total_manual_deposit_denom_20;
                                    equipoExistente.TotalEquipoManualBill50 = model.Total_manual_deposit_denom_50;
                                    equipoExistente.TotalEquipoManualBill100 = model.Total_manual_deposit_denom_100;
                                    equipoExistente.TotalEquipoManualCoin1 = model.Total_manual_deposit_coin_1;
                                    equipoExistente.TotalEquipoManualCoin5 = model.Total_manual_deposit_coin_5;
                                    equipoExistente.TotalEquipoManualCoin10 = model.Total_manual_deposit_coin_10;
                                    equipoExistente.TotalEquipoManualCoin25 = model.Total_manual_deposit_coin_25;
                                    equipoExistente.TotalEquipoManualCoin50 = model.Total_manual_deposit_coin_50;
                                    equipoExistente.TotalEquipoManualCoin100 = model.Total_manual_deposit_coin_100;
                                }
                            }
                            else
                            {
                                await _context.TotalesEquipos.AddAsync(new TotalesEquipos
                                {
                                    Equipo = model.Machine_Sn,
                                    UltimaTransaccion = model.Transaction_no,
                                    FechaUltimaTransaccion = model.Time_generated,
                                    Tipo = "A",
                                    EquipoDepositoBill1 = model.Deposit_denom_1,
                                    EquipoDepositoBill2 = model.Deposit_denom_2,
                                    EquipoDepositoBill5 = model.Deposit_denom_5,
                                    EquipoDepositoBill10 = model.Deposit_denom_10,
                                    EquipoDepositoBill20 = model.Deposit_denom_20,
                                    EquipoDepositoBill50 = model.Deposit_denom_50,
                                    EquipoDepositoBill100 = model.Deposit_denom_100,
                                    TotalEquipoDepositoBill1 = model.Total_deposit_denom_1,
                                    TotalEquipoDepositoBill2 = model.Total_deposit_denom_2,
                                    TotalEquipoDepositoBill5 = model.Total_deposit_denom_5,
                                    TotalEquipoDepositoBill10 = model.Total_deposit_denom_10,
                                    TotalEquipoDepositoBill20 = model.Total_deposit_denom_20,
                                    TotalEquipoDepositoBill50 = model.Total_deposit_denom_50,
                                    TotalEquipoDepositoBill100 = model.Total_deposit_denom_100,
                                    TotalEquipoManualBill1 = model.Total_manual_deposit_denom_1,
                                    TotalEquipoManualBill2 = model.Total_manual_deposit_denom_2,
                                    TotalEquipoManualBill5 = model.Total_manual_deposit_denom_5,
                                    TotalEquipoManualBill10 = model.Total_manual_deposit_denom_10,
                                    TotalEquipoManualBill20 = model.Total_manual_deposit_denom_20,
                                    TotalEquipoManualBill50 = model.Total_manual_deposit_denom_50,
                                    TotalEquipoManualBill100 = model.Total_manual_deposit_denom_100,
                                    TotalEquipoManualCoin1 = model.Total_manual_deposit_coin_1,
                                    TotalEquipoManualCoin5 = model.Total_manual_deposit_coin_5,
                                    TotalEquipoManualCoin10 = model.Total_manual_deposit_coin_10,
                                    TotalEquipoManualCoin25 = model.Total_manual_deposit_coin_25,
                                    TotalEquipoManualCoin50 = model.Total_manual_deposit_coin_50,
                                    TotalEquipoManualCoin100 = model.Total_manual_deposit_coin_100,
                                    TotalCuadreEquipo = 0
                                });
                            }
                            if (await _context.SaveChangesAsync() > 0)
                            {
                                var resultado = from cli in _context.Clientes
                                                join eq in _context.Equipos
                                                on model.Machine_Sn equals eq.serieEquipo into eqJoin
                                                from eq in eqJoin.DefaultIfEmpty()
                                                join ti in _context.Tiendas
                                                on eq.codigoTiendaidFk equals ti.id into tiJoin
                                                from ti in tiJoin.DefaultIfEmpty()
                                                where cli.CodigoCliente == ti.CodigoClienteidFk
                                                select new
                                                {
                                                    cli.NombreCliente,
                                                    ti.NombreTienda
                                                };

                                var resultado1 = from u in _context.Usuarios
                                                 join dp in _context.Datos_Personales
                                                 on u.Usuario equals dp.UsuarioidFk into dpJoin
                                                 from dp in dpJoin.DefaultIfEmpty()
                                                 join cb in _context.cuentas_bancarias
                                                 on u.CuentasidFk equals cb.id into cbJoin
                                                 from cb in cbJoin.DefaultIfEmpty()
                                                 where u.Usuario == model.User_id
                                                 select new
                                                 {
                                                     dp.Nombres,
                                                     dp.Cedula,
                                                     cb.nombanco,
                                                     cb.TipoCuenta,
                                                     cb.numerocuenta,
                                                     u.Observacion
                                                 };

                                await _context.TransaccionesExcel.AddAsync(new TransaccionesExcel
                                {
                                    FechaTransaccion = model.Time_generated,
                                    Fecha = model.Time_generated.Date,
                                    Hora = model.Time_generated.ToString("HH:mm:ss"),
                                    NombreCliente = resultado.First().NombreCliente,
                                    NombreTienda = resultado.First().NombreTienda,
                                    Transaccion_No = model.Transaction_no,
                                    Machine_Sn = model.Machine_Sn,
                                    Usuarios_idFk = model.User_id,
                                    Establecimiento = resultado1.First().Nombres,
                                    CodigoEstablecimiento = resultado1.First().Cedula,
                                    nombanco = resultado1.First().nombanco,
                                    TipoCuenta = resultado1.First().TipoCuenta,
                                    numerocuenta = resultado1.First().numerocuenta,
                                    Observacion = resultado1.First().Observacion,
                                    Deposito_Bill_1 = model.Deposit_denom_1,
                                    Deposito_Bill_2 = model.Deposit_denom_2,
                                    Deposito_Bill_5 = model.Deposit_denom_5,
                                    Deposito_Bill_10 = model.Deposit_denom_10,
                                    Deposito_Bill_20 = model.Deposit_denom_20,
                                    Deposito_Bill_50 = model.Deposit_denom_50,
                                    Deposito_Bill_100 = model.Deposit_denom_100,
                                    Manual_Deposito_Coin_1 = 0,
                                    Manual_Deposito_Coin_5 = 0,
                                    Manual_Deposito_Coin_10 = 0,
                                    Manual_Deposito_Coin_25 = 0,
                                    Manual_Deposito_Coin_50 = 0,
                                    Manual_Deposito_Coin_100 = 0,
                                    Total = ((model.Deposit_denom_1 * 1) +
                                             (model.Deposit_denom_2 * 2) +
                                             (model.Deposit_denom_5 * 5) +
                                             (model.Deposit_denom_10 * 10) +
                                             (model.Deposit_denom_20 * 20) +
                                             (model.Deposit_denom_50 * 50) +
                                             (model.Deposit_denom_100 * 100)),
                                    Repetido = "1",
                                    TipoTransaccion = "Deposito",
                                    Acreditada = "N"
                                });
                                if (await _context.SaveChangesAsync() > 0)
                                {
                                    await transaction.CommitAsync();
                                    decimal Monto = (model.Deposit_denom_1 * 1) +
                                                (model.Deposit_denom_2 * 2) +
                                                (model.Deposit_denom_5 * 5) +
                                                (model.Deposit_denom_10 * 10) +
                                                (model.Deposit_denom_20 * 20) +
                                                (model.Deposit_denom_50 * 50) +
                                                (model.Deposit_denom_100 * 100);
                                    int Cant = model.Deposit_denom_1 +
                                                model.Deposit_denom_2 +
                                                model.Deposit_denom_5 +
                                                model.Deposit_denom_10 +
                                                model.Deposit_denom_20 +
                                                model.Deposit_denom_50 +
                                                model.Deposit_denom_100;
                                    await _manualesHub.Clients.All.SendAsync("SendTransaccionManual", new { model.Machine_Sn, Cant, Monto, fechaTransaccion = model.Time_generated, model.Transaction_no, tipo = "A" });
                                    return Ok(1);
                                }                            
                            }
                        }                      
                    }                    
                    else
                    {
                        return Ok(2);
                    }
                }                            
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return Ok(0);
            }
            await transaction.RollbackAsync();
            return Ok(0);
        }

        [HttpPost("ManualIngresar")]
        public async Task<IActionResult> IngresarManualAsync([FromBody] OManual model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                bool verificacion = VerificacionTransaccion(model.User_id, model.Machine_Sn);
                if (verificacion)
                {
                    bool transactionExists = _context.ManualDepositos
                                            .Where(d => d.FechaTransaccion >= model.Time_generated.AddDays(-5) && d.Machine_Sn == model.Machine_Sn)
                                            .Any(d => d.Transaccion_No == model.Transaction_no &&
                                                        d.Usuarios_idFk == model.User_id &&
                                                        d.Machine_Sn == model.Machine_Sn);
                    if (!transactionExists)
                    {
                        await _context.ManualDepositos.AddAsync(new ManualDepositos
                        {
                            Usuarios_idFk = model.User_id,
                            Machine_Sn = model.Machine_Sn,
                            Transaccion_No = model.Transaction_no,
                            FechaTransaccion = model.Time_generated,
                            DivisaTransaccion = "USD",
                            Manual_Deposito_Bill_1 = model.Deposit_denom_1,
                            Manual_Deposito_Bill_2 = model.Deposit_denom_2,
                            Manual_Deposito_Bill_5 = model.Deposit_denom_5,
                            Manual_Deposito_Bill_10 = model.Deposit_denom_10,
                            Manual_Deposito_Bill_20 = model.Deposit_denom_20,
                            Manual_Deposito_Bill_50 = model.Deposit_denom_50,
                            Manual_Deposito_Bill_100 = model.Deposit_denom_100,
                            Manual_Deposito_Coin_1 = model.Deposit_coin_1,
                            Manual_Deposito_Coin_5 = model.Deposit_coin_5,
                            Manual_Deposito_Coin_10 = model.Deposit_coin_10,
                            Manual_Deposito_Coin_25 = model.Deposit_coin_25,
                            Manual_Deposito_Coin_50 = model.Deposit_coin_50,
                            Manual_Deposito_Coin_100 = model.Deposit_coin_100,
                            Total_Deposito_Bill_1 = model.Total_deposit_denom_1,
                            Total_Deposito_Bill_2 = model.Total_deposit_denom_2,
                            Total_Deposito_Bill_5 = model.Total_deposit_denom_5,
                            Total_Deposito_Bill_10 = model.Total_deposit_denom_10,
                            Total_Deposito_Bill_20 = model.Total_deposit_denom_20,
                            Total_Deposito_Bill_50 = model.Total_deposit_denom_50,
                            Total_Deposito_Bill_100 = model.Total_deposit_denom_100,
                            Total_Manual_Deposito_Bill_1 = model.Total_manual_deposit_denom_1,
                            Total_Manual_Deposito_Bill_2 = model.Total_manual_deposit_denom_2,
                            Total_Manual_Deposito_Bill_5 = model.Total_manual_deposit_denom_5,
                            Total_Manual_Deposito_Bill_10 = model.Total_manual_deposit_denom_10,
                            Total_Manual_Deposito_Bill_20 = model.Total_manual_deposit_denom_20,
                            Total_Manual_Deposito_Bill_50 = model.Total_manual_deposit_denom_50,
                            Total_Manual_Deposito_Bill_100 = model.Total_manual_deposit_denom_100,
                            Total_Manual_Deposito_Coin_1 = model.Total_manual_deposit_coin_1,
                            Total_Manual_Deposito_Coin_5 = model.Total_manual_deposit_coin_5,
                            Total_Manual_Deposito_Coin_10 = model.Total_manual_deposit_coin_10,
                            Total_Manual_Deposito_Coin_25 = model.Total_manual_deposit_coin_25,
                            Total_Manual_Deposito_Coin_50 = model.Total_manual_deposit_coin_50,
                            Total_Manual_Deposito_Coin_100 = model.Total_manual_deposit_coin_100,
                            Active = "A"
                        });
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            var equipoExistente = await _context.TotalesEquipos.FirstOrDefaultAsync(t => t.Equipo == model.Machine_Sn);

                            if (equipoExistente != null)
                            {
                                if (model.Time_generated < equipoExistente.FechaUltimaRecoleccion)
                                {
                                    equipoExistente.TotalCuadreEquipo -= ((model.Deposit_denom_1    * 1    ) +
                                                                          (model.Deposit_denom_2    * 2    ) +
                                                                          (model.Deposit_denom_5    * 5    ) +
                                                                          (model.Deposit_denom_10   * 10   ) +
                                                                          (model.Deposit_denom_20   * 20   ) +
                                                                          (model.Deposit_denom_50   * 50   ) +
                                                                          (model.Deposit_denom_100  * 100  ) +
                                                                          (model.Deposit_coin_1     * 0.01m) +
                                                                          (model.Deposit_coin_5     * 0.05m) +
                                                                          (model.Deposit_coin_10    * 0.10m) +
                                                                          (model.Deposit_coin_25    * 0.25m) +
                                                                          (model.Deposit_coin_50    * 0.50m) +
                                                                          (model.Deposit_coin_100   * 1m   ));
                                }
                                else if (equipoExistente.FechaUltimaTransaccion > model.Time_generated)
                                {
                                    equipoExistente.EquipoManualCoin1   += model.Deposit_coin_1;
                                    equipoExistente.EquipoManualCoin5   += model.Deposit_coin_5;
                                    equipoExistente.EquipoManualCoin10  += model.Deposit_coin_10;
                                    equipoExistente.EquipoManualCoin25  += model.Deposit_coin_25;
                                    equipoExistente.EquipoManualCoin50  += model.Deposit_coin_50;
                                    equipoExistente.EquipoManualCoin100 += model.Deposit_coin_100;
                                    equipoExistente.EquipoManualBill1   += model.Deposit_denom_1;
                                    equipoExistente.EquipoManualBill2   += model.Deposit_denom_2;
                                    equipoExistente.EquipoManualBill5   += model.Deposit_denom_5;
                                    equipoExistente.EquipoManualBill10  += model.Deposit_denom_10;
                                    equipoExistente.EquipoManualBill20  += model.Deposit_denom_20;
                                    equipoExistente.EquipoManualBill50  += model.Deposit_denom_50;
                                    equipoExistente.EquipoManualBill100 += model.Deposit_denom_100;
                                }
                                else
                                {
                                    equipoExistente.UltimaTransaccion      = model.Transaction_no;
                                    equipoExistente.FechaUltimaTransaccion = model.Time_generated;
                                    equipoExistente.Tipo = "M";
                                    equipoExistente.EquipoManualCoin1   += model.Deposit_coin_1;
                                    equipoExistente.EquipoManualCoin5   += model.Deposit_coin_5;
                                    equipoExistente.EquipoManualCoin10  += model.Deposit_coin_10;
                                    equipoExistente.EquipoManualCoin25  += model.Deposit_coin_25;
                                    equipoExistente.EquipoManualCoin50  += model.Deposit_coin_50;
                                    equipoExistente.EquipoManualCoin100 += model.Deposit_coin_100;
                                    equipoExistente.EquipoManualBill1   += model.Deposit_denom_1;
                                    equipoExistente.EquipoManualBill2   += model.Deposit_denom_2;
                                    equipoExistente.EquipoManualBill5   += model.Deposit_denom_5;
                                    equipoExistente.EquipoManualBill10  += model.Deposit_denom_10;
                                    equipoExistente.EquipoManualBill20  += model.Deposit_denom_20;
                                    equipoExistente.EquipoManualBill50  += model.Deposit_denom_50;
                                    equipoExistente.EquipoManualBill100 += model.Deposit_denom_100;
                                    equipoExistente.TotalEquipoDepositoBill1   = model.Total_deposit_denom_1;
                                    equipoExistente.TotalEquipoDepositoBill2   = model.Total_deposit_denom_2;
                                    equipoExistente.TotalEquipoDepositoBill5   = model.Total_deposit_denom_5;
                                    equipoExistente.TotalEquipoDepositoBill10  = model.Total_deposit_denom_10;
                                    equipoExistente.TotalEquipoDepositoBill20  = model.Total_deposit_denom_20;
                                    equipoExistente.TotalEquipoDepositoBill50  = model.Total_deposit_denom_50;
                                    equipoExistente.TotalEquipoDepositoBill100 = model.Total_deposit_denom_100;
                                    equipoExistente.TotalEquipoManualBill1   = model.Total_manual_deposit_denom_1;
                                    equipoExistente.TotalEquipoManualBill2   = model.Total_manual_deposit_denom_2;
                                    equipoExistente.TotalEquipoManualBill5   = model.Total_manual_deposit_denom_5;
                                    equipoExistente.TotalEquipoManualBill10  = model.Total_manual_deposit_denom_10;
                                    equipoExistente.TotalEquipoManualBill20  = model.Total_manual_deposit_denom_20;
                                    equipoExistente.TotalEquipoManualBill50  = model.Total_manual_deposit_denom_50;
                                    equipoExistente.TotalEquipoManualBill100 = model.Total_manual_deposit_denom_100;
                                    equipoExistente.TotalEquipoManualCoin1   = model.Total_manual_deposit_coin_1;
                                    equipoExistente.TotalEquipoManualCoin5   = model.Total_manual_deposit_coin_5;
                                    equipoExistente.TotalEquipoManualCoin10  = model.Total_manual_deposit_coin_10;
                                    equipoExistente.TotalEquipoManualCoin25  = model.Total_manual_deposit_coin_25;
                                    equipoExistente.TotalEquipoManualCoin50  = model.Total_manual_deposit_coin_50;
                                    equipoExistente.TotalEquipoManualCoin100 = model.Total_manual_deposit_coin_100;
                                }
                            }
                            else
                            {
                                await _context.TotalesEquipos.AddAsync(new TotalesEquipos
                                {
                                    Equipo = model.Machine_Sn,
                                    UltimaTransaccion = model.Transaction_no,
                                    FechaUltimaTransaccion = model.Time_generated,
                                    Tipo = "M",
                                    EquipoManualCoin1 = model.Deposit_coin_1,
                                    EquipoManualCoin5 = model.Deposit_coin_5,
                                    EquipoManualCoin10 = model.Deposit_coin_10,
                                    EquipoManualCoin25 = model.Deposit_coin_25,
                                    EquipoManualCoin50 = model.Deposit_coin_50,
                                    EquipoManualCoin100 = model.Deposit_coin_100,
                                    EquipoManualBill1 = model.Deposit_denom_1,
                                    EquipoManualBill2 = model.Deposit_denom_2,
                                    EquipoManualBill5 = model.Deposit_denom_5,
                                    EquipoManualBill10 = model.Deposit_denom_10,
                                    EquipoManualBill20 = model.Deposit_denom_20,
                                    EquipoManualBill50 = model.Deposit_denom_50,
                                    EquipoManualBill100 = model.Deposit_denom_100,
                                    TotalEquipoDepositoBill1 = model.Total_deposit_denom_1,
                                    TotalEquipoDepositoBill2 = model.Total_deposit_denom_2,
                                    TotalEquipoDepositoBill5 = model.Total_deposit_denom_5,
                                    TotalEquipoDepositoBill10 = model.Total_deposit_denom_10,
                                    TotalEquipoDepositoBill20 = model.Total_deposit_denom_20,
                                    TotalEquipoDepositoBill50 = model.Total_deposit_denom_50,
                                    TotalEquipoDepositoBill100 = model.Total_deposit_denom_100,
                                    TotalEquipoManualBill1 = model.Total_manual_deposit_denom_1,
                                    TotalEquipoManualBill2 = model.Total_manual_deposit_denom_2,
                                    TotalEquipoManualBill5 = model.Total_manual_deposit_denom_5,
                                    TotalEquipoManualBill10 = model.Total_manual_deposit_denom_10,
                                    TotalEquipoManualBill20 = model.Total_manual_deposit_denom_20,
                                    TotalEquipoManualBill50 = model.Total_manual_deposit_denom_50,
                                    TotalEquipoManualBill100 = model.Total_manual_deposit_denom_100,
                                    TotalEquipoManualCoin1 = model.Total_manual_deposit_coin_1,
                                    TotalEquipoManualCoin5 = model.Total_manual_deposit_coin_5,
                                    TotalEquipoManualCoin10 = model.Total_manual_deposit_coin_10,
                                    TotalEquipoManualCoin25 = model.Total_manual_deposit_coin_25,
                                    TotalEquipoManualCoin50 = model.Total_manual_deposit_coin_50,
                                    TotalEquipoManualCoin100 = model.Total_manual_deposit_coin_100,
                                    TotalCuadreEquipo = 0
                                });
                            }
                            if (await _context.SaveChangesAsync() > 0)
                            {
                                var resultado = from cli in _context.Clientes
                                                join eq in _context.Equipos
                                                on model.Machine_Sn equals eq.serieEquipo into eqJoin
                                                from eq in eqJoin.DefaultIfEmpty()
                                                join ti in _context.Tiendas
                                                on eq.codigoTiendaidFk equals ti.id into tiJoin
                                                from ti in tiJoin.DefaultIfEmpty()
                                                where cli.CodigoCliente == ti.CodigoClienteidFk
                                                select new
                                                {
                                                    cli.NombreCliente,
                                                    ti.NombreTienda
                                                };

                                var resultado1 = from u in _context.Usuarios
                                                 join dp in _context.Datos_Personales
                                                 on u.Usuario equals dp.UsuarioidFk into dpJoin
                                                 from dp in dpJoin.DefaultIfEmpty()
                                                 join cb in _context.cuentas_bancarias
                                                 on u.CuentasidFk equals cb.id into cbJoin
                                                 from cb in cbJoin.DefaultIfEmpty()
                                                 where u.Usuario == model.User_id
                                                 select new
                                                 {
                                                     dp.Nombres,
                                                     dp.Cedula,
                                                     cb.nombanco,
                                                     cb.TipoCuenta,
                                                     cb.numerocuenta,
                                                     u.Observacion
                                                 };

                                await _context.TransaccionesExcel.AddAsync(new TransaccionesExcel
                                {
                                    FechaTransaccion = model.Time_generated,
                                    Fecha = model.Time_generated.Date,
                                    Hora = model.Time_generated.ToString("HH:mm:ss"),
                                    NombreCliente = resultado.First().NombreCliente,
                                    NombreTienda = resultado.First().NombreTienda,
                                    Transaccion_No = model.Transaction_no,
                                    Machine_Sn = model.Machine_Sn,
                                    Usuarios_idFk = model.User_id,
                                    Establecimiento = resultado1.First().Nombres,
                                    CodigoEstablecimiento = resultado1.First().Cedula,
                                    nombanco = resultado1.First().nombanco,
                                    TipoCuenta = resultado1.First().TipoCuenta,
                                    numerocuenta = resultado1.First().numerocuenta,
                                    Observacion = resultado1.First().Observacion,
                                    Deposito_Bill_1 = model.Deposit_denom_1,
                                    Deposito_Bill_2 = model.Deposit_denom_2,
                                    Deposito_Bill_5 = model.Deposit_denom_5,
                                    Deposito_Bill_10 = model.Deposit_denom_10,
                                    Deposito_Bill_20 = model.Deposit_denom_20,
                                    Deposito_Bill_50 = model.Deposit_denom_50,
                                    Deposito_Bill_100 = model.Deposit_denom_100,
                                    Manual_Deposito_Coin_1 = model.Deposit_coin_1,
                                    Manual_Deposito_Coin_5 = model.Deposit_coin_5,
                                    Manual_Deposito_Coin_10 = model.Deposit_coin_10,
                                    Manual_Deposito_Coin_25 = model.Deposit_coin_25,
                                    Manual_Deposito_Coin_50 = model.Deposit_coin_50,
                                    Manual_Deposito_Coin_100 = model.Deposit_coin_100,
                                    Total = ((model.Deposit_denom_1 * 1) +
                                            (model.Deposit_denom_2 * 2) +
                                            (model.Deposit_denom_5 * 5) +
                                            (model.Deposit_denom_10 * 10) +
                                            (model.Deposit_denom_20 * 20) +
                                            (model.Deposit_denom_50 * 50) +
                                            (model.Deposit_denom_100 * 100) +
                                            (model.Deposit_coin_1 * 0.01m) +
                                            (model.Deposit_coin_5 * 0.05m) +
                                            (model.Deposit_coin_10 * 0.10m) +
                                            (model.Deposit_coin_25 * 0.25m) +
                                            (model.Deposit_coin_50 * 0.50m) +
                                            (model.Deposit_coin_100 * 1m)),
                                    Repetido = "1",
                                    TipoTransaccion = "Manual",
                                    Acreditada = "N"
                                });
                                if (await _context.SaveChangesAsync() > 0)
                                {
                                    await transaction.CommitAsync();
                                    decimal Monto = (model.Deposit_denom_1 * 1) +
                                                    (model.Deposit_denom_2 * 2) +
                                                    (model.Deposit_denom_5 * 5) +
                                                    (model.Deposit_denom_10 * 10) +
                                                    (model.Deposit_denom_20 * 20) +
                                                    (model.Deposit_denom_50 * 50) +
                                                    (model.Deposit_denom_100 * 100) +
                                                    (model.Deposit_coin_1 * 0.01m) +
                                                    (model.Deposit_coin_5 * 0.05m) +
                                                    (model.Deposit_coin_10 * 0.1m) +
                                                    (model.Deposit_coin_25 * 0.25m) +
                                                    (model.Deposit_coin_50 * 0.5m) +
                                                    (model.Deposit_coin_100 * 1m);
                                    int Cant = (model.Deposit_coin_1 +
                                                model.Deposit_coin_5 +
                                                model.Deposit_coin_10 +
                                                model.Deposit_coin_25 +
                                                model.Deposit_coin_50 +
                                                model.Deposit_coin_100);
                                    await _manualesHub.Clients.All.SendAsync("SendTransaccionManual", new { model.Machine_Sn, Cant, Monto, fechaTransaccion = model.Time_generated, model.Transaction_no, tipo = "M" });
                                    return Ok(1);
                                }
                            }
                        }
                    }
                    else
                    {
                        return Ok(2);
                    }
                }
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return Ok(0);
            }
            await transaction.RollbackAsync();
            return Ok(0);
        }        

        [HttpPost("RecoleccionIngresar")]
        public async Task<IActionResult> IngresarRecoleccionAsync([FromBody] ORecoleccion model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                bool verificacion = VerificacionTransaccion(model.User_id, model.Machine_Sn);
                if (verificacion)
                {
                    bool transactionExists = _context.Recolecciones
                                            .Where(d => d.FechaTransaccion >= model.Time_generated.AddDays(-5) && d.Machine_Sn == model.Machine_Sn)
                                            .Any(d => d.Transaccion_No == model.Transaction_no &&
                                                        d.Usuarios_idFk == model.User_id &&
                                                        d.Machine_Sn == model.Machine_Sn);
                    if (!transactionExists)
                    {
                        await _context.Recolecciones.AddAsync(new Recolecciones
                        {
                            Usuarios_idFk = model.User_id,
                            Machine_Sn = model.Machine_Sn,
                            Transaccion_No = model.Transaction_no,
                            FechaTransaccion = model.Time_generated,
                            DivisaTransaccion = "USD",
                            Total_Deposito_Bill_1 = model.Total_deposit_denom_1,
                            Total_Deposito_Bill_2 = model.Total_deposit_denom_2,
                            Total_Deposito_Bill_5 = model.Total_deposit_denom_5,
                            Total_Deposito_Bill_10 = model.Total_deposit_denom_10,
                            Total_Deposito_Bill_20 = model.Total_deposit_denom_20,
                            Total_Deposito_Bill_50 = model.Total_deposit_denom_50,
                            Total_Deposito_Bill_100 = model.Total_deposit_denom_100,
                            Total_Manual_Deposito_Bill_1 = model.Total_manual_deposit_denom_1,
                            Total_Manual_Deposito_Bill_2 = model.Total_manual_deposit_denom_2,
                            Total_Manual_Deposito_Bill_5 = model.Total_manual_deposit_denom_5,
                            Total_Manual_Deposito_Bill_10 = model.Total_manual_deposit_denom_10,
                            Total_Manual_Deposito_Bill_20 = model.Total_manual_deposit_denom_20,
                            Total_Manual_Deposito_Bill_50 = model.Total_manual_deposit_denom_50,
                            Total_Manual_Deposito_Bill_100 = model.Total_manual_deposit_denom_100,
                            Total_Manual_Deposito_Coin_1 = model.Total_manual_deposit_coin_1,
                            Total_Manual_Deposito_Coin_5 = model.Total_manual_deposit_coin_5,
                            Total_Manual_Deposito_Coin_10 = model.Total_manual_deposit_coin_10,
                            Total_Manual_Deposito_Coin_25 = model.Total_manual_deposit_coin_25,
                            Total_Manual_Deposito_Coin_50 = model.Total_manual_deposit_coin_50,
                            Total_Manual_Deposito_Coin_100 = model.Total_manual_deposit_coin_100,
                            Active = "A"
                        });
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            var equipoExiste = await _context.TotalesEquipos.FirstOrDefaultAsync(t => t.Equipo == model.Machine_Sn);

                            if (equipoExiste != null)
                            {
                                decimal? TotalBase = (equipoExiste.EquipoDepositoBill1 * 1) +
                                                     (equipoExiste.EquipoDepositoBill2 * 2) +
                                                     (equipoExiste.EquipoDepositoBill5 * 5) +
                                                     (equipoExiste.EquipoDepositoBill10 * 10) +
                                                     (equipoExiste.EquipoDepositoBill20 * 20) +
                                                     (equipoExiste.EquipoDepositoBill50 * 50) +
                                                     (equipoExiste.EquipoDepositoBill100 * 100) +
                                                     (equipoExiste.EquipoManualBill1 * 1) +
                                                     (equipoExiste.EquipoManualBill2 * 2) +
                                                     (equipoExiste.EquipoManualBill5 * 5) +
                                                     (equipoExiste.EquipoManualBill10 * 10) +
                                                     (equipoExiste.EquipoManualBill20 * 20) +
                                                     (equipoExiste.EquipoManualBill50 * 50) +
                                                     (equipoExiste.EquipoManualBill100 * 100) +
                                                     (equipoExiste.EquipoManualCoin1 * 0.01m) +
                                                     (equipoExiste.EquipoManualCoin5 * 0.05m) +
                                                     (equipoExiste.EquipoManualCoin10 * 0.1m) +
                                                     (equipoExiste.EquipoManualCoin25 * 0.25m) +
                                                     (equipoExiste.EquipoManualCoin50 * 0.5m) +
                                                     (equipoExiste.EquipoManualCoin100 * 1);

                                decimal? TotalEquipo = (model.Total_deposit_denom_1 * 1) +
                                                       (model.Total_deposit_denom_2 * 2) +
                                                       (model.Total_deposit_denom_5 * 5) +
                                                       (model.Total_deposit_denom_10 * 10) +
                                                       (model.Total_deposit_denom_20 * 20) +
                                                       (model.Total_deposit_denom_50 * 50) +
                                                       (model.Total_deposit_denom_100 * 100) +
                                                       (model.Total_manual_deposit_denom_1 * 1) +
                                                       (model.Total_manual_deposit_denom_2 * 2) +
                                                       (model.Total_manual_deposit_denom_5 * 5) +
                                                       (model.Total_manual_deposit_denom_10 * 10) +
                                                       (model.Total_manual_deposit_denom_20 * 20) +
                                                       (model.Total_manual_deposit_denom_50 * 50) +
                                                       (model.Total_manual_deposit_denom_100 * 100) +
                                                       (model.Total_manual_deposit_coin_1 * 0.01m) +
                                                       (model.Total_manual_deposit_coin_5 * 0.05m) +
                                                       (model.Total_manual_deposit_coin_10 * 0.1m) +
                                                       (model.Total_manual_deposit_coin_25 * 0.25m) +
                                                       (model.Total_manual_deposit_coin_50 * 0.5m) +
                                                       (model.Total_manual_deposit_coin_100 * 1);
                                equipoExiste.UltimaTransaccion = model.Transaction_no;
                                equipoExiste.FechaUltimaTransaccion = model.Time_generated;
                                equipoExiste.Tipo = "R";
                                equipoExiste.EquipoDepositoBill1 = 0;
                                equipoExiste.EquipoDepositoBill2 = 0;
                                equipoExiste.EquipoDepositoBill5 = 0;
                                equipoExiste.EquipoDepositoBill10 = 0;
                                equipoExiste.EquipoDepositoBill20 = 0;
                                equipoExiste.EquipoDepositoBill50 = 0;
                                equipoExiste.EquipoDepositoBill100 = 0;
                                equipoExiste.EquipoManualCoin1 = 0;
                                equipoExiste.EquipoManualCoin5 = 0;
                                equipoExiste.EquipoManualCoin10 = 0;
                                equipoExiste.EquipoManualCoin25 = 0;
                                equipoExiste.EquipoManualCoin50 = 0;
                                equipoExiste.EquipoManualCoin100 = 0;
                                equipoExiste.EquipoManualBill1 = 0;
                                equipoExiste.EquipoManualBill2 = 0;
                                equipoExiste.EquipoManualBill5 = 0;
                                equipoExiste.EquipoManualBill10 = 0;
                                equipoExiste.EquipoManualBill20 = 0;
                                equipoExiste.EquipoManualBill50 = 0;
                                equipoExiste.EquipoManualBill100 = 0;
                                equipoExiste.TotalEquipoDepositoBill1 = 0;
                                equipoExiste.TotalEquipoDepositoBill2 = 0;
                                equipoExiste.TotalEquipoDepositoBill5 = 0;
                                equipoExiste.TotalEquipoDepositoBill10 = 0;
                                equipoExiste.TotalEquipoDepositoBill20 = 0;
                                equipoExiste.TotalEquipoDepositoBill50 = 0;
                                equipoExiste.TotalEquipoDepositoBill100 = 0;
                                equipoExiste.TotalEquipoManualBill1 = 0;
                                equipoExiste.TotalEquipoManualBill2 = 0;
                                equipoExiste.TotalEquipoManualBill5 = 0;
                                equipoExiste.TotalEquipoManualBill10 = 0;
                                equipoExiste.TotalEquipoManualBill20 = 0;
                                equipoExiste.TotalEquipoManualBill50 = 0;
                                equipoExiste.TotalEquipoManualBill100 = 0;
                                equipoExiste.TotalEquipoManualCoin1 = 0;
                                equipoExiste.TotalEquipoManualCoin5 = 0;
                                equipoExiste.TotalEquipoManualCoin10 = 0;
                                equipoExiste.TotalEquipoManualCoin25 = 0;
                                equipoExiste.TotalEquipoManualCoin50 = 0;
                                equipoExiste.TotalEquipoManualCoin100 = 0;
                                equipoExiste.FechaUltimaRecoleccion = model.Time_generated;

                                TimeSpan diferencia = (TimeSpan)(model.Time_generated - equipoExiste.FechaUltimaRecoleccion);
                                double minutos = diferencia.TotalMinutes;
                                
                                if (TotalEquipo != TotalBase && minutos > 5 && TotalBase is not null)
                                {
                                    equipoExiste.TotalCuadreEquipo += Math.Abs((decimal)(TotalEquipo - TotalBase));
                                }
                            }
                            if (await _context.SaveChangesAsync() > 0)
                            {
                                await transaction.CommitAsync();
                                decimal Monto = 0;
                                int Cant = 0;
                                await _manualesHub.Clients.All.SendAsync("SendTransaccionManual", new { model.Machine_Sn, Cant, Monto, fechaTransaccion = model.Time_generated, model.Transaction_no, tipo = "R" });
                                return Ok(1);                                                                    
                            }
                        }
                    }
                    else
                    {
                        return Ok(2);
                    }
                }                
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return Ok(0);
            }
            await transaction.RollbackAsync();
            return Ok(0);
        }    

        private bool VerificacionTransaccion(string? user_id, string? machine_Sn)
        {
            bool usuarioExists = _context.Usuarios
                    .Any(u => u.Usuario == user_id && u.Active == "A");

            bool equipoExists = _context.Equipos
                .Any(e => e.serieEquipo == machine_Sn && e.active == "A");

            return usuarioExists && equipoExists;
        }
    }
}