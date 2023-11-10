using Microsoft.AspNetCore.SignalR;
using PortalWeb_API.Models;

namespace PortalWeb_API.Hub
{
    public class EliminarUsuario : Hub
    {

        public async Task EliminarUsuarioTemporal(UsuariosTemporales model)
        {
            await Clients.All.SendAsync("EliminarUsuarioTemporal", model);
        }
    }
}
