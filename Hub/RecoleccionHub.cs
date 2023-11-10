using Microsoft.AspNetCore.SignalR;

namespace PortalWeb_API.Hub
{
    public class RecoleccionHub : Hub
    {
        public async Task SendTransaccionRecoleccion(List<object> list)
        {
            await Clients.All.SendAsync("SendTransaccionRecoleccion", list);
        }
    }
}
