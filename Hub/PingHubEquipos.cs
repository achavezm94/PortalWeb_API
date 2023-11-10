using Microsoft.AspNetCore.SignalR;
using PortalWeb_API.Models;

namespace PortalWeb_API.Hub
{
    public class PingHubEquipos : Hub
    {
        public async Task SendPingEquipo(List<MonitoreoModel> monitoreo)
        {
            await Clients.All.SendAsync("SendPingEquipo", monitoreo);
        }
    }
}
