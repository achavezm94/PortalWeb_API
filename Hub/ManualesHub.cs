using Microsoft.AspNetCore.SignalR;

namespace PortalWeb_APIs
{
    public class ManualesHub : Hub
    {
        public async Task SendTransaccionManual(List<object> list)
        {
            await Clients.All.SendAsync("SendTransaccionManual", list);
        }
    }
}
