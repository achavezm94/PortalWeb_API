using Microsoft.AspNetCore.SignalR;

namespace PortalWeb_API
{
    public class PingHubEquipos: Hub
    {
        //Equipos equipo
        public async Task SendPingEquipo()
        {
            await Clients.All.SendAsync("SendPingEquipo", "HOLAAAAAAAAaa");
        }
    }
}
