using Microsoft.AspNetCore.SignalR;
using PortalWeb_API.Models;

namespace PortalWeb_API
{
    public class ActualizarHub : Hub
    {
        public async Task UpdateUsuarioTemporal(UsuariosTemporales model)
        {
            await Clients.All.SendAsync("UpdateUsuarioTemporal", model);
        }
    }
}
