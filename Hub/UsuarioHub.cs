using Microsoft.AspNetCore.SignalR;
using PortalWeb_API.Models;

namespace PortalWeb_APIs
{
    public class UsuarioHub :Hub
    {
        public async Task SendUsuarioTemporal(UsuariosTemporales model)
        {
            await Clients.All.SendAsync("SendUsuarioTemporal", model);
        }
    }
}
