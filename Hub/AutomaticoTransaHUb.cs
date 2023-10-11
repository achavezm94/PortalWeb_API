using Microsoft.AspNetCore.SignalR;
using PortalWeb_API.Models;

namespace PortalWeb_APIs
{
    public class AutomaticoTransaHUb: Hub
    {
        public async Task SendTransaccionAuto(Depositos model, SP_TablaTransaccionalPrincipalResult response)
        {
            await Clients.All.SendAsync("SendTransaccionAuto", model, response);
        }
    }
}
