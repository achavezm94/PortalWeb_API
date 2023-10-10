using Microsoft.AspNetCore.SignalR;

namespace PortalWeb_API
{
    public class PingHubEquipos: Hub
    {
        public async Task SendPingEquipo()
        {
            await Clients.All.SendAsync("SendPingEquipo");
        }
    }
}
