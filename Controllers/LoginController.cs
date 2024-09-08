using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Methods_Token;
using PortalWeb_API.Models;


namespace PortalWeb_API.Controllers
{
    [Route("api/Login")]
    [ApiController]

    public class LoginController : ControllerBase
    {
        
        public IConfiguration _configuration;
        private readonly PortalWebContext _context;
        public LoginController(PortalWebContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        
        public IActionResult Login(UserRequest userRequest) 
        {
            try
            {
                GenerateToken generateToken = new(_configuration);

                var usuario = _context?.Usuarios_Portal.SingleOrDefault(x => x.Usuario == userRequest.Usuario);
                if (usuario == null)
                {
                    return NotFound(new { Message = "Usuario incorrecto" });
                }
                else if (usuario.Contrasenia != userRequest.Password)
                {
                    return NotFound(new { Message = "Contraseña incorrecta" });
                }
                else if (usuario.Active != "A")
                {
                    return NotFound(new { Message = "El usuario no está activo." });
                }
                var token = generateToken.Generate(usuario);
                return Ok(new { Token = token});

            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al procesar la solicitud." });
            }
        }
    }
}