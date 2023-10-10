using Microsoft.AspNetCore.SignalR;

namespace PortalWeb_APIs
{
    public class PingHubEquipos: Hub
    {
        public async Task SendPingEquipo()
        {
            await Clients.All.SendAsync("SendPingEquipo");
        }
    }
}
