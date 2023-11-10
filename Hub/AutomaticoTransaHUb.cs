using Microsoft.AspNetCore.SignalR;

namespace PortalWeb_API.Hub
{
    public class AutomaticoTransaHUb : Hub
    {
        public async Task SendTransaccionAuto(List<object> list)
        {
            await Clients.All.SendAsync("SendTransaccionAuto", list);
        }
    }
}
