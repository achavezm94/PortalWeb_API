using Microsoft.AspNetCore.SignalR;
using PortalWeb_API.Models;

namespace PortalWeb_APIs
{
    public class ManualesHub: Hub
    {
        public async Task SendTransaccionManual( ManualDepositos model )
        {
            await Clients.All.SendAsync( "SendTransaccionManual", model );
        }
    }
}
