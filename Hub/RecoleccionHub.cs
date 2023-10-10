using Microsoft.AspNetCore.SignalR;
using PortalWeb_API.Models;

namespace PortalWeb_APIs
{
    public class RecoleccionHub : Hub
    {
        public async Task SendTransaccionRecoleccion(Recolecciones model)
        {
            await Clients.All.SendAsync("SendTransaccionRecoleccion", model);
        }
    }
}
