using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Methods
{
    public class MetodosTotales
    {
        private readonly PortalWebContext _context;
        public MetodosTotales(PortalWebContext context)
        {
            _context = context;
        }

        public TotalUltimaTransaccion[] TotalesUltimaTransaccion(string machine_Sn)
        {
            var datos = _context.TotalesEquipos.Where(d => d.Equipo == machine_Sn).First();
            TotalUltimaTransaccion resultado = new();

            resultado = new TotalUltimaTransaccion
            {
                TotalCantMonedas = datos.TotalEquipoManualCoin1 +
                                                       datos.TotalEquipoManualCoin5 +
                                                       datos.TotalEquipoManualCoin10 +
                                                       datos.TotalEquipoManualCoin25 +
                                                       datos.TotalEquipoManualCoin50 +
                                                       datos.TotalEquipoManualCoin100,
                TotalCantBilletes = datos.TotalEquipoDepositoBill100 +
                                                        datos.TotalEquipoDepositoBill50 +
                                                        datos.TotalEquipoDepositoBill20 +
                                                        datos.TotalEquipoDepositoBill10 +
                                                        datos.TotalEquipoDepositoBill5 +
                                                        datos.TotalEquipoDepositoBill2 +
                                                        datos.TotalEquipoDepositoBill1,
                TotalMont = (double?)((datos.TotalEquipoDepositoBill100 * 100) +
                                                (datos.TotalEquipoDepositoBill50 * 50) +
                                                (datos.TotalEquipoDepositoBill20 * 20) +
                                                (datos.TotalEquipoDepositoBill10 * 10) +
                                                (datos.TotalEquipoDepositoBill5 * 5) +
                                                (datos.TotalEquipoDepositoBill2 * 2) +
                                                (datos.TotalEquipoDepositoBill1 * 1) +
                                                (datos.TotalEquipoManualBill100 * 100) +
                                                (datos.TotalEquipoManualBill50 * 50) +
                                                (datos.TotalEquipoManualBill20 * 20) +
                                                (datos.TotalEquipoManualBill10 * 10) +
                                                (datos.TotalEquipoManualBill5 * 5) +
                                                (datos.TotalEquipoManualBill2 * 2) +
                                                (datos.TotalEquipoManualBill1 * 1) +
                                                (datos.TotalEquipoManualCoin100 * 1) +
                                                (datos.TotalEquipoManualCoin50 * 0.5m) +
                                                (datos.TotalEquipoManualCoin25 * 0.25m) +
                                                (datos.TotalEquipoManualCoin10 * 0.1m) +
                                                (datos.TotalEquipoManualCoin5 * 0.05m) +
                                                (datos.TotalEquipoManualCoin1 * 0.01m))

            };
            return new[] { resultado };
        }

        public TotalTodoTransacciones TotalesTodasTransacciones(string machine_Sn)
        {
            var datos = _context.TotalesEquipos.Where(d => d.Equipo == machine_Sn).First();
            var TotalDepositos = (datos.EquipoDepositoBill1 * 1) +
                                  (datos.EquipoDepositoBill2 * 2) +
                                  (datos.EquipoDepositoBill5 * 5) +
                                  (datos.EquipoDepositoBill10 * 10) +
                                  (datos.EquipoDepositoBill20 * 20) +
                                  (datos.EquipoDepositoBill50 * 50) +
                                  (datos.EquipoDepositoBill100 * 100);
            var TotalManualDepositos = (datos.EquipoManualBill1 * 1) +
                                        (datos.EquipoManualBill2 * 2) +
                                        (datos.EquipoManualBill5 * 5) +
                                        (datos.EquipoManualBill10 * 10) +
                                        (datos.EquipoManualBill20 * 20) +
                                        (datos.EquipoManualBill50 * 50) +
                                        (datos.EquipoManualBill100 * 100) +
                                        (datos.EquipoManualCoin1 * 0.01m) +
                                        (datos.EquipoManualCoin5 * 0.05m) +
                                        (datos.EquipoManualCoin10 * 0.10m) +
                                        (datos.EquipoManualCoin25 * 0.25m) +
                                        (datos.EquipoManualCoin50 * 0.50m) +
                                        (datos.EquipoManualCoin100 * 1);

            return new TotalTodoTransacciones
            {
                Total = (double?)(TotalDepositos + TotalManualDepositos)
            };
        }

        public DateTime? FechaUltimaRecoleccion(string machine_Sn)
        {
            DateTime? fechaRecoleccion = _context.Recolecciones
                .Where(r => r.Machine_Sn == machine_Sn)
                .OrderByDescending(r => r.FechaTransaccion)
                .Select(r => (DateTime?)r.FechaTransaccion)
                .FirstOrDefault() ?? _context.Equipos
                .Where(e => e.serieEquipo == machine_Sn)
                .Select(e => e.fechaInstalacion)
                .FirstOrDefault();
            return fechaRecoleccion;
        }
    }
}