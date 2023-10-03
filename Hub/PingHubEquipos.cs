using Microsoft.AspNetCore.SignalR;
using PortalWeb_API.Models;

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
