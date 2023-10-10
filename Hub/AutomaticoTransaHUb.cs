using Microsoft.AspNetCore.SignalR;
using PortalWeb_API.Models;

namespace PortalWeb_APIs
{
    public class AutomaticoTransaHUb: Hub
    {
        public async Task SendTransaccionAuto(Depositos model)
        {
            await Clients.All.SendAsync("SendTransaccionAuto", model);
        }
    }
}
