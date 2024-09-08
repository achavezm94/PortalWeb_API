using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/EquipoDetalle")]
    [ApiController]
    public class EquipoTransaccionController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public EquipoTransaccionController(PortalWebContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Transaccional")]
        [HttpGet("ObtenerDetalle/{machineSn}")]
        public IActionResult ObtenerDetalle(string machineSn)
        {
            var datos = _context.TotalesEquipos.Where(d => d.Equipo == machineSn).First();
            var result = new
            {
                machineSn = datos.Equipo,
                DepositoCant100 = datos.TotalEquipoDepositoBill100,
                DepositoCant50 = datos.TotalEquipoDepositoBill50,
                DepositoCant20 = datos.TotalEquipoDepositoBill20,
                DepositoCant10 = datos.TotalEquipoDepositoBill10,
                DepositoCant5 = datos.TotalEquipoDepositoBill5,
                DepositoCant2 = datos.TotalEquipoDepositoBill2,
                DepositoCant1 = datos.TotalEquipoDepositoBill1,
                DepositoMont100 = datos.TotalEquipoDepositoBill100 * 100,
                DepositoMont50 = datos.TotalEquipoDepositoBill50 * 50,
                DepositoMont20 = datos.TotalEquipoDepositoBill20 * 20,
                DepositoMont10 = datos.TotalEquipoDepositoBill10 * 10,
                DepositoMont5 = datos.TotalEquipoDepositoBill5 * 5,
                DepositoMont2 = datos.TotalEquipoDepositoBill2 * 2,
                DepositoMont1 = datos.TotalEquipoDepositoBill1 * 1,
                TotalDepositoCant = datos.TotalEquipoDepositoBill100 +
                                                datos.TotalEquipoDepositoBill50 +
                                                datos.TotalEquipoDepositoBill20 +
                                                datos.TotalEquipoDepositoBill10 +
                                                datos.TotalEquipoDepositoBill5 +
                                                datos.TotalEquipoDepositoBill2 +
                                                datos.TotalEquipoDepositoBill1,
                TotalDepositoMont = (datos.TotalEquipoDepositoBill100 * 100) +
                                                    (datos.TotalEquipoDepositoBill50 * 50) +
                                                    (datos.TotalEquipoDepositoBill20 * 20) +
                                                    (datos.TotalEquipoDepositoBill10 * 10) +
                                                    (datos.TotalEquipoDepositoBill5 * 5) +
                                                    (datos.TotalEquipoDepositoBill2 * 2) +
                                                    (datos.TotalEquipoDepositoBill1 * 1),
                ManualCant100 = datos.TotalEquipoManualBill100,
                ManualCant50 = datos.TotalEquipoManualBill50,
                ManualCant20 = datos.TotalEquipoManualBill20,
                ManualCant10 = datos.TotalEquipoManualBill10,
                ManualCant5 = datos.TotalEquipoManualBill5,
                ManualCant2 = datos.TotalEquipoManualBill2,
                ManualCant1 = datos.TotalEquipoManualBill1,
                ManualMont100 = datos.TotalEquipoManualBill100 * 100,
                ManualMont50 = datos.TotalEquipoManualBill50 * 50,
                ManualMont20 = datos.TotalEquipoManualBill20 * 20,
                ManualMont10 = datos.TotalEquipoManualBill10 * 10,
                ManualMont5 = datos.TotalEquipoManualBill5 * 5,
                ManualMont2 = datos.TotalEquipoManualBill2 * 2,
                ManualMont1 = datos.TotalEquipoManualBill1 * 1,
                ManualCantCoin100 = datos.TotalEquipoManualCoin100,
                ManualCantCoin50 = datos.TotalEquipoManualCoin50,
                ManualCantCoin25 = datos.TotalEquipoManualCoin25,
                ManualCantCoin10 = datos.TotalEquipoManualCoin10,
                ManualCantCoin5 = datos.TotalEquipoManualCoin5,
                ManualCantCoin1 = datos.TotalEquipoManualCoin1,
                ManualMontCoin100 = datos.TotalEquipoManualCoin100 * 1,
                ManualMontCoin50 = datos.TotalEquipoManualCoin50 * 0.5m,
                ManualMontCoin25 = datos.TotalEquipoManualCoin25 * 0.25m,
                ManualMontCoin10 = datos.TotalEquipoManualCoin10 * 0.1m,
                ManualMontCoin5 = datos.TotalEquipoManualCoin5 * 0.05m,
                ManualMontCoin1 = datos.TotalEquipoManualCoin1 * 0.01m,
                TotalManualBillCant = datos.TotalEquipoManualBill100 +
                                                    datos.TotalEquipoManualBill50 +
                                                    datos.TotalEquipoManualBill20 +
                                                    datos.TotalEquipoManualBill10 +
                                                    datos.TotalEquipoManualBill5 +
                                                    datos.TotalEquipoManualBill2 +
                                                    datos.TotalEquipoManualBill1,
                TotalManualBillMont = (datos.TotalEquipoManualBill100 * 100) +
                                                    (datos.TotalEquipoManualBill50 * 50) +
                                                    (datos.TotalEquipoManualBill20 * 20) +
                                                    (datos.TotalEquipoManualBill10 * 10) +
                                                    (datos.TotalEquipoManualBill5 * 5) +
                                                    (datos.TotalEquipoManualBill2 * 2) +
                                                    (datos.TotalEquipoManualBill1 * 1),
                TotalManualCoinCant = datos.TotalEquipoManualCoin100 +
                                                        datos.TotalEquipoManualCoin50 +
                                                        datos.TotalEquipoManualCoin25 +
                                                        datos.TotalEquipoManualCoin10 +
                                                        datos.TotalEquipoManualCoin5 +
                                                        datos.TotalEquipoManualCoin1,
                TotalManualCoinMont = (datos.TotalEquipoManualCoin100 * 1) +
                                                        (datos.TotalEquipoManualCoin50 * 0.5m) +
                                                        (datos.TotalEquipoManualCoin25 * 0.25m) +
                                                        (datos.TotalEquipoManualCoin10 * 0.1m) +
                                                        (datos.TotalEquipoManualCoin5 * 0.05m) +
                                                        (datos.TotalEquipoManualCoin1 * 0.01m)
            };
            var resultList = new List<object> { result };
            return Ok(resultList);                       
        }
    }
}