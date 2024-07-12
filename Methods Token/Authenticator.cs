using PortalWeb_API.Data;
using PortalWeb_API.Models;

namespace PortalWeb_API.Methods_Token
{
    public class Authenticator
    {
        private readonly PortalWebContext? _context;
        public Authenticator(PortalWebContext context)
        {
            _context = context;
        }

        public UsuariosPortal? Authenticator_User(UserRequest userRequest)
        {
            var usuario = _context?.UsuariosPortal.Where(x => x.Usuario == userRequest.Usuario && x.Contrasenia == userRequest.Password && x.Active == "A").FirstOrDefault();       
            if (usuario != null)
            {
                return usuario;
            }
            else
            {
                return null;
            }
        }
    }
}
