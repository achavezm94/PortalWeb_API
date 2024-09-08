using Microsoft.AspNetCore.SignalR;

namespace PortalWeb_APIs
{
    public class ManualesHub : Hub
    {
        public async Task SendTransaccionManual(string machine_Sn, int cant, decimal monto, DateTime fechaTransaccion, string transactionNo, string tipo)
        {
            await Clients.All.SendAsync("SendTransaccionManual", machine_Sn, cant, monto, fechaTransaccion, transactionNo, tipo);
        }
    }
}
